using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_DonisSanchez_MoralesQuezada.Modelos;

namespace P01_DonisSanchez_MoralesQuezada.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalesController : ControllerBase
    {
        private readonly ParqueoDBcontext _parqueoDbC;

        public SucursalesController(ParqueoDBcontext parqueocontext)
        {
            _parqueoDbC = parqueocontext;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<Sucursales> listadosucur = (from s in _parqueoDbC.sucursales select s).ToList();

            if (listadosucur.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadosucur);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Guardar([FromBody] Sucursales sucursal)
        {
            try
            {
                _parqueoDbC.sucursales.Add(sucursal);
                _parqueoDbC.SaveChanges();
                return Ok(sucursal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult Actualizar(int id, [FromBody] Sucursales sucursalesmodificar)
        {
            Sucursales? sucursalesactual = (from s in _parqueoDbC.sucursales
                                                  where s.Id == id
                                                  select s).FirstOrDefault();

            if (sucursalesactual == null)
            {
                return NotFound();
            }

            sucursalesactual.Nombre = sucursalesmodificar.Nombre;
            sucursalesactual.Direccion = sucursalesmodificar.Direccion;
            sucursalesactual.Telefono = sucursalesmodificar.Telefono;

            _parqueoDbC.Entry(sucursalesactual).State = EntityState.Modified;
            _parqueoDbC.SaveChanges();

            return Ok(sucursalesmodificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            Sucursales? sucursal = (from s in _parqueoDbC.sucursales
                                      where s.Id == id
                                      select s).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound();
            }

            _parqueoDbC.sucursales.Add(sucursal);
            _parqueoDbC.SaveChanges();

            return Ok(sucursal);
        }


    }
}
