using Amazon.DynamoDBv2.DataModel;
using KoskuUserService.Contracts;
using System;
using System.Threading.Tasks;

namespace KoskuUserService.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(Guid id);
        Task Save(User user);
        Task Delete(Guid id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDynamoDBContext _context;

        public UserRepository(IDynamoDBContext context) => _context = context;

        public async Task<User> Get(Guid id) => await _context.LoadAsync<User>(id);

        public async Task Save(User user) => await _context.SaveAsync(user);

        public async Task Delete(Guid id) => await _context.DeleteAsync<User>(id);
    }
}
