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
    public class Policies_and_regulationsController : ControllerBase
    {
        private readonly GenericService<Policies_and_regulations> _service;
        private readonly IMapper _mapper;
        private readonly IBunyimagesServices _bunny;
        public Policies_and_regulationsController(GenericService<Policies_and_regulations> service, IMapper mapper, IBunyimagesServices bunny)
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
        public async Task<IActionResult> Create([FromForm] Policies_and_regulationsDto dto)
        {
            string? uploadedUrl = null;

            if (dto.PolicyURL != null && dto.PolicyURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.PolicyURL, "programs");

                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                uploadedUrl = $"https://this0is0my0pull0zone.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var entity = _mapper.Map<Policies_and_regulations>(dto);
            entity.PolicyURL = uploadedUrl;

            await _service.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromForm] Policies_and_regulationsDto dto)
        {
            var existingEntity = await _service.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            if (dto.PolicyURL != null && dto.PolicyURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.PolicyURL, "programs");
                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                existingEntity.PolicyURL = $"https://this0is0my0pull0zone.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var currentUrl = existingEntity.PolicyURL;
            _mapper.Map(dto, existingEntity);
            existingEntity.PolicyURL = currentUrl;

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