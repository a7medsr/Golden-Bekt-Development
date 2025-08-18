using AutoMapper;
using Golden_Bekt_Development.DTOs;
using Golden_Bekt_Development.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Golden_Bekt_Development.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrancheController : ControllerBase
    {
        private readonly GenericService<Branche> _service;
        private readonly IMapper _mapper;

        public BrancheController(GenericService<Branche> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;

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
        public async Task<IActionResult> Create([FromBody] BrancheDto dto)
        {


            var entity = _mapper.Map<Branche>(dto);
            await _service.AddAsync(entity);
            return Ok(entity);
        }


        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update(string id, [FromBody] BrancheDto dto)
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