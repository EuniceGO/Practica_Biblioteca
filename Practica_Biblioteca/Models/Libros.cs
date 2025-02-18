using System.ComponentModel.DataAnnotations;

namespace Practica_Biblioteca.Models
{
    public class Libros
    {
        [Key]
        public int id { get; set; }
        public string Titulo { get; set; }
        public int AñoPublicacion { get; set; }

        public int autor_id { get; set; }

        public int categoria_id { get; set; }

        public string Resumen { get; set; }
    }
}
