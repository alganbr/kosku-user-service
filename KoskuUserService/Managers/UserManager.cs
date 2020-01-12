using AutoMapper;
using KoskuUserService.Contracts;
using KoskuUserService.Repositories;
using System;
using System.Threading.Tasks;

namespace KoskuUserService.Managers
{
    public interface IUserManager
    {
        Task<UserResponse> Get(Guid id);
        Task<UserResponse> Create(UserRequest request);
        Task<UserResponse> Update(Guid id, UserRequest request);
        Task Delete(Guid id);
    }

    public class UserManager : IUserManager
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserManager(
            IMapper mapper,
            IUserRepository userRepository
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserResponse> Get(Guid id)
        {
            var user = await _userRepository.Get(id);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> Create(UserRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.Id = Guid.NewGuid();
            user.CreatedDate = DateTime.UtcNow;

            await _userRepository.Save(user);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> Update(Guid id, UserRequest request)
        {
            var response = await Get(id);
            if (response == null)
                return null;

            var user = _mapper.Map<User>(request);
            user.Id = id;
            user.LastUpdatedDate = DateTime.UtcNow;

            await _userRepository.Save(user);

            return _mapper.Map<UserResponse>(user);
        }

        public Task Delete(Guid id)
        {
            return _userRepository.Delete(id);
        }
    }
}
