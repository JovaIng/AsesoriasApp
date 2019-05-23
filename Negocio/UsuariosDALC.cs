
using Android.App;
using Android.Content;
using DataAccess;
using Entidades;
using System;
using System.Data;
using System.Data.SQLite;

namespace Negocio
{
    public class UsuariosDALC : ConnectionClass
    {
        public UsuariosDALC(string packagename, Context context, Activity activity) : base(packagename, context, activity)
        { }

        public UsuarioEntity MostrarUsuario(string correo, string password)
        {
            UsuarioEntity entidad = new UsuarioEntity();
            SQLiteCommand command = new SQLiteCommand("select *from usuarios where ", Connection);
            command.Parameters.AddWithValue("@correo", correo);
            command.Parameters.AddWithValue("@password", password);
            SQLiteDataAdapter da = new SQLiteDataAdapter(command);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    Popule(dr, entidad);
            }
            catch(Exception ex)
            {

            }

            return entidad;
        }

        private void Popule(DataRow dr, UsuarioEntity entidad)
        {
            entidad.Activo = Convert.ToInt32(dr["activo"]);
            //entidad.AlumnosAsesorados = dr["alumnos_asesorados"].ToString();
            entidad.Carrera = dr[""].ToString();
            entidad.Correo = dr[""].ToString();
            entidad.Experiencia = dr[""].ToString();
            entidad.Facultad = dr[""].ToString();
            entidad.IdUsuario = Convert.ToInt32(dr[""].ToString());
            //entidad.Medallas = dr[""].ToString();
            entidad.Nivel = dr[""].ToString();
            entidad.Nombre = dr[""].ToString();
            entidad.Score = dr[""].ToString();
            entidad.Semestre = dr[""].ToString();
            entidad.Telefono = dr[""].ToString();
        }
    }
}