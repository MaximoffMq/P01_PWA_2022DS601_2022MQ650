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


}
