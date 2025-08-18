using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FX.Services.Bunny;
using Golden_Bekt_Development.DTOs;
using Golden_Bekt_Development.Models;
using Microsoft.AspNetCore.Authorization;

namespace Golden_Bekt_Development.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Strategic_and_operational_objectivesControllerr : ControllerBase
    {
        private readonly GenericService<Strategic_and_operational_objectives> _service;
        private readonly IMapper _mapper;
        private readonly IBunyimagesServices _bunny;
        public Strategic_and_operational_objectivesControllerr(GenericService<Strategic_and_operational_objectives> service, IMapper mapper, IBunyimagesServices bunny)
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
        public async Task<IActionResult> Create([FromForm] Strategic_and_operational_objectivesDto dto)
        {
            string? uploadedUrl = null;

            if (dto.ObjectiveURL != null && dto.ObjectiveURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.ObjectiveURL, "programs");

                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                uploadedUrl = $"https://this0is0my0pull0zone.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var entity = _mapper.Map<Strategic_and_operational_objectives>(dto);
            entity.ObjectiveURL = uploadedUrl;

            await _service.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromForm] Strategic_and_operational_objectivesDto dto)
        {
            var existingEntity = await _service.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            if (dto.ObjectiveURL != null && dto.ObjectiveURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.ObjectiveURL, "programs");
                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                existingEntity.ObjectiveURL = $"https://this0is0my0pull0zone.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var currentUrl = existingEntity.ObjectiveURL;
            _mapper.Map(dto, existingEntity);
            existingEntity.ObjectiveURL = currentUrl;

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