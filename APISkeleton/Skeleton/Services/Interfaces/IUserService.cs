using Skeleton.DTO;

namespace Skeleton.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAll();

        Task<UserDto> Create(UserDto userDto);
    }
}
