using Android.App;
using Android.Content;
using Android.Preferences;
using System.Data.SQLite;

namespace DataAccess
{
    public class ConnectionClass
    {
        public SQLiteConnection Connection = null;
        public string LogsPath;
  
        public ConnectionClass(string packageName, Context context, Activity activity)
        {
            Packagename = packageName;
            ApplicationContext = context;
            Activity = activity;

            string databasePath = string.Format("/data/data/{0}/DataBase", packageName);
            string connectionString = string.Format("Data source={0};New=false; Compress=true", databasePath);
            Connection = new SQLiteConnection(connectionString);  
        }

        public string Packagename { get; set; }
        public Context ApplicationContext { get; set; }
        public Activity Activity { get; set; } = null;
    }
}