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
    public class Financial_reportController : ControllerBase
    {
        private readonly GenericService<Financial_report> _service;
        private readonly IMapper _mapper;
        private readonly IBunyimagesServices _bunny;
        public Financial_reportController(GenericService<Financial_report> service, IMapper mapper, IBunyimagesServices bunny)
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
        public async Task<IActionResult> Create([FromForm] Financial_reportDto dto)
        {
            string? uploadedUrl = null;

            if (dto.ReportURL != null && dto.ReportURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.ReportURL, "programs");

                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                uploadedUrl = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var entity = _mapper.Map<Financial_report>(dto);
            entity.ReportURL = uploadedUrl;

            await _service.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromForm] Financial_reportDto dto)
        {
            var existingEntity = await _service.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            if (dto.ReportURL != null && dto.ReportURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.ReportURL, "programs");
                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                existingEntity.ReportURL = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var currentUrl = existingEntity.ReportURL;
            _mapper.Map(dto, existingEntity);
            existingEntity.ReportURL = currentUrl;

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