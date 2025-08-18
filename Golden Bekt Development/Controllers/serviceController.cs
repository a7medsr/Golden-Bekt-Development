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
    public class serviceController : ControllerBase
    {
        private readonly GenericService<service> _service;
        private readonly IMapper _mapper;
        private readonly IBunyimagesServices _bunny;
        public serviceController(GenericService<service> service, IMapper mapper, IBunyimagesServices bunny)
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
        public async Task<IActionResult> Create([FromForm] serviceDto dto)
        {
            string? uploadedUrl = null;

            if (dto.ServiceUrl != null && dto.ServiceUrl.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.ServiceUrl, "programs");

                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                uploadedUrl = $"https://this0is0my0pull0zone.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var entity = _mapper.Map<service>(dto);
            entity.ServiceUrl = uploadedUrl;

            await _service.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromForm] serviceDto dto)
        {
            var existingEntity = await _service.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            if (dto.ServiceUrl != null && dto.ServiceUrl.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.ServiceUrl, "programs");
                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                existingEntity.ServiceUrl = $"https://this0is0my0pull0zone.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var currentUrl = existingEntity.ServiceUrl;
            _mapper.Map(dto, existingEntity);
            existingEntity.ServiceUrl = currentUrl;

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