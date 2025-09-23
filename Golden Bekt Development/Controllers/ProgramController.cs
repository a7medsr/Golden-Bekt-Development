using AutoMapper;
using FX.Services.Bunny;
using Golden_Bekt_Development;
using Golden_Bekt_Development.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProgramController : ControllerBase
{
    private readonly GenericService<programs> _service;
    private readonly IMapper _mapper;
    private readonly IBunyimagesServices _bunny;
    public ProgramController(GenericService<programs> service, IMapper mapper,IBunyimagesServices bunny)
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
    public async Task<IActionResult> Create([FromForm] ProgramCreateDto dto)
    {
        string? uploadedUrl = null;
        if (dto.ProgramUrl != null && dto.ProgramUrl.Length > 0)
        {
            var uploadResult = await _bunny.UploadFileAsync(dto.ProgramUrl, "programs");

            if (!uploadResult.Success)
            {
                return BadRequest(new { error = uploadResult.ErrorMessage });
            }

            uploadedUrl = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
        }
        var entity = _mapper.Map<programs>(dto);
        entity.ProgramUrl = uploadedUrl;
        await _service.AddAsync(entity);
        return Ok(entity);
    }




    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> Update(string id, [FromForm] ProgramCreateDto dto)
    {
        var existingEntity = await _service.GetByIdAsync(id);
        if (existingEntity == null) return NotFound();

        if (dto.ProgramUrl != null && dto.ProgramUrl.Length > 0)
        {
            var uploadResult = await _bunny.UploadFileAsync(dto.ProgramUrl, "programs");
            if (!uploadResult.Success)
            {
                return BadRequest(new { error = uploadResult.ErrorMessage });
            }

            // Build public URL with your pull zone
            existingEntity.ProgramUrl = $"https://konouz.b-cdn.net/programs/{uploadResult.FileName}";
            

        }
        var currentUrl = existingEntity.ProgramUrl;
        _mapper.Map(dto, existingEntity);
        existingEntity.ProgramUrl = currentUrl;
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
