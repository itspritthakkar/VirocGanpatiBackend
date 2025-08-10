using AutoMapper;
using VirocGanpati.DTOs;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return roleDtos;
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            return role == null ? null : _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto?> GetRoleByIdentifierAsync(string identifier)
        {
            var role = await _roleRepository.GetRoleByIdentifierAsync(identifier);
            return role == null ? null : _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> AddRoleAsync(AddRoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var createdRole = await _roleRepository.AddRoleAsync(role);
            return _mapper.Map<RoleDto>(createdRole);
        }

        public async Task<RoleDto?> UpdateRoleAsync(int id, AddRoleDto roleDto)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null) return null;

            _mapper.Map(roleDto, role);
            await _roleRepository.UpdateRoleAsync(role);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task DeleteRoleAsync(int id)
        {
            await _roleRepository.DeleteRoleAsync(id);
        }
    }
}
