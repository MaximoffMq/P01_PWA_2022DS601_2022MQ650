using System.ComponentModel.DataAnnotations;
namespace P01_DonisSanchez_MoralesQuezada.Modelos
{
    public class EspaciosParqueo
    {
        [Key]
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Ubicacion { get; set; }
        public decimal CostoPorHora { get; set; }
        public int SucursalId { get; set; }
    }
}
