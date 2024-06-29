using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Truck_Visit_Management.Data;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Entities;
using Truck_Visit_Management.Services.ServiceImpl;
using Truck_Visit_Management.Utils;

public class AuthService : IAuthService
{
    private readonly TruckVisitDbContext _context;
    private readonly IJwtUtils _jwtUtils;

    public AuthService(TruckVisitDbContext context, IJwtUtils jwtUtils)
    {
        _context = context;
        _jwtUtils = jwtUtils;
    }

    public string Authenticate(UserLoginDto login)
    {
        var user = _context.User.SingleOrDefault(u => u.Username == login.Username);
        if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, login.Password))
        {
            return null;
        }
        // authentication successful
        return _jwtUtils.GenerateToken(user);
    }


    public User GetById(int id)
    {
        return _context.User.SingleOrDefault(u => u.Id == id);
    }

    public async Task Register(UserRegisterDto model)
    {
        if (_context.User.Any(u => u.Username == model.Username))
            throw new Exception("Username is already taken");

        var user = new User
        {
            Username = model.Username,
            PasswordHash = PasswordHasher.HashPassword(model.Password),
            Role = model.Role
        };

        _context.User.Add(user);
        await _context.SaveChangesAsync();
    }


}
