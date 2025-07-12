using BloggingPlatform.Application.Interfaces;
using PersonalBloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
