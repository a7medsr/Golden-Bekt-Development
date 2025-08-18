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
    public class Association_MemberController : ControllerBase
    {
        private readonly GenericService<Association_Member> _service;
        private readonly IMapper _mapper;
        private readonly IBunyimagesServices _bunny;
        public Association_MemberController(GenericService<Association_Member> service, IMapper mapper, IBunyimagesServices bunny)
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
        public async Task<IActionResult> Create([FromBody] Association_MemberDto dto)
        {
           

            var entity = _mapper.Map<Association_Member>(dto);
            await _service.AddAsync(entity);
            return Ok(entity);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromBody] Association_MemberDto dto)
        {
            var existingEntity = await _service.GetByIdAsync(id);
            if (existingEntity == null) return NotFound();

            _mapper.Map(dto, existingEntity);
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