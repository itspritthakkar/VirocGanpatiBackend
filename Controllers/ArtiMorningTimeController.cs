using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirocGanpati.DTOs.ArtiMorningTimes;
using VirocGanpati.Services;

namespace VirocGanpati.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtiMorningTimeController : ControllerBase
    {
        private readonly IArtiMorningTimeService _service;

        public ArtiMorningTimeController(IArtiMorningTimeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Create([FromBody] CreateArtiMorningTimeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ArtiMorningTimeId }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateArtiMorningTimeDto dto)
        {
            if (id != dto.ArtiMorningTimeId) return BadRequest("ID mismatch");

            var updated = await _service.UpdateAsync(dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
