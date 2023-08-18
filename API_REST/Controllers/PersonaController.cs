using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_REST.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using API_REST.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_REST.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly _context _context;

        public PersonaController(_context context)
        {
            _context = context;
        }
        //GetPersona
        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPersona(int id)
        {
            var persona = await _context.Personas.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            return Ok(persona);
        }


        //AgregarPersona
        [HttpPost("AgregarPersona")]
        public async Task<IActionResult> AgregarPersona(Persona persona)
        {
            try
            {
                var parametros = new[]
                {
                new SqlParameter("@Nombre", persona.Nombre),
                new SqlParameter("@Paterno", persona.Paterno),
                new SqlParameter("@Materno", persona.Materno),
                new SqlParameter("@RFC", persona.RFC),
                new SqlParameter("@FNacimiento", persona.FNacimiento),
                new SqlParameter("@Usuario", persona.Usuario)
                };

                var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                var mensajeParam = new SqlParameter("@MensajeError", SqlDbType.VarChar, 150)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_NuevaPersona @Nombre, @Paterno, @Materno, @RFC, @FNacimiento, @Usuario, @Resultado OUTPUT, @MensajeError OUTPUT",
                    parametros.Concat(new[] { resultadoParam, mensajeParam }).ToArray());

                int resultado = (int)resultadoParam.Value;
                string mensajeError = mensajeParam.Value?.ToString();

                if (resultado == 1)
                {
                    return Ok(new { Resultado = true, MENSAJE = mensajeError }); // Devuelve un objeto JSON

                }
                else
                {
                    return BadRequest(new { Resultado = false, Descripcion = mensajeError ?? "Error al agregar persona" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Error interno del servidor",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }



        //ActualizarPersona
        [HttpPut("ActualizarPersona")]
        public async Task<IActionResult> ActualizarPersona(Persona persona)
        {
            try
            {
                var parametros = new[]
                {
                    new SqlParameter("@IdPer", persona.IdPer),
                    new SqlParameter("@Nombre", persona.Nombre),
                    new SqlParameter("@Paterno", persona.Paterno),
                    new SqlParameter("@Materno", persona.Materno),
                    new SqlParameter("@RFC", persona.RFC),
                    new SqlParameter("@FNacimiento", persona.FNacimiento),
                    new SqlParameter("@Usuario", persona.Usuario)
                };

                var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                var mensajeParam = new SqlParameter("@MensajeError", SqlDbType.VarChar, 150)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_Actualizar @IdPer, @Nombre, @Paterno, @Materno, @RFC, @FNacimiento, @Usuario, @Resultado OUTPUT, @MensajeError OUTPUT",
                    parametros.Concat(new[] { resultadoParam, mensajeParam }).ToArray());

                int resultado = (int)resultadoParam.Value;
                string mensajeError = mensajeParam.Value?.ToString();

                if (resultado == 1)
                {
                    return Ok(new { Resultado = true, MENSAJE = mensajeError }); // Devuelve un objeto JSON
                }
                else
                {
                    return NotFound(new { Resultado = false, Descripcion = mensajeError ?? "Error al actualizar persona" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Error interno del servidor",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }


        //EliminarPersona
        [HttpDelete("EliminarPersona")]
        public async Task<IActionResult> EliminarPersona(int id, string usuario)
        {
            try
            {
                var parametros = new[]
               {
                    new SqlParameter("@IdPer",id),
                    new SqlParameter("@Usuario", usuario)
                };

                var resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                var mensajeParam = new SqlParameter("@MensajeError", SqlDbType.VarChar, 150)
                {
                    Direction = ParameterDirection.Output
                };
               

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_Eliminar @IdPer, @Usuario, @Resultado OUTPUT, @MensajeError OUTPUT",
                     parametros.Concat(new[] { resultadoParam, mensajeParam }).ToArray()
                );

                int resultado = (int)resultadoParam.Value;
                string mensajeError = mensajeParam.Value?.ToString();

                if (resultado > 0)
                {
                    return Ok(new { Resultado = true, MENSAJE = mensajeError }); // Devuelve un objeto JSON
                }
                else
                {
                    return NotFound(new { Resultado = false, Descripcion = mensajeError ?? "Error al eliminar persona" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Error interno del servidor",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }


    }
}
