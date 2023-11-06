using Microsoft.AspNetCore.Mvc;

namespace Presentation.Interfaces;

public interface IBaseController<in TCreateDto, in TUpdateDto>
    where TCreateDto : class
    where TUpdateDto : class
{
    Task<IActionResult> GetAllAsync();
    Task<IActionResult> GetByIdAsync(Guid id);
    Task<IActionResult> CreateAsync(TCreateDto createDto);
    Task<IActionResult> UpdateAsync(Guid id, TUpdateDto updateDto);
    Task<IActionResult> Delete(Guid id);
    Task<IActionResult> Restore(Guid id);
    Task<IActionResult> ForceDeleteAsync(Guid id);
}