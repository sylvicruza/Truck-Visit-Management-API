namespace Truck_Visit_Management.Utils
{
    using BCrypt.Net;

    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Verify(password, hashedPassword);
        }
    }

}
