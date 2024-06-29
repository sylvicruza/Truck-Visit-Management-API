
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Entities;

namespace Truck_Visit_Management.Services.ServiceImpl
{
    public interface IAuthService
    {
        string Authenticate(UserLoginDto login);
        User GetById(int id);

        Task Register(UserRegisterDto model);
    }
}
