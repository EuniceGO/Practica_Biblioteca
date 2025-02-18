using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica_Biblioteca.Models;

namespace Practica_Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {

        private readonly BibliotecaContext _bibliotecaContexto;

        public AutoresController(BibliotecaContext equiposContexto)
        {
            _bibliotecaContexto = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Autor> listadoAutores = (from e in _bibliotecaContexto.autor
                                          select e).ToList();

            if (listadoAutores.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoAutores);
        }



        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {

            Autor? autor = (from e in _bibliotecaContexto.autor
                            where e.Id == id
                            select e).FirstOrDefault();

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEquipo([FromBody] Autor autor)
        {
            try
            {
                _bibliotecaContexto.autor.Add(autor);
                _bibliotecaContexto.SaveChanges();
                return Ok(autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }


        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] Autor autorModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            Autor? equipoActual = (from e in _bibliotecaContexto.autor
                                   where e.Id == id
                                   select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (equipoActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            equipoActual.Nombre = autorModificar.Nombre;
            equipoActual.Nacionalidad = autorModificar.Nacionalidad;



            //Se marca el registro como modificado en el contexto
            //y se envia la modificación a la base de datos
            _bibliotecaContexto.Entry(equipoActual).State = EntityState.Modified;
            _bibliotecaContexto.SaveChanges();

            return Ok(autorModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual eliminaremos
            Autor? equipo = (from e in _bibliotecaContexto.autor
                             where e.Id == id
                             select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (equipo == null)
                return NotFound();

            //Ejecutamos la acción de eliminar el registro
            _bibliotecaContexto.autor.Attach(equipo);
            _bibliotecaContexto.autor.Remove(equipo);
            _bibliotecaContexto.SaveChanges();

            return Ok(equipo);
        }





    }
}
