using API.Domain.Entities.UserManagement;
using API.Infrastructure.Persistence;
using API.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
    
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<ApplicationUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
