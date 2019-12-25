using Core3.xWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core3.xWebApi.Data
{
    public class WebApiDbContext : DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Company>().HasData(
                new Company()
                {
                    Id = Guid.Parse("99BA5433-DF5F-A898-C8E0-78B8BA55F251"),
                    Introduction = "OK",
                    Name = "Ali"
                },
                new Company()
                {
                    Id = Guid.Parse("F32A94E2-DCA3-767E-7CE0-06CCAE6EF474"),
                    Introduction = "OK",
                    Name = "TX"
                },
                new Company()
                {
                    Id = Guid.Parse("19660152-E925-1BFC-CB2C-B8EBA0FBCA82"),
                    Introduction = "OK",
                    Name = "JD"
                }
            );
        }

    }
}
