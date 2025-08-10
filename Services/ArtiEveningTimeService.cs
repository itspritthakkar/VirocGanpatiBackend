using AutoMapper;
using VirocGanpati.DTOs.ArtiEveningTime;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class ArtiEveningTimeService : IArtiEveningTimeService
    {
        private readonly IArtiEveningTimeRepository _repository;
        private readonly IMapper _mapper;

        public ArtiEveningTimeService(IArtiEveningTimeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtiEveningTimeDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArtiEveningTimeDto>>(list);
        }

        public async Task<ArtiEveningTimeDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<ArtiEveningTimeDto>(entity);
        }

        public async Task<ArtiEveningTimeDto> CreateAsync(CreateArtiEveningTimeDto dto)
        {
            var entity = _mapper.Map<ArtiEveningTime>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ArtiEveningTimeDto>(created);
        }

        public async Task<ArtiEveningTimeDto> UpdateAsync(UpdateArtiEveningTimeDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.ArtiEveningTimeId);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<ArtiEveningTimeDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
