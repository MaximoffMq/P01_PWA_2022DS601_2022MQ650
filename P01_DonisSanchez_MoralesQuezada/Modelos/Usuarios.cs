using System.ComponentModel.DataAnnotations;
namespace P01_DonisSanchez_MoralesQuezada.Modelos
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Contrasena { get; set; } // Encriptada
        public string Rol { get; set; }

    }
}
