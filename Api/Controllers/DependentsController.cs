using Api.business;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Exceptions;
using Api.Models;
using Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    IDataStore _store;

    public DependentsController(IDataStore store)
    {
        _store = store;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        try
        {
            Dependent dependent = await _store.GetDependentAsync(id).ConfigureAwait(false);
            return base.Ok(
                DataUtils.GetSuccessApiResponse(
                DataUtils.ConvertToDependentDto(dependent)));

        }
        catch (EntityNotFoundException enfe)
        {
            return NotFound(DataUtils.GetErrorApiResponse<GetDependentDto>(enfe.Message));
        }
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        List<Dependent> dependentsList = await _store.GetAllDependentsAsync().ConfigureAwait(false);

        List<GetDependentDto> dependentDtos = dependentsList.Select(dependent => DataUtils.ConvertToDependentDto(dependent))
            .ToList();

        return DataUtils.GetSuccessApiResponse(dependentDtos);
    }
}
