using Microsoft.EntityFrameworkCore;
using ManejoPresupuesto.Models;
using Examen.Data;
using System.Collections.Generic;

namespace ManejoPresupuesto.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Venta> Ventas { get; set; }
    }
}
