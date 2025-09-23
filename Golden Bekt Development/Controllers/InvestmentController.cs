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
    public class InvestmentController : ControllerBase
    {
        private readonly GenericService<Investment> _service;
        private readonly IMapper _mapper;
        private readonly IBunyimagesServices _bunny;
        public InvestmentController(GenericService<Investment> service, IMapper mapper, IBunyimagesServices bunny)
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
        public async Task<IActionResult> Create([FromForm] InvestmentDto dto)
        {
            string? uploadedUrl = null;

            if (dto.InvestmentUrl != null && dto.InvestmentUrl.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.InvestmentUrl, "programs");

                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                uploadedUrl = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var entity = _mapper.Map<Investment>(dto);
            entity.InvestmentUrl = uploadedUrl;

            await _service.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromForm] InvestmentDto dto)
        {
            var existingEntity = await _service.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            if (dto.InvestmentUrl != null && dto.InvestmentUrl.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.InvestmentUrl, "programs");
                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                existingEntity. InvestmentUrl = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var currentUrl = existingEntity.InvestmentUrl;
            _mapper.Map(dto, existingEntity);
            existingEntity.InvestmentUrl = currentUrl;

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