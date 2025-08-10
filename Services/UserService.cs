using AutoMapper;
using VirocGanpati.DTOs;
using VirocGanpati.Helpers;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null || !VerifyPassword(user.Password, password))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            if (user.Status == ActiveInactiveStatus.Inactive)
            {
                throw new UnauthorizedAccessException("User account inactive");
            }

            await _userRepository.MarkUserLoggin(user);

            return user;
        }

        // Updated to handle pagination and search functionality
        public async Task<(int totalCount, IEnumerable<UserDto>)> GetAllUsersAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, string role, bool enablePagination, int? projectId)
        {
            var (totalCount, users) = await _userRepository.GetAllUsersAsync(page, pageSize, searchValue, sortField, sortOrder, role, enablePagination, projectId);
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return (totalCount, userDtos);
        }

        public async Task<MandalDto> GetUserProjectAsync(int userId)
        {
            var project = await _userRepository.GetUserProjectAsync(userId);
            var projectDto = _mapper.Map<MandalDto>(project);
            return projectDto;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AddUserAsync(AddUserDto userDto)
        {
            // Check if a user with the same email or name already exists
            bool userExists = await _userRepository.UserExistsAsync(userDto.Email);
            if (userExists)
            {
                throw new Exception("A user with the same email or name already exists.");
            }

            _ = Enum.TryParse(userDto.Status, out ActiveInactiveStatus statusEnum);

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Mobile = userDto.Mobile,
                Password = HashPassword(userDto.Password),  // Ensure to hash password
                Status = statusEnum,
                RoleId = userDto.RoleId,
                MandalId = userDto.MandalId,
                CreatedAt = DateTime.UtcNow
            };

            var addedUser = await _userRepository.AddUserAsync(user);
            return _mapper.Map<UserDto>(addedUser);  // Return the UserDto instead of the User entity
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            _ = Enum.TryParse(userDto.Status, out ActiveInactiveStatus statusEnum);

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Status = statusEnum;
            user.RoleId = userDto.RoleId;
            user.MandalId = userDto.ProjectId;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = HashPassword(userDto.Password);
            }

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return _mapper.Map<UserDto>(updatedUser);  // Return the updated UserDto
        }

        public async Task<UserDto> UpdateManagerAsync(int id, UpdateManagerDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null || user.Role.RoleIdentifier.ToLower() != "manager")
            {
                return null;
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = HashPassword(userDto.Password);
            }

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return _mapper.Map<UserDto>(updatedUser);  // Return the updated UserDto
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        private bool VerifyPassword(string hashedPassword, string enteredPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<bool> CheckIfEmailExists(string email)
        {
            return await _userRepository.CheckIfEmailExists(email);
        }

        public async Task<bool> CheckIfMobileExists(string mobile)
        {
            return await _userRepository.CheckIfMobileExists(mobile);
        }
    }
}
