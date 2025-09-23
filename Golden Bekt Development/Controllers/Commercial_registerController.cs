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
    public class Commercial_registerController : ControllerBase
    {
        private readonly GenericService<Commercial_register> _service;
        private readonly IMapper _mapper;
        private readonly IBunyimagesServices _bunny;
        public Commercial_registerController(GenericService<Commercial_register> service, IMapper mapper, IBunyimagesServices bunny)
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
        public async Task<IActionResult> Create([FromForm] Commercial_registerDto dto)
        {
            string? uploadedUrl = null;

            if (dto.CommercialRegisterURL != null && dto.CommercialRegisterURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.CommercialRegisterURL, "programs");

                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                uploadedUrl = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var entity = _mapper.Map<Commercial_register>(dto);
            entity.CommercialRegisterURL = uploadedUrl;

            await _service.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromForm] Commercial_registerDto dto)
        {
            var existingEntity = await _service.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            if (dto.CommercialRegisterURL!= null && dto.CommercialRegisterURL.Length > 0)
            {
                var uploadResult = await _bunny.UploadFileAsync(dto.CommercialRegisterURL, "programs");
                if (!uploadResult.Success)
                {
                    return BadRequest(new { error = uploadResult.ErrorMessage });
                }

                existingEntity.CommercialRegisterURL = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            }

            var currentUrl = existingEntity.CommercialRegisterURL;
            _mapper.Map(dto, existingEntity);
            existingEntity.CommercialRegisterURL = currentUrl;

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