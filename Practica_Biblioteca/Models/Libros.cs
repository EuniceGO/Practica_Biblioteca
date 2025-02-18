using System.ComponentModel.DataAnnotations;

namespace Practica_Biblioteca.Models
{
    public class Libros
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int AñoPublicacion { get; set; }

        public int AutorId { get; set; }

        public int Categoria_id { get; set; }

        public string Resumen { get; set; }
    }
}
