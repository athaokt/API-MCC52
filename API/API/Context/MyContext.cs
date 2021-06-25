using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Proxies;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Education>()
                .HasOne(u => u.University)
                .WithMany(e => e.Educations);

            modelBuilder.Entity<Profiling>()
                .HasOne(e => e.Education)
                .WithMany(p => p.Profilings);

            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Account)
                .WithOne(e => e.Employee)
                .HasForeignKey<Account>(e => e.NIK);

            modelBuilder.Entity<Account>()
                .HasOne(p => p.Profiling)
                .WithOne(a => a.Account)
                .HasForeignKey<Profiling>(a => a.NIK);

            modelBuilder.Entity<Role>()
                .HasMany(x => x.Account)
                .WithMany(p => p.Roles)
                .UsingEntity<AccountRole>(a => a.HasOne(w => w.Account)
                .WithMany().HasForeignKey(s => s.NIK), s => s.HasOne(l => l.Role).WithMany().HasForeignKey(s => s.RoleId));

            /*modelBuilder.Entity<AccountRole>().HasKey(pt => new { pt.NIK, pt.RoleId });*/
        }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }*/
    }
}
