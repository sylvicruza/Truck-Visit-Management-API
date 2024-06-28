using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Truck_Visit_Management.Entities;

namespace Truck_Visit_Management.Data
{
    public class TruckVisitDbContext : DbContext
    {
        public TruckVisitDbContext (DbContextOptions<TruckVisitDbContext> options)
            : base(options)
        {
        }

        public DbSet<Truck_Visit_Management.Entities.VisitRecordEntity> VisitRecordEntity { get; set; } = default!;

        public DbSet<Truck_Visit_Management.Entities.ActivityEntity>? ActivityEntity { get; set; }

        public DbSet<Truck_Visit_Management.Entities.DriverInformationEntity>? DriverInformationEntity { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VisitRecordEntity>()
                .HasMany(v => v.Activities)
                .WithOne(a => a.VisitRecord)
                .HasForeignKey(a => a.VisitRecordId);

            modelBuilder.Entity<VisitRecordEntity>()
                .OwnsOne(v => v.Driver);
        }
    }
}
