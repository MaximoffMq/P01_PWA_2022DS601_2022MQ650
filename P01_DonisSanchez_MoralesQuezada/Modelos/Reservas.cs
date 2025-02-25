using System.ComponentModel.DataAnnotations;
namespace P01_DonisSanchez_MoralesQuezada.Modelos
{
    public class Reservas
    {
        [Key]

        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EspacioParqueoId { get; set; }
        public DateTime Fecha { get; set; } //yyyy-MM-dd
        public int CantidadHoras { get; set; }
        public int Cancelada { get; set; }
    }
}
