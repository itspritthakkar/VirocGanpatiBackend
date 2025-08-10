using AutoMapper;
using VirocGanpati.DTOs.ArtiMorningTimes;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class ArtiMorningTimeService : IArtiMorningTimeService
    {
        private readonly IArtiMorningTimeRepository _repository;
        private readonly IMapper _mapper;

        public ArtiMorningTimeService(IArtiMorningTimeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtiMorningTimeDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArtiMorningTimeDto>>(list);
        }

        public async Task<ArtiMorningTimeDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<ArtiMorningTimeDto>(entity);
        }

        public async Task<ArtiMorningTimeDto> CreateAsync(CreateArtiMorningTimeDto dto)
        {
            var entity = _mapper.Map<ArtiMorningTime>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<ArtiMorningTimeDto>(created);
        }

        public async Task<ArtiMorningTimeDto> UpdateAsync(UpdateArtiMorningTimeDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.ArtiMorningTimeId);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<ArtiMorningTimeDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
