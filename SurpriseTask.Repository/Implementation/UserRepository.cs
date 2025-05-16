using Microsoft.EntityFrameworkCore;
using SurpriseTask.Repository.Interfaces;
using SurpriseTask.Repository.Models;

namespace SurpriseTask.Repository.Implementation;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;       
    }
    public async Task<List<Users>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Users?> GetUserByEmail(string Email)
    {
        return await _context.Users.Where(x => x.UserEmail == Email).FirstOrDefaultAsync();
    }
}
