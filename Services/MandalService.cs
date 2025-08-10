using AutoMapper;
using System.Text.RegularExpressions;
using VirocGanpati.DTOs;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class MandalService : IMandalService
    {
        private readonly IMandalRepository _mandalRepository;
        private readonly IMapper _mapper;

        public MandalService(IMandalRepository mandalRepository, IMapper mapper)
        {
            _mandalRepository = mandalRepository;
            _mapper = mapper;
        }

        public async Task<(int totalElements, IEnumerable<MandalDto> data)> GetMandalsAsync(int page, int pageSize, string searchValue, string sortField, string sortOrder, string status)
        {
            var (totalCount, mandals) = await _mandalRepository.GetMandalsAsync(page, pageSize, searchValue, sortField, sortOrder, status);
            var mandalDtos = _mapper.Map<IEnumerable<MandalDto>>(mandals);
            return (totalCount, mandalDtos);
        }

        public async Task<(int totalElements, IEnumerable<MandalDto> data)> GetAllMandalsAsync()
        {
            var (totalCount, mandals) = await _mandalRepository.GetAllMandalsAsync();
            var mandalDtos = _mapper.Map<IEnumerable<MandalDto>>(mandals);
            return (totalCount, mandalDtos);
        }

        public async Task<MandalDto> GetMandalByIdAsync(int id)
        {
            var mandal = await _mandalRepository.GetMandalByIdAsync(id);
            if (mandal == null) return null;
            return _mapper.Map<MandalDto>(mandal);  // Map Mandal to MandalDto
        }

        public async Task<MandalDto> AddMandalAsync(AddMandalDto mandalDto)
        {
            var mandal = _mapper.Map<Mandal>(mandalDto);
            mandal.MandalSlug = await GenerateUniqueMandalSlugAsync(mandalDto.MandalName);
            var createdMandal = await _mandalRepository.AddMandalAsync(mandal);
            return _mapper.Map<MandalDto>(createdMandal);
        }

        private async Task<string> GenerateUniqueMandalSlugAsync(string mandalName)
        {
            string baseSlug = GenerateSlug(mandalName);

            // Fetch all slugs that start with the base slug (e.g., shree_ganesh_mandal, shree_ganesh_mandal_1, shree_ganesh_mandal_2)
            var existingSlugs = await _mandalRepository.GetMandalSlugStartingWithAsync(baseSlug);

            if (!existingSlugs.Contains(baseSlug))
            {
                return baseSlug; // No conflict, just use the base
            }

            // Find highest number suffix already used
            int maxNumber = 0;
            foreach (var slug in existingSlugs)
            {
                var match = Regex.Match(slug, $"^{Regex.Escape(baseSlug)}_(\\d+)$");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int num))
                {
                    if (num > maxNumber) maxNumber = num;
                }
            }

            return $"{baseSlug}_{maxNumber + 1}";
        }

        private static string GenerateSlug(string mandalName)
        {
            // Convert to lowercase
            string slug = mandalName.ToLower();

            // Replace spaces with underscores
            slug = slug.Replace(" ", "_");

            // Remove characters that are not letters, digits, or underscores
            slug = Regex.Replace(slug, @"[^a-z0-9_]", "");

            // Remove consecutive underscores
            slug = Regex.Replace(slug, @"_+", "_");

            // Trim underscores from start and end
            slug = slug.Trim('_');

            return slug;
        }

        public async Task<MandalDto> UpdateMandalAsync(int id, UpdateMandalDto mandalDto)
        {
            var mandal = await _mandalRepository.GetMandalByIdAsync(id);
            if (mandal == null) return null;

            _mapper.Map(mandalDto, mandal);  // Update Mandal with AddMandalDto properties
            await _mandalRepository.UpdateMandalAsync(mandal);
            return _mapper.Map<MandalDto>(mandal);  // Return the updated MandalDto
        }

        public async Task DeleteMandalAsync(int id)
        {
            await _mandalRepository.DeleteMandalAsync(id);
        }
    }
}

