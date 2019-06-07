using Android.App;
using Android.Content;
using Android.Preferences;
using Mono.Data.Sqlite;
using System;
using System.IO;

namespace DataAccess
{
    public class ConnectionClass
    {
        public SqliteConnection Connection = null;
        public string LogsPath;

        public string Packagename { get; set; }
        public Context ApplicationContext { get; set; }
        public Activity Activity { get; set; } = null;

        private string _dbFile = string.Empty;
        private string _dbFileBak = string.Empty;
        private string _appPath = string.Empty;
        private string _logsPath = string.Empty;
        private string _databasePath = string.Empty;

        public string DbPath { get => _dbFile; set => _dbFile = value; }
        public string DbFileBak { get => _dbFileBak; set => _dbFileBak = value; }
        public string DatabasePath { get => _databasePath; set => _databasePath = value; }
        public string AppPath { get => _appPath; set => _appPath = value; }

        public ConnectionClass(string packageName, Context context, Activity activity)
        {
            Packagename = packageName;
            ApplicationContext = context;
            Activity = activity;

            string dbName = "data.db";
            AppPath = string.Format("/data/data/{0}", Packagename);
            _logsPath = string.Format("/data/data/{0}/Logs", Packagename);
            _databasePath = string.Format("/data/data/{0}/DataBase", Packagename);
            string folderPersonal = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            try
            {
                if (!Directory.Exists(_logsPath))
                    Directory.CreateDirectory(_logsPath);
                // Validar privilegios
                string dummyLog = string.Format("{0}/dummy.txt", _logsPath);
                File.WriteAllText(dummyLog, string.Empty);
                if (File.Exists(dummyLog))
                    File.Delete(dummyLog);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logsPath = string.Format("{0}/logs", folderPersonal);
                _databasePath = string.Format("{0}/database", folderPersonal);
            }

            try
            {
                Java.IO.File javafile = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                folderPersonal = javafile.AbsolutePath;
                string databasePath = string.Format("{0}/database", folderPersonal);
                string logsPath = string.Format("{0}/logs", folderPersonal);
                if (File.Exists(string.Format("{0}/{1}.db", databasePath, dbName)))
                {
                    _logsPath = logsPath;
                    _databasePath = databasePath;
                }
            }
            catch { }

            if (!Directory.Exists(_logsPath))
                Directory.CreateDirectory(_logsPath);
            if (!Directory.Exists(_databasePath))
                Directory.CreateDirectory(_databasePath);

            _databasePath += ("/" + dbName);
            string connectionString = string.Format("Data source={0};New=false; Compress=true", _databasePath);
            Connection = new SqliteConnection(connectionString);  
        }

        public void MuestraErrorDialog(Exception ex, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetCancelable(false);
            builder.SetTitle(title);
            builder.SetMessage("Exception: " + ex.Message);
            builder.SetPositiveButton("", delegate {
                builder.Dispose();
            });
            builder.SetPositiveButton("Aceptar", delegate {
                builder.Dispose();
            });
            Activity.RunOnUiThread(() => { builder.Show(); });
        }
    }
}