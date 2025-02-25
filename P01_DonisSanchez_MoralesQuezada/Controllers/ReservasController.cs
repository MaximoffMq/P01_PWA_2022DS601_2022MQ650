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
        [Route("reservar")]
        public IActionResult ReservarEspacio([FromBody] Reservas nuevaReserva)
        {
            try
            {
                // Validar datos de entrada
                if (nuevaReserva.UsuarioId <= 0 || nuevaReserva.EspacioParqueoId <= 0 || nuevaReserva.CantidadHoras <= 0)
                {
                    return BadRequest(new { message = "Todos los campos son obligatorios y deben ser válidos." });
                }

                // Verificar si el usuario existe
                var usuario = _parqueoContexto.usuarios.FirstOrDefault(u => u.Id == nuevaReserva.UsuarioId);
                if (usuario == null)
                {
                    return BadRequest(new { message = "El usuario no existe." });
                }

                // Verificar si el espacio de parqueo existe
                var espacio = _parqueoContexto.EspaciosParqueo.FirstOrDefault(e => e.Id == nuevaReserva.EspacioParqueoId);
                if (espacio == null)
                {
                    return BadRequest(new { message = "El espacio de parqueo no existe." });
                }

                // Verificar si el espacio ya está reservado en esa fecha
                var reservaExistente = _parqueoContexto.reservas.FirstOrDefault(r =>
                    r.EspacioParqueoId == nuevaReserva.EspacioParqueoId &&
                    r.Fecha == nuevaReserva.Fecha);

                if (reservaExistente != null)
                {
                    return BadRequest(new { message = "El espacio ya está reservado en esa fecha." });
                }

                // Guardar la reserva en la base de datos
                _parqueoContexto.reservas.Add(nuevaReserva);
                _parqueoContexto.SaveChanges();

                return Ok(new { message = "Reserva realizada con éxito.", reserva = nuevaReserva });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
