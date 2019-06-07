using Entidades;

namespace AsesoriasApp
{
    public static class Variables
    {
        public static string CveMateria { get; set; } = string.Empty;
        public static string DescripcionMateria { get; set; } = string.Empty;
        public static string CorreoUsuario { get; set; } = string.Empty;
        public static string MateriaSeleccionada { get; set; } = string.Empty;
        public static UsuarioEntity Usuario { get; set; } = null; 
        public static string ModoUsuario { get; set; } = string.Empty;

        public static string MODO_ALUMNO  = "MODO_ALUMNO";
        public static string MODO_ASESOR = "MODO_ASESOR";
    }
}