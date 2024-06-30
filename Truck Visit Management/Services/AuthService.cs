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
using Truck_Visit_Management.Repositories;
using Truck_Visit_Management.Services.ServiceImpl;
using Truck_Visit_Management.Utils;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtUtils _jwtUtils;

    public AuthService(IUserRepository userRepository, IJwtUtils jwtUtils)
    {
        _userRepository = userRepository;
        _jwtUtils = jwtUtils;
    }

    public string Authenticate(UserLoginDto login)
    {
        var user = _userRepository.GetByUsername(login.Username);
        if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, login.Password))
        {
            return null;
        }
        // authentication successful
        return _jwtUtils.GenerateToken(user);
    }

    public User GetById(int id)
    {
        return _userRepository.GetById(id);
    }

    public async Task Register(UserRegisterDto model)
    {
        if (await _userRepository.UsernameExistsAsync(model.Username))
            throw new Exception("Username is already taken");

        var user = new User
        {
            Username = model.Username,
            PasswordHash = PasswordHasher.HashPassword(model.Password),
            Role = model.Role
        };

        await _userRepository.AddUserAsync(user);
    }
}

