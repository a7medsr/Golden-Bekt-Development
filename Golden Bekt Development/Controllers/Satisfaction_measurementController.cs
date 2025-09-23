using AutoMapper;
using FX.Services.Bunny;
using Golden_Bekt_Development.DTOs;
using Golden_Bekt_Development.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Golden_Bekt_Development.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Satisfaction_measurementController : ControllerBase
    {
        private readonly GenericService<Satisfaction_measurement> _service;
        private readonly IMapper _mapper;
        private readonly IBunyimagesServices _bunny;
        public Satisfaction_measurementController(GenericService<Satisfaction_measurement> service, IMapper mapper, IBunyimagesServices bunny)
        {
            _service = service;
            _mapper = mapper;
            _bunny = bunny;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromForm] Satisfaction_measurementDto dto)
        {
            string? uploadedUrl = null;

            if (dto.MeasurementURL != null && dto.MeasurementURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.MeasurementURL, "programs");

                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                uploadedUrl = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var entity = _mapper.Map<Satisfaction_measurement>(dto);
            entity.MeasurementURL = uploadedUrl;

            await _service.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromForm] Satisfaction_measurementDto dto)
        {
            var existingEntity = await _service.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            if (dto.MeasurementURL != null && dto.MeasurementURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.MeasurementURL, "programs");
                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                existingEntity.MeasurementURL = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var currentUrl = existingEntity.MeasurementURL;
            _mapper.Map(dto, existingEntity);
            existingEntity.MeasurementURL = currentUrl;

            await _service.UpdateAsync(existingEntity);

            return Ok(existingEntity);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}