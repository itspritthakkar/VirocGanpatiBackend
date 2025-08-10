using AutoMapper;
using VirocGanpati.DTOs.MandalAreas;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class MandalAreaService : IMandalAreaService
    {
        private readonly IMandalAreaRepository _repository;
        private readonly IMapper _mapper;

        public MandalAreaService(IMandalAreaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MandalAreaDto>> GetAllAsync()
        {
            var areas = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MandalAreaDto>>(areas);
        }

        public async Task<MandalAreaDto> GetByIdAsync(int id)
        {
            var area = await _repository.GetByIdAsync(id);
            return _mapper.Map<MandalAreaDto>(area);
        }

        public async Task<MandalAreaDto> CreateAsync(CreateMandalAreaDto dto)
        {
            var entity = _mapper.Map<MandalArea>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<MandalAreaDto>(created);
        }

        public async Task<MandalAreaDto> UpdateAsync(UpdateMandalAreaDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.AreaId);
            if (existing == null) return null;

            _mapper.Map(dto, existing); // Update the existing entity with new values
            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<MandalAreaDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
