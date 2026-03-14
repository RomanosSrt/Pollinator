using API.Domain.Entities.UserManagement;

namespace API.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? Get(Guid id);
        
        User Add(User user);

        User Update(Guid userId, User user);

        User Delete(Guid userId);

    }
}
