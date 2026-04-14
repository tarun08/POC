using Microsoft.AspNetCore.Http.HttpResults;
using Skeleton.DTO;
using Skeleton.Repositories.Interfaces;
using Skeleton.Services.Interfaces;

namespace Skeleton.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        public UserService(IUsersRepository usersRepository) 
        {
            _usersRepository = usersRepository;
        }

        public async Task<List<UserDto>> GetAll()
        {
            var allUsers = await _usersRepository.GetAllAsync();
            return allUsers;
        }

        public async Task<UserDto> Create(UserDto userDto)
        {
            var user = await _usersRepository.CreateAsync(userDto);
            return user;
        }
    }
}
