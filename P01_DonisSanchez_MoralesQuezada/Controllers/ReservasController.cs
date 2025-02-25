using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_DonisSanchez_MoralesQuezada.Modelos;

namespace P01_DonisSanchez_MoralesQuezada.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ParqueoDBcontext _parqueoContexto;

        public ReservasController(ParqueoDBcontext parqueoContexto)
        {
            _parqueoContexto = parqueoContexto;
        }

        [HttpPost]
        [Route("reservar/{UsuarioId}/{EspacioParqueoId}/{fecha}/{CantidadHoras}")]
        public IActionResult ReservarEspacio(int UsuarioId, int EspacioParqueoId, string fecha, int CantidadHoras)
        {
            try
            {
                // Validar datos de entrada
                if (UsuarioId <= 0 || EspacioParqueoId <= 0 || CantidadHoras <= 0 || string.IsNullOrEmpty(fecha))
                {
                    return BadRequest(new { message = "Todos los campos son obligatorios y deben ser válidos." });
                }

                // Verificar si el usuario existe
                var usuario = _parqueoContexto.usuarios.FirstOrDefault(u => u.Id == UsuarioId);
                if (usuario == null)
                {
                    return BadRequest(new { message = "El usuario no existe." });
                }

                // Verificar si el espacio de parqueo existe
                var espacio = _parqueoContexto.EspaciosParqueo.FirstOrDefault(e => e.Id == EspacioParqueoId);
                if (espacio == null)
                {
                    return BadRequest(new { message = "El espacio de parqueo no existe." });
                }

                // Convertir la fecha a DateTime (si es necesario)
                DateTime fechaReserva;
                if (!DateTime.TryParse(fecha, out fechaReserva))
                {
                    return BadRequest(new { message = "La fecha proporcionada no es válida." });
                }

                // Verificar si el espacio ya está reservado en esa fecha
                var reservaExistente = _parqueoContexto.reservas.FirstOrDefault(r =>
                    r.EspacioParqueoId == EspacioParqueoId &&
                    r.Fecha == fechaReserva);

                if (reservaExistente != null)
                {
                    return BadRequest(new { message = "El espacio ya está reservado en esa fecha." });
                }

                // Crear una nueva reserva
                Reservas nuevaReserva = new Reservas
                {
                    UsuarioId = UsuarioId,
                    EspacioParqueoId = EspacioParqueoId,
                    Fecha = fechaReserva,
                    CantidadHoras = CantidadHoras
                };

                // Guardar la reserva en la base de datos
                _parqueoContexto.reservas.Add(nuevaReserva);
                _parqueoContexto.SaveChanges();

                // Obtener los nombres del usuario y espacio
                var nombreUsuario = usuario.Nombre;
                var nombreEspacio = espacio.Ubicacion;

                // Respuesta exitosa con los nombres en lugar de los ID
                return Ok(new
                {
                    message = "Reserva realizada con éxito.",
                    reserva = new
                    {
                        Usuario = nombreUsuario,
                        EspacioParqueo = nombreEspacio,
                        Fecha = fechaReserva,
                        CantidadHoras = nuevaReserva.CantidadHoras
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
