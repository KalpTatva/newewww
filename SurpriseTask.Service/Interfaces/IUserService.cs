using SurpriseTask.Repository.Models;
using SurpriseTask.Repository.ViewModels;

namespace SurpriseTask.Service.Interfaces;

public interface IUserService
{
    Task<LoginViewModel> GetUsers();
    Task<ResponseTokenViewModel> UserLogin(LoginViewModel model);
}
