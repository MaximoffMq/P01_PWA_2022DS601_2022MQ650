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


        //reservaciones por dia
        [HttpGet]
        [Route("reservasPorDia/{fecha}")]
        public IActionResult ObtenerReservasPorDia(string fecha)
        {
            try
            {
                // Convertir la fecha a DateTime
                DateTime fechaReserva;
                if (!DateTime.TryParse(fecha, out fechaReserva))
                {
                    return BadRequest(new { message = "La fecha proporcionada no es válida." });
                }

                // Obtener las reservas para esa fecha
                var reservas = _parqueoContexto.reservas
                    .Where(r => r.Fecha.Date == fechaReserva.Date) // Asegurarse de que solo se obtiene la fecha sin la hora
                    .Join(_parqueoContexto.usuarios, r => r.UsuarioId, u => u.Id, (r, u) => new { r, u })
                    .Join(_parqueoContexto.EspaciosParqueo, ru => ru.r.EspacioParqueoId, e => e.Id, (ru, e) => new { ru, e })
                    .Join(_parqueoContexto.sucursales, rue => rue.e.SucursalId, s => s.Id, (rue, s) => new
                    {
                        Usuario = rue.ru.u.Nombre,
                        EspacioParqueo = rue.e.Ubicacion,
                        IdEspacioParqueo = rue.e.Id,
                        Sucursal = s.Nombre,
                        Fecha = rue.ru.r.Fecha,
                        CantidadHoras = rue.ru.r.CantidadHoras
                    })
                    .ToList();

                // Verificar si hay reservas
                if (!reservas.Any())
                {
                    return NotFound(new { message = "No hay reservas para la fecha especificada." });
                }

                // Devolver las reservas agrupadas por sucursal
                var reservasPorSucursal = reservas
                    .GroupBy(r => r.Sucursal)
                    .Select(g => new
                    {
                        Sucursal = g.Key,
                        Reservas = g.ToList()
                    })
                    .ToList();

                return Ok(new { reservasPorSucursal });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        //Mostrar una lista de reservas activas del usuario.
        [HttpGet]
        [Route("reservasActivas/{UsuarioId}")]
        public IActionResult ObtenerReservasActivas(int UsuarioId)
        {
            try
            {
                // Validar si el usuario existe
                var usuario = _parqueoContexto.usuarios.FirstOrDefault(u => u.Id == UsuarioId);
                if (usuario == null)
                {
                    return BadRequest(new { message = "El usuario no existe." });
                }

                // Obtener las reservas activas del usuario (en este caso, aquellas cuya fecha es futura)
                var reservasActivas = _parqueoContexto.reservas
                    .Where(r => r.UsuarioId == UsuarioId && r.Fecha >= DateTime.Now) // Filtrar por reservas futuras
                    .Join(_parqueoContexto.EspaciosParqueo, r => r.EspacioParqueoId, e => e.Id, (r, e) => new { r, e })
                    .Join(_parqueoContexto.sucursales, rue => rue.e.SucursalId, s => s.Id, (rue, s) => new
                    {
                        Usuario = usuario.Nombre,
                        EspacioParqueo = rue.e.Ubicacion,
                        IdEspacioParqueo = rue.e.Id,
                        Sucursal = s.Nombre,
                        Fecha = rue.r.Fecha,
                        CantidadHoras = rue.r.CantidadHoras
                    })
                    .ToList();

                // Verificar si existen reservas activas
                if (!reservasActivas.Any())
                {
                    return NotFound(new { message = "No hay reservas activas para el Usuario " + usuario.Nombre });
                }

                return Ok(new { reservasActivas });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        //Cancelar una reserva antes de su uso
        [HttpPost]
        [Route("cancelarReserva/{ReservaId}")]
        public IActionResult CancelarReserva(int ReservaId)
        {
            try
            {
                var reserva = _parqueoContexto.reservas.FirstOrDefault(r => r.Id == ReservaId);

                if (reserva == null)
                {
                    return BadRequest(new { message = "La reserva no existe." });
                }

                if (reserva.Fecha < DateTime.Now)
                {
                    return BadRequest(new { message = "No se puede cancelar una reserva ya pasada." });
                }

                reserva.Cancelada = 1; // Marcar como cancelada
                _parqueoContexto.SaveChanges();

                return Ok(new { message = "Reserva cancelada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
