using Aeternum.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aeternum.Profiles
{
    // DbContext pro ASP.NET Core Identity s vlastními modely pro uživatele a role
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Zde můžeš přidat další konfiguraci modelu, pokud budeš potřebovat
        }

        // Pokud máš další DbSety pro jiné modely, můžeš je přidat zde
        // Např. DbSet<Article> nebo jiné entity
    }
}
