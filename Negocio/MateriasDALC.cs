using System;
using System.Collections.Generic;
using System.Data;
using Android.App;
using Android.Content;
using DataAccess;
using Entidades;
using Mono.Data.Sqlite;

namespace Negocio
{
    public class MateriasDALC : ConnectionClass
    {
        public MateriasDALC(string packagename, Context context, Activity activity) : base(packagename, context, activity)
        { }

        public List<MateriaEntity> MostrarMaterias()
        {
            List<MateriaEntity> lst = new List<MateriaEntity>();
            SqliteCommand command = new SqliteCommand("select *from materias order by nombre", Connection);
            SqliteDataAdapter da = new SqliteDataAdapter(command);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    MateriaEntity entidad = new MateriaEntity();
                    Popule(dr, entidad);
                    lst.Add(entidad);
                }
            }
            catch(Exception ex)
            {
                MuestraErrorDialog(ex, "Error al mostrar materias.");
            }

            return lst;
        }

        private void Popule(DataRow dr, MateriaEntity entidad)
        {
            entidad.id = Convert.ToInt32(dr["id"]);
            entidad.Nombre = dr["nombre"].ToString();

            if(!string.IsNullOrEmpty(dr["numero_solicitudes"].ToString()))
                entidad.NumeroSolicitudes = Convert.ToInt32(dr["numero_solicitudes"]);
        }

        public bool InsertarMaterias(string nombre)
        {
            bool success = false;
            SqliteCommand command = new SqliteCommand("insert into materias(nombre) values (@nombre)", Connection);
            command.Parameters.AddWithValue("@nombre", nombre);

            try
            {
                Connection.Open();
                if (command.ExecuteNonQuery() > 0)
                    success = true;
            }
            catch (Exception ex)
            {
                MuestraErrorDialog(ex, "Error al generar grupo de asesoría");
            }
            finally
            {
                if (Connection.State == ConnectionState.Open || Connection.State == ConnectionState.Broken)
                    Connection.Close();
            }

            return success;
        }
    }
}