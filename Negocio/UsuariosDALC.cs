
using Android.App;
using Android.Content;
using DataAccess;
using Entidades;
using Mono.Data.Sqlite;
using System;
using System.Data;

namespace Negocio
{
    public class UsuariosDALC : ConnectionClass
    {
        public UsuariosDALC(string packagename, Context context, Activity activity) : base(packagename, context, activity)
        { }

        public UsuarioEntity MostrarUsuario(string correo, string password)
        {
            UsuarioEntity entidad = new UsuarioEntity();
            SqliteCommand command = new SqliteCommand("select *from usuarios where correo = @correo and password = @password", Connection);
            command.Parameters.AddWithValue("@correo", correo);
            command.Parameters.AddWithValue("@password", password);
            SqliteDataAdapter da = new SqliteDataAdapter(command);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    Popule(dr, entidad);
            }
            catch(Exception ex)
            {
                entidad = null;
                MuestraErrorDialog(ex, "Error al mostrar usuario.");
            }

            return entidad;
        }

        public UsuarioEntity MostrarUsuario(int idAsesor)
        {
            UsuarioEntity entidad = new UsuarioEntity();
            SqliteCommand command = new SqliteCommand("select *from usuarios where id = @id", Connection);
            command.Parameters.AddWithValue("@id", idAsesor);
            SqliteDataAdapter da = new SqliteDataAdapter(command);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    Popule(dr, entidad);
            }
            catch (Exception ex)
            {
                entidad = null;
                MuestraErrorDialog(ex, "Error al mostrar usuario.");
            }

            return entidad;
        }

        private void Popule(DataRow dr, UsuarioEntity entidad)
        {
            entidad.Activo = Convert.ToInt32(dr["activo"]);
            //entidad.AlumnosAsesorados = dr["alumnos_asesorados"].ToString();
            entidad.Carrera = dr["carrera"].ToString();
            entidad.Correo = dr["correo"].ToString();
            entidad.Password = dr["password"].ToString();
            entidad.Experiencia = dr["experiencia"].ToString();
            entidad.Facultad = dr["facultad"].ToString();
            entidad.IdUsuario = Convert.ToInt32(dr["id"].ToString());
            //entidad.Medallas = dr[""].ToString();
            entidad.Nivel = dr["nivel"].ToString();
            entidad.Nombres = dr["nombres"].ToString();
            entidad.Apellidos = dr["apellidos"].ToString();
            entidad.Score = dr["score"].ToString();
            entidad.Semestre = dr["semestre"].ToString();
            entidad.Telefono = dr["telefono"].ToString();
        }

        public bool InsertarUsuario(UsuarioEntity usuario)
        {
            bool success = false;
            string query = "insert into usuarios(activo, alumnos_asesorados, apellidos, carrera, correo, experiencia, facultad, medallas, nivel, " +
                "nombres, password, score, semestre, telefono) values (@activo, @alumnos_asesorados, @apellidos, @carrera, @correo, @experiencia, @facultad, @medallas, " +
                    "@nivel, @nombres, @password, @score, @semestre, @telefono);";
            SqliteCommand command = new SqliteCommand(query, Connection);

            command.Parameters.AddWithValue("@activo", usuario.Activo);
            command.Parameters.AddWithValue("@alumnos_asesorados", usuario.AlumnosAsesorados);
            command.Parameters.AddWithValue("@apellidos", usuario.Apellidos);
            command.Parameters.AddWithValue("@carrera", usuario.Carrera);
            command.Parameters.AddWithValue("@correo", usuario.Correo);
            command.Parameters.AddWithValue("@experiencia", usuario.Experiencia);
            command.Parameters.AddWithValue("@facultad", usuario.Facultad);
            command.Parameters.AddWithValue("@medallas", usuario.Medallas);
            command.Parameters.AddWithValue("@nivel", usuario.Nivel);
            command.Parameters.AddWithValue("@nombres", usuario.Nombres);
            command.Parameters.AddWithValue("@password", usuario.Password);
            command.Parameters.AddWithValue("@score", usuario.Score);
            command.Parameters.AddWithValue("@semestre", usuario.Semestre);
            command.Parameters.AddWithValue("@telefono", usuario.Telefono);
 
            try
            {
                Connection.Open();
                if (command.ExecuteNonQuery() > 0)
                    success = true;
            }
            catch(Exception ex)
            {
                MuestraErrorDialog(ex, "Error al registrar usuario.");
            }
            finally
            {
                Connection.Close();
            }

            return success;
        }
    }
}