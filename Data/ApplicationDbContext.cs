using mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace mvc.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients {get; set; } = default ;

    public DbSet<Staff> Staff {get; set; } = default ;

    public DbSet<Appointment> Appointment {get; set; } = default ;
}
