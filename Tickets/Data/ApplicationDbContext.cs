using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Regularidad.Models;
using Tickets.Models;

namespace Tickets.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Regularidad.Models.Afiliado>? Afiliado { get; set; }
        public DbSet<Regularidad.Models.Estado>? Estado { get; set; }
        public DbSet<Regularidad.Models.TicketDetalle>? TicketDetalle { get; set; }
        public DbSet<Regularidad.Models.Ticket>? Ticket { get; set; }
    }
}