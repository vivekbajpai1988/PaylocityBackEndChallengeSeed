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
public class EmployeesController : ControllerBase
{
    IDataStore _store;

    public EmployeesController(IDataStore store)
    {
        _store = store;
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        try
        {
            // Configure Await is set to false, to avoid the callback to
            // be invoked on original context - Better performance and deadlock avoidance.
            Employee employee = await _store.GetEmployeeAsync(id).ConfigureAwait(false);
            return base.Ok(
                DataUtils.GetSuccessApiResponse(
                DataUtils.ConvertToEmployeeDto(employee)));

        }
        catch (EntityNotFoundException enfe)
        {
            return NotFound(DataUtils.GetErrorApiResponse<GetEmployeeDto>(enfe.Message));
        }
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        List<Employee> employeeList = await _store.GetAllEmployeesAsync().ConfigureAwait(false);

        List<GetEmployeeDto> employeeDtos = employeeList.Select(employee => DataUtils.ConvertToEmployeeDto(employee))
            .ToList();

        return DataUtils.GetSuccessApiResponse(employeeDtos);
    }

    [SwaggerOperation(Summary = "Get employee Paycheck by employee id")]
    [HttpGet("{id}/paycheck")]
    public async Task<ActionResult<ApiResponse<double>>> GetPaycheck(int id)
    {
        try
        {
            Employee employee = await _store.GetEmployeeAsync(id).ConfigureAwait(false);

            double perPayCheckIncome = PaycheckUtils.CalculatePaycheck(employee);

            return base.Ok(
                DataUtils.GetSuccessApiResponse(perPayCheckIncome));

        }
        catch (EntityNotFoundException enfe)
        {
            return NotFound(DataUtils.GetErrorApiResponse<double>(enfe.Message));
        }
    }
}
