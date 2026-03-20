using API.Domain.Entities.UserManagement;
using API.Infrastructure.Persistence;
using API.Infrastructure.Repositories.Interfaces;

namespace API.Infrastructure.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
    
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
    
        public User Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Delete(Guid userId)
        {
            throw new NotImplementedException();
        }

        public User? Get(Guid id)
        {
            return _context.Users.Find(id);
        }

        public User Update(Guid userId, User user)
        {
            throw new NotImplementedException();
        }
    }
}
