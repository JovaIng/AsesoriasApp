using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Entidades
{
    public class UsuarioEntity
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Facultad { get; set; } = string.Empty;
        public string Semestre { get; set; } = string.Empty;
        public string Carrera { get; set; } = string.Empty;
        public string Experiencia { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string Medallas { get; set; } = string.Empty;
        public string AlumnosAsesorados { get; set; } = string.Empty;
        public int Activo { get; set; }

        public UsuarioEntity()
        {
            Nivel = "1";
            Score = "0";
            Activo = 1;
        }
    }
}