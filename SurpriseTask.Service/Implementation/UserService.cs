using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SurpriseTask.Repository.Interfaces;
using SurpriseTask.Repository.Models;
using SurpriseTask.Repository.ViewModels;
using SurpriseTask.Service.Interfaces;
using static SurpriseTask.Repository.Helpers.Enums;

namespace SurpriseTask.Service.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public UserService(
        IUserRepository userRepository,
        IConfiguration configuration
        )
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<LoginViewModel> GetUsers()
    {
        List<Users> users = await _userRepository.GetAllUsers();
        
        LoginViewModel userViewModel = new ();
        userViewModel.Users = users;
        
        return userViewModel;
    }


    public async Task<ResponseTokenViewModel> UserLogin(LoginViewModel model)
    {
        try{
            Users? user = await _userRepository.GetUserByEmail(model.Email.Trim());
            var password = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password);
            Console.WriteLine(model.Password);
            if(user!=null)
            {
                string? RoleName = user.RoleId != 0 ? ((RolesEnum)user.RoleId).ToString():null;

                if(RoleName != null && BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.password))
                {
                    string token = GenerateJwtToken(model.Email, DateTime.Now.AddDays(30), RoleName);
                    if (token != null)
                    {
                        return new ResponseTokenViewModel()
                        {
                            token = token,
                            Role = RoleName,
                            response = "Login successful",
                        };
                    }
                }
            }

            return new ResponseTokenViewModel()
            {
                token = "",
                response = "Invalid User Credentials",
            };

        }catch(Exception e)
        {
            // Console.WriteLine(e.Message);
            return new ResponseTokenViewModel()
            {
                token = "",
                response = "Error 500 : Internal Server Error",
            };
        }
    }



    private string GenerateJwtToken(string email, DateTime expiryTime, string roleName)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
        );
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Get user role from database
        // Console.WriteLine(roleName);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, roleName), 
        };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiryTime,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
