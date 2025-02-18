using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica_Biblioteca.Models;

namespace Practica_Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaContext _bibliotecaContexto;

        public LibrosController(BibliotecaContext equiposContexto)
        {
            _bibliotecaContexto = equiposContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los equipos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll_libro")]
        public IActionResult Get()
        {
            List<Libros> listadoAutores = (from e in _bibliotecaContexto.libro
                                           select e).ToList();

            if (listadoAutores.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoAutores);
        }



        [HttpGet]
        [Route("GetById_libro/{id}")]
        public IActionResult Get(int id)
        {

            Libros? autor = (from e in _bibliotecaContexto.libro
                             where e.Id == id
                             select e).FirstOrDefault();

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }


        [HttpPost]
        [Route("Add_libro")]
        public IActionResult GuardarEquipo([FromBody] Libros libro)
        {
            try
            {
                _bibliotecaContexto.libro.Add(libro);
                _bibliotecaContexto.SaveChanges();
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }


        [HttpPut]
        [Route("actualizar_libro/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] Libros autorModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            Libros? equipoActual = (from e in _bibliotecaContexto.libro
                                    where e.Id == id
                                    select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (equipoActual == null)
            {
                return NotFound();
            }

            //Si se encuentra el registro, se alteran los campos modificables
            equipoActual.Titulo = autorModificar.Titulo;
            equipoActual.AñoPublicacion = autorModificar.AñoPublicacion;
            equipoActual.Titulo = autorModificar.Titulo;
            equipoActual.AñoPublicacion = autorModificar.AñoPublicacion;
            equipoActual.AutorId = autorModificar.AutorId;
            equipoActual.Categoria_id = autorModificar.Categoria_id;
            equipoActual.Resumen = autorModificar.Resumen;


            //Se marca el registro como modificado en el contexto
            //y se envia la modificación a la base de datos
            _bibliotecaContexto.Entry(equipoActual).State = EntityState.Modified;
            _bibliotecaContexto.SaveChanges();

            return Ok(autorModificar);
        }

        [HttpDelete]
        [Route("eliminar_Libro/{id}")]
        public IActionResult EliminarLibro(int id)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual eliminaremos
            Libros? equipo = (from e in _bibliotecaContexto.libro
                              where e.Id == id
                              select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (equipo == null)
                return NotFound();

            //Ejecutamos la acción de eliminar el registro
            _bibliotecaContexto.libro.Attach(equipo);
            _bibliotecaContexto.libro.Remove(equipo);

            _bibliotecaContexto.SaveChanges();

            return Ok(equipo);
        }

        /// <summary>
        /// Consultas LINQ
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("LibroMayores")]
        public IActionResult LibrosMayores()
        {
            var listadoLibros = (from l in _bibliotecaContexto.libro
                                 where l.AñoPublicacion > 2000
                                 select l).ToList();

            if (listadoLibros == null)
            {
                return NotFound();
            }
            return Ok(listadoLibros);

        }


        [HttpGet]
        [Route("ContarLibroById")]
        public IActionResult Contar(int id)
        {
            var listadoLibros = (from l in _bibliotecaContexto.libro
                                 where l.AutorId == id
                                 select l).Count();

            if (listadoLibros == 0)
            {
                return NotFound();
            }
            return Ok(listadoLibros);

        }

        [HttpGet]
        [Route("GetTituloLibro/{titulo}")]

        public IActionResult GetLibroTitulo(string titulo)
        {
            var AutorById = (from l in _bibliotecaContexto.libro
                             join a in _bibliotecaContexto.autor
                             on l.AutorId equals a.Id
                             where l.Titulo == titulo
                             select new
                             {
                                 l.Id,
                                 l.Titulo,
                                 l.AñoPublicacion,
                                 l.AutorId,
                                 l.Categoria_id,
                                 l.Resumen,
                                 a.Nombre

                             }).ToList();

            if (AutorById.Count() == 0)
            {
                return NotFound();
            }
            return Ok(AutorById);
        }

        /// Segundas consultas LINQ
        /// 
        [HttpGet]
        [Route("Recientes")]
        public IActionResult LibrosMasRecientes()
        {
            var librosMasRecientes = (from l in _bibliotecaContexto.libro
                                      orderby l.AñoPublicacion descending
                                      select l)
                                      .ToList();

            return Ok(librosMasRecientes);
        }

        [HttpGet]
        [Route("CantidadLibros")]
        public IActionResult CantidadLibrosPorAño()
        {
            var cantidadLibrosPorAño = (from l in _bibliotecaContexto.libro
                                        group l by l.AñoPublicacion into g
                                        select new
                                        {
                                            Año = g.Key,
                                            CantidadLibros = g.Count()
                                        }).ToList();

            return Ok(cantidadLibrosPorAño);
        }

        [HttpGet]
        [Route("VerAutorLibros/{id}")]
        public IActionResult VerificarAutorLibros(int id)
        {
            var autorConLibros = (from l in _bibliotecaContexto.libro
                                  where l.AutorId == id
                                  select new
                                  {
                                      l.AutorId
                                  }).FirstOrDefault();

            if (autorConLibros == null)
            {
                return NotFound();
            }

            return Ok(autorConLibros);
        }

    

        [HttpGet]
        [Route("PrimerLibroByAutor/{id}")]
        public IActionResult PrimerLibro(int id)
        {
            var listaLibro = (from l in _bibliotecaContexto.libro
                              where l.AutorId == id
                              orderby l.AñoPublicacion ascending
                              select new
                              {
                                  l.Id,
                                  l.Titulo,
                                  l.AñoPublicacion,
                                  l.AutorId,
                                  l.Categoria_id,
                                  l.Resumen
                              }).FirstOrDefault();

            if (listaLibro == null)
            {
                return NotFound();
            }

            return Ok(listaLibro);
        }
    }
}
