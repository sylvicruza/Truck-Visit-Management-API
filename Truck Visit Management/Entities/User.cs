namespace Truck_Visit_Management.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // In a real application, store hashed passwords
        public string Role { get; set; } // e.g., "Admin", "User"
    }

}
