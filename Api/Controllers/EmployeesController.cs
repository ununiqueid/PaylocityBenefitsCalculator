using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    //Use dependency injection for service layer
    private IEmployeeService employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    /// <summary>
    /// This endpoint will return all of the employees in the database
    /// </summary>
    /// <returns>If successful, the data object will contain all of the employee records</returns>
    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var result = new ApiResponse<List<GetEmployeeDto>>();
        var employees = await employeeService.GetAllEmployees();
        if (employees == null)
        {
            result.Success = false;
            result.Error = "No employees were found in the repository";

            return NotFound(result);
        }

        result.Success = true;
        result.Data = employees;

        return Ok(result);
    }

    /// <summary>
    /// This endpoint will return a specific employee record from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If successful, the data object will contain the employee's record</returns>
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var result = new ApiResponse<GetEmployeeDto>();
        var employee = await employeeService.GetEmployeeById(id);
        if (employee == null)
        {
            result.Success = false;
            result.Error = $"Specified employee id {id} was not found";

            return NotFound(result);
        }

        result.Success = true;
        result.Data = employee;

        return result;
    }

    /// <summary>
    /// Added an endpoint to allow consumer to add new employees to the database
    /// </summary>
    /// <param name="employee"></param>
    /// <returns>If successful, the data object will contain the new employee record</returns>
    [SwaggerOperation(Summary = "Add new employee")]
    [HttpPost("")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> AddNewEmployee([FromBody] GetEmployeeDto employee)
    {
        var result = new ApiResponse<GetEmployeeDto>();
        var newEmployee = await employeeService.AddNewEmployee(employee);
        if (newEmployee == null)
        {
            result.Success = false;
            result.Error = $"Error adding new employee";

            return result;
        }

        result.Success = true;
        result.Data = newEmployee;

        return Ok(result);
    }

    /// <summary>
    /// Added endpoint to allow consumer to edit an existing employee record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="employee"></param>
    /// <returns>If successful, the data object will contain the updated employee record</returns>
    [SwaggerOperation(Summary = "Update employee")]
    [HttpPut("{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> UpdateEmployee(int employeeId, [FromBody] GetEmployeeDto employee)
    {
        var result = new ApiResponse<GetEmployeeDto>();
        var employeeRecord = await employeeService.GetEmployeeById(employeeId);
        if (employeeRecord == null)
        {
            result.Success = false;
            result.Error = $"Employee was not found in the repository";

            return NotFound(result);
        }

        var updatedEmployee = await employeeService.UpdateEmployee(employeeId, employee);
        if (updatedEmployee == null)
        {
            result.Success = false;
            result.Error = $"There was an error updating the employee";

            return NotFound(result);
        }

        result.Success = true;
        result.Data = updatedEmployee;

        return Ok(result);
    }

    /// <summary>
    /// Added endpoint to allow consumer to remove an employee's record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>If successful, an Ok result is returned</returns>
    [SwaggerOperation(Summary = "Delete employee")]
    [HttpDelete("{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> DeleteEmployee(int employeeId)
    {
        var result = new ApiResponse<GetEmployeeDto>();
        var employee = await employeeService.GetEmployeeById(employeeId);
        if (employee == null)
        {
            result.Success = false;
            result.Error = $"Employee was not found in the repository";

            return NotFound(result);
        }

        var success = await employeeService.DeleteEmployee(employeeId);
        if (!success)
        {
            result.Success = false;
            result.Error = $"There was an error deleting the employee";

            return (result);
        }

        result.Success = true;

        return Ok(result);
    }
}
