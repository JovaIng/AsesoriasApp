using Android.App;
using Android.Content;
using DataAccess;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Negocio
{
    public class PublicacionesDALC : ConnectionClass
    {
        public PublicacionesDALC(string packagename, Context context, Activity activity) : base(packagename, context, activity)
        { }

        public bool InsertarPublicacion(PublicacionEntity entidad)
        {
            bool success = false;
            SqliteCommand command = new SqliteCommand("insert into publicaciones(descripcion, tipo_cobro, id_usuario, capacidad, materia, modo) " +
                "values (@descripcion, @tipo_cobro, @id_usuario, @capacidad, @materia, @modo)", Connection);

            command.Parameters.AddWithValue("@materia", entidad.Materia);
            command.Parameters.AddWithValue("@descripcion", entidad.DescripciónCurso);
            command.Parameters.AddWithValue("@tipo_cobro", entidad.TipoCobro);
            command.Parameters.AddWithValue("@id_usuario", entidad.IdUsuario);
            command.Parameters.AddWithValue("@capacidad", entidad.Capacidad);
            command.Parameters.AddWithValue("@modo", entidad.Modo);

            try
            {
                Connection.Open();
                if (command.ExecuteNonQuery() > 0)
                    success = true;
            }
            catch(Exception ex)
            {
                MuestraErrorDialog(ex, "Error al generar grupo de asesoría");
            }

            return success;
        }

        public List<PublicacionEntity> MostrarPublicaciones(string materia, string modo)
        {
            SqliteCommand command = new SqliteCommand("select *from publicaciones where materia = @materia and modo = @modo", Connection);
            command.Parameters.AddWithValue("@materia", materia);
            command.Parameters.AddWithValue("@modo", modo);
            SqliteDataAdapter da = new SqliteDataAdapter(command);
            DataSet ds = new DataSet();
            List<PublicacionEntity> lst = new List<PublicacionEntity>();

            try
            {
                da.Fill(ds);
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    PublicacionEntity entidad = new PublicacionEntity();
                    Popule(dr, entidad);
                    lst.Add(entidad);
                }
            }
            catch (Exception ex)
            {
                MuestraErrorDialog(ex, "Error al mostrar asesores");
            }

            return lst;
        }

        private void Popule(DataRow dr, PublicacionEntity entidad)
        {
            entidad.Materia = dr["materia"].ToString();
            entidad.DescripciónCurso = dr["descripcion"].ToString();
            entidad.Id = Convert.ToInt32(dr["id"]);
            entidad.IdUsuario = Convert.ToInt32(dr["id_usuario"]);
            entidad.TipoCobro = dr["tipo_cobro"].ToString();
            entidad.Modo = dr["modo"].ToString();
        }
    }
}