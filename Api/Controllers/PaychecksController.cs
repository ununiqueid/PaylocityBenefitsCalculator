using Api.Dtos.Paycheck;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaychecksController : ControllerBase
{
    //Use dependency injection for service layer
    private IEmployeeService employeeService;

    public PaychecksController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    /// <summary>
    /// Added endpoint to allow consumer to calculate an employee paycheck information
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>If successful, the data object will contain the paycheck data</returns>
    [SwaggerOperation(Summary = "Get paycheck by id")]
    [HttpGet("{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> Get(int employeeId)
    {
        var result = new ApiResponse<GetPaycheckDto>();

        //Get the employee by ID
        var employee = await employeeService.GetEmployeeById(employeeId);
        if (employee == null)
        {
            result.Success = false;
            result.Error = $"Employee was not found in the repository";

            return NotFound(result);
        }

        //Calculate the employee paycheck data
        var paycheck = await employeeService.GetPaycheckForEmployee(employee);
        if (paycheck == null)
        {
            result.Success = false;
            result.Error = $"Paycheck could not be calculated for employee id: {employeeId}";

            return result;
        }

        result.Success = true;
        result.Data = paycheck;

        return result;
    }
}
