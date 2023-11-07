using UMS.Authentication.Application.Interfaces;
using UMS.Authentication.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.Interfaces;
using UMS.Authentication.Application.Authorization;

namespace Presentation.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class BaseController<TEntity, TCreateDto, TUpdateDto>
    : ControllerBase, IBaseController<TCreateDto, TUpdateDto>
    where TEntity : BaseEntity
    where TCreateDto : class
    where TUpdateDto : class
{
    private readonly IBaseService<TEntity, TCreateDto, TUpdateDto> _service;

    public BaseController(IBaseService<TEntity, TCreateDto, TUpdateDto> service)
    {
        _service = service;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<IActionResult> GetByIdAsync(Guid id)
    {
        return Ok(await _service.FindByIdAsync(id));
    }

    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] TCreateDto createDto)
    {
        return Ok(await _service.CreateAsync(createDto));
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TUpdateDto updateDto)
    {
        return Ok(await _service.UpdateAsync(id, updateDto));
    }

    [HttpPost("delete/{id:guid}")]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _service.DeleteAsync(id));
    }

    [HttpPost("restore/{id:guid}")]
    public virtual async Task<IActionResult> Restore(Guid id)
    {
        return Ok(await _service.RestoreAsync(id));
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task<IActionResult> ForceDeleteAsync(Guid id)
    {
        return Ok(await _service.ForceDeleteAsync(id));
    }
}