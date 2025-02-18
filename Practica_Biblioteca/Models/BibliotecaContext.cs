using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Practica_Biblioteca.Models;

namespace Practica_Biblioteca.Models
{
    public class BibliotecaContext : DbContext

    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
        {
        }

        public DbSet<Autor> autor { get; set; }
        public DbSet<Libros> libro { get; set; }  // <--- Agregar esta línea

    }
}
