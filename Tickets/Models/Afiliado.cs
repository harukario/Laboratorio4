namespace Regularidad.Models
{
    public class Afiliado
    {
        public int Id { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int  Dni { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? foto { get; set; }

        public string NombreCompleto => $"{Nombres} {Apellido}";

    }
}
