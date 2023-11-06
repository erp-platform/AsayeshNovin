using api.Interfaces;
using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class CrudController<TEntity, TCreateDto, TUpdateDto>
    : ControllerBase, ICrudController<TCreateDto, TUpdateDto>
    where TEntity : BaseEntity
    where TCreateDto : class
    where TUpdateDto : class
{
    private readonly ICrudService<TEntity, TCreateDto, TUpdateDto> _authService;

    public CrudController(ICrudService<TEntity, TCreateDto, TUpdateDto> authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _authService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id)
    {
        return Ok(await _authService.GetByIdAsync(id));
    }

    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] TCreateDto createDto)
    {
        return Ok(await _authService.CreateAsync(createDto));
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TUpdateDto updateDto)
    {
        return Ok(await _authService.UpdateAsync(id, updateDto));
    }

    [HttpPost("delete/{id:guid}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _authService.DeleteAsync(id));
    }

    [HttpPost("restore/{id:guid}")]
    public virtual async Task<IActionResult> Restore(Guid id)
    {
        return Ok(await _authService.RestoreAsync(id));
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task<IActionResult> ForceDeleteAsync(Guid id)
    {
        return Ok(await _authService.ForceDeleteAsync(id));
    }
}