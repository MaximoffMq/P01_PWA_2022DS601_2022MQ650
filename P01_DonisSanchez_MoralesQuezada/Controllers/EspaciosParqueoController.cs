using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_DonisSanchez_MoralesQuezada.Modelos;
using System;

[Route("api/[controller]")]
[ApiController]
public class EspaciosParqueoController : ControllerBase
{
    private readonly ParqueoDBcontext _parqueoDbC;

    public EspaciosParqueoController(ParqueoDBcontext context)
    {
        _parqueoDbC = context;
    }

    [HttpPost]
    [Route("Add")]
    public IActionResult Guardar([FromBody] EspaciosParqueo espacio)
    {
        try
        {
            _parqueoDbC.EspaciosParqueo.Add(espacio);
            _parqueoDbC.SaveChanges();
            return Ok(espacio);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("actualizar/{id}")]
    public IActionResult Actualizar(int id, [FromBody] EspaciosParqueo espacioModificar)
    {
        var espacioActual = _parqueoDbC.EspaciosParqueo.FirstOrDefault(e => e.Id == id);

        if (espacioActual == null)
        {
            return NotFound("El espacio de parqueo no existe.");
        }

        espacioActual.Numero = espacioModificar.Numero;
        espacioActual.Ubicacion = espacioModificar.Ubicacion;
        espacioActual.CostoPorHora = espacioModificar.CostoPorHora;
        espacioActual.SucursalId = espacioModificar.SucursalId;

        _parqueoDbC.Entry(espacioActual).State = EntityState.Modified;
        _parqueoDbC.SaveChanges();

        return Ok(espacioModificar);
    }


    [HttpGet]
    [Route("espaciosDisponibles/{sucursalId}/{fecha}")]
    public IActionResult ObtenerEspaciosDisponibles(int sucursalId, DateTime fecha)
    {
        var fechaFormateada = fecha.Date; // Asegúrate de que la fecha esté en formato de solo fecha (sin hora)

        var espaciosDisponibles = _parqueoDbC.EspaciosParqueo
            .Where(ep => ep.SucursalId == sucursalId)
            .GroupJoin(
                _parqueoDbC.reservas.Where(r => r.Fecha.Date == fechaFormateada),
                ep => ep.Id,
                r => r.EspacioParqueoId,
                (ep, reservas) => new
                {
                    Id_espacio = ep.Id,
                    ep.Numero,
                    ep.Ubicacion,
                    ep.CostoPorHora,
                    Estado = reservas.Any() ? "Ocupado" : "Disponible"
                })
            .Where(e => e.Estado == "Disponible")
            .ToList();

        if (espaciosDisponibles.Count == 0)
        {
            return NotFound("No hay espacios disponibles para esta fecha.");
        }

        return Ok(espaciosDisponibles);
    }

    [HttpDelete]
    [Route("eliminarEspacio/{id}")]
    public IActionResult EliminarEspacio(int id)
    {
        var espacio = _parqueoDbC.EspaciosParqueo.FirstOrDefault(e => e.Id == id);

        if (espacio == null)
        {
            return NotFound("El espacio de parqueo no existe.");
        }

        // Si hay reservas asociadas al espacio de parqueo, no se puede eliminar
        var reservas = _parqueoDbC.reservas.Any(r => r.EspacioParqueoId == id);
        if (reservas)
        {
            return BadRequest("El espacio de parqueo no se puede eliminar porque tiene reservas asociadas.");
        }
        else
        {
            _parqueoDbC.EspaciosParqueo.Remove(espacio);
            _parqueoDbC.SaveChanges();
        }
        

        return Ok("Espacio de parqueo eliminado exitosamente.");
    }

    [HttpGet]
    [Route("obtenerTodosEspacios")]
    public IActionResult ObtenerTodosEspacios()
    {
        var espacios = _parqueoDbC.EspaciosParqueo.ToList();

        if (espacios.Count == 0)
        {
            return NotFound("No hay espacios de parqueo disponibles.");
        }

        return Ok(espacios);
    }


}
