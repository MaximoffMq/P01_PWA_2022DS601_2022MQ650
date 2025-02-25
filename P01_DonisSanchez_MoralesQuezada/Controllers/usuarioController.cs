using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_DonisSanchez_MoralesQuezada.Modelos;

namespace P01_DonisSanchez_MoralesQuezada.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuarioController : ControllerBase
    {
        private readonly ParqueoDBcontext _parqueoContexto;

        public usuarioController(ParqueoDBcontext parqueoContexto)
        {
            _parqueoContexto = parqueoContexto;
        }

        [HttpPost]
        [Route("Add/{nombre}/{correo}/{telefono}/{contrasena}/{rol}")]
        public IActionResult CrearUsuario(string nombre, string correo, string telefono, string contrasena, string rol)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(contrasena) || string.IsNullOrEmpty(rol))
                {
                    return BadRequest(new { message = "Todos los campos son obligatorios." });
                }

                if (rol != "Cliente" && rol != "Empleado")
                {
                    return BadRequest(new { message = "El rol debe ser 'Cliente' o 'Empleado'." });
                }

                var existingUser = _parqueoContexto.usuarios.FirstOrDefault(u => u.Correo == correo);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Ya existe un usuario con ese correo." });
                }

                Usuarios usuario = new Usuarios
                {
                    Nombre = nombre,
                    Correo = correo,
                    Telefono = telefono,
                    Contrasena = contrasena,
                    Rol = rol
                };

                _parqueoContexto.usuarios.Add(usuario);
                _parqueoContexto.SaveChanges();

                return Ok(new { message = "Usuario creado exitosamente.", usuario = usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }





        [HttpGet]
        [Route("Login/{correo}/{contrasena}")]
        public IActionResult IniciarSesion(string correo, string contrasena)
        {
            try
            {
                // Buscar el usuario en la base de datos por correo
                var usuarioDb = _parqueoContexto.usuarios
                    .FirstOrDefault(u => u.Correo == correo);

                // Si el usuario no existe
                if (usuarioDb == null)
                {
                    return Unauthorized("Credenciales inválidas.");
                }

                // Verificar si la contraseña ingresada coincide con la almacenada
                if (usuarioDb.Contrasena == contrasena)
                {
                    return Ok(new
                    {
                        message = "Inicio de sesión exitoso",
                        Id_Usuario = usuarioDb.Id,
                        Nombre = usuarioDb.Nombre,
                        Correo = usuarioDb.Correo,
                        Telefono = usuarioDb.Telefono,
                        Rol = usuarioDb.Rol,
                        Contrasena = usuarioDb.Contrasena // No es recomendable enviar la contraseña, aunque esté encriptada
                    });
                }
                else
                {
                    return Unauthorized("Credenciales inválidas.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //Resto del CRUD de usuarios

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Usuarios> listadoAutor = (from e in _parqueoContexto.usuarios select e).ToList();

            if (listadoAutor.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoAutor);
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] Usuarios usuarioModificar)
        {
            try
            {
                // Obtener el registro original de la base de datos
                Usuarios? usuarioActual = _parqueoContexto.usuarios
                    .FirstOrDefault(u => u.Id == id);

                // Verificamos que exista el registro según su ID
                if (usuarioActual == null)
                {
                    return NotFound(new { message = "Usuario no encontrado." });
                }

                // Si se encuentra el registro, se alteran los campos modificables
                usuarioActual.Nombre = usuarioModificar.Nombre ?? usuarioActual.Nombre;
                usuarioActual.Correo = usuarioModificar.Correo ?? usuarioActual.Correo;
                usuarioActual.Telefono = usuarioModificar.Telefono ?? usuarioActual.Telefono;
                usuarioActual.Contrasena = usuarioModificar.Contrasena ?? usuarioActual.Contrasena;
                usuarioActual.Rol = usuarioModificar.Rol ?? usuarioActual.Rol;

                // Se marca el registro como modificado en el contexto y se envía la modificación a la base de datos
                _parqueoContexto.Entry(usuarioActual).State = EntityState.Modified;
                _parqueoContexto.SaveChanges();

                return Ok(new { message = "Usuario actualizado exitosamente.", usuario = usuarioActual });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                // Obtener el registro original de la base de datos
                Usuarios? usuario = _parqueoContexto.usuarios
                    .FirstOrDefault(u => u.Id == id);

                // Verificamos que exista el registro según su ID
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado." });
                }

                // Ejecutamos la acción de eliminar el registro
                _parqueoContexto.usuarios.Remove(usuario);
                _parqueoContexto.SaveChanges();

                // Retorna un mensaje de éxito al eliminar el usuario
                return Ok(new { message = "Usuario eliminado exitosamente.", usuario = usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
