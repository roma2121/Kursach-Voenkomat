using Kursach_Voenkomat.Models;
using Microsoft.EntityFrameworkCore;

namespace Kursach_Voenkomat.Data
{
    public class AuditDBcontext : DbContext
    {
        public AuditDBcontext(DbContextOptions<AuditDBcontext> options)
            : base(options)
        {
        }

        public DbSet<LoginViewModel> Login { get; set; }
    }
}