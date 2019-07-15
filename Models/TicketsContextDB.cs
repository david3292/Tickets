using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tickets.Models
{
    public class TicketsContextDB : DbContext
    {
        public TicketsContextDB(DbContextOptions<TicketsContextDB> options) : base(options) { }

        public DbSet<Client> Client { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity => {
                entity.Property(e => e.nameQueue)
                .IsRequired();

                entity.Property(e => e.idClient)
                .IsRequired();

                entity.Property(e => e.nameCient)
                .IsRequired();

                entity.Property(e => e.attended)
                .IsRequired();
            });
        }
    }
}
