using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI_DAY3.Models
{
    public class EmpContext : IdentityDbContext
    {
        public EmpContext(DbContextOptions<EmpContext> options):base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {


            builder.Entity<IdentityRole>().HasData(

                new IdentityRole() { Id = "1", Name = "employee", NormalizedName = "EMPLOYEE", ConcurrencyStamp = "1" },
                new IdentityRole() { Id = "2", Name = "manager", NormalizedName = "MANAGER", ConcurrencyStamp = "2" }
                );


            base.OnModelCreating(builder);


        }

        public DbSet<Employee> Employess { get; set; }


    }
}
