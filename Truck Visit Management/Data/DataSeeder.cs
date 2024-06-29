using Microsoft.CodeAnalysis.Scripting;
using System;
using Truck_Visit_Management.Entities;
using Truck_Visit_Management.Utils;

namespace Truck_Visit_Management.Data
{
    public static class DataSeeder
    {
        public static void SeedUsers(TruckVisitDbContext context)
        {
            if (!context.User.Any())
            {
                // Seed users with roles
                var users = new[]
                {
                    new User { Username = "admin", PasswordHash = PasswordHasher.HashPassword("password"), Role = "Admin" },
                    new User { Username = "user1", PasswordHash = PasswordHasher.HashPassword("password1"), Role = "User" }
                };

                context.User.AddRange(users);
                context.SaveChanges();
            }
        }
    }

}
