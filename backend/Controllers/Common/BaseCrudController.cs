using backend.Models.Common;
using backend.Models.Dto;
using backend.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Common;

public class BaseCrudController<TService, TEntity, TDto> : ControllerBase
    where TService : BaseCrudService<TEntity, TDto>
    where TEntity : BaseCrudEntity
    where TDto : BaseCrudDto
{
    protected readonly TService _service;

    public BaseCrudController(TService service)
    {
        _service = service;
    }


    [HttpGet()]
    public IEnumerable<TDto> GetAll()
    {
            return _service.GetAll();
    }

    [HttpGet("{id}")]

    public TDto GetById([FromRoute] Guid id)
    {
            return _service.GetById(id);
    }

    [HttpPost]
    public IActionResult Save(TDto dto)
    {
            _service.Save(dto);
            return StatusCode(200, "BRAVO");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
            _service.Delete(id);
            return StatusCode(200, "BRAVO");
    }

    [HttpPut("{id}")]
    public IActionResult Update(TDto dto)
    {
        _service.Update(dto);
        return StatusCode(200, "BRAVO");
    }





}