using System.ComponentModel.DataAnnotations;
namespace P01_DonisSanchez_MoralesQuezada.Modelos
{
    public class Sucursales
    {
        [Key]

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int AdministradorId { get; set; }

    }
}
