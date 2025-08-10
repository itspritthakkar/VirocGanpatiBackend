using AutoMapper;
using VirocGanpati.DTOs.DateOfVisarjans;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class DateOfVisarjanService : IDateOfVisarjanService
    {
        private readonly IDateOfVisarjanRepository _repository;
        private readonly IMapper _mapper;

        public DateOfVisarjanService(IDateOfVisarjanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DateOfVisarjanDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DateOfVisarjanDto>>(list);
        }

        public async Task<DateOfVisarjanDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<DateOfVisarjanDto>(entity);
        }

        public async Task<DateOfVisarjanDto> CreateAsync(CreateDateOfVisarjanDto dto)
        {
            var entity = _mapper.Map<DateOfVisarjan>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<DateOfVisarjanDto>(created);
        }

        public async Task<DateOfVisarjanDto> UpdateAsync(UpdateDateOfVisarjanDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.DateOfVisarjanId);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<DateOfVisarjanDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
