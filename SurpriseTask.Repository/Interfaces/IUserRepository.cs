using SurpriseTask.Repository.Models;

namespace SurpriseTask.Repository.Interfaces;

public interface IUserRepository
{
    Task<List<Users>> GetAllUsers();
     Task<Users?> GetUserByEmail(string Email);
}
