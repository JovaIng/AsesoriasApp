using Android.App;
using Android.Content;
using Android.Preferences;
using Mono.Data.Sqlite;
using System;
using System.Data;
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
            if(!File.Exists(_databasePath))
            {
                File.Create(_databasePath);
                GeneraTablas();
            }
            Connection = new SqliteConnection(connectionString);  
        }

        private void GeneraTablas()
        {
            string connectionString = string.Format("Data source={0},Compress=true", _databasePath);
            SqliteConnection Connection = new SqliteConnection(connectionString);
            SqliteCommand command = new SqliteCommand("CREATE TABLE `materias` (id INTEGER PRIMARY KEY AUTOINCREMENT, nombre varchar(255), numero_solicitudes INTEGER); ", Connection);
            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            { }
            catch (Exception ex)
            { }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            Connection = new SqliteConnection(connectionString);
            command = new SqliteCommand("CREATE TABLE publicaciones(id INTEGER PRIMARY KEY AUTOINCREMENT, id_usuario INTEGER, materia varchar(255), descripcion varchar(255)," +
                " tipo_cobro varchar(255), capacidad INTEGER, modo varchar(255))", Connection);

            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            { }
            catch (Exception ex)
            { }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

            Connection = new SqliteConnection(connectionString);
            command = new SqliteCommand("CREATE TABLE usuarios (id	INTEGER PRIMARY KEY AUTOINCREMENT, nombres	varchar(255), apellidos	varchar(255), correo archar(255), " +
                "password	varchar(255), telefono	varchar(50), semestre varchar(10), facultad	varchar(255), activo bit, alumnos_asesorados varchar(255), carrera varchar(255), " +
                  "experiencia varchar(255), medallas varchar(255), nivel varchar(50), score varchar(50))", Connection);
            try
            {
                Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            { }
            catch (Exception ex)
            { }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
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