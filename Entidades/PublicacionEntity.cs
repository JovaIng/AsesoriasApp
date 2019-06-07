namespace Negocio
{
    public class PublicacionEntity
    {
        public int Id;
        public int IdAsesor;
        public string Materia { get; set; } = string.Empty;
        public string DescripciónCurso { get; set; } = string.Empty;
        public string TipoCobro { get; set; } = string.Empty;
        public int Capacidad { get; set; } = 0;
        public string Modo { get; set; } = string.Empty;

        public GrupoAsesoriaEntity() { }
    }
}