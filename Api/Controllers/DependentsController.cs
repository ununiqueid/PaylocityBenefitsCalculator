using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    //Use dependency injection for service layer
    private IEmployeeService employeeService;

    public DependentsController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    /// <summary>
    /// This endpoint will return all of the dependents from the database
    /// </summary>
    /// <returns>If successful, the data object will contain all of the dependent records</returns>
    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var result = new ApiResponse<List<GetDependentDto>>();
        var dependents = new List<GetDependentDto>();
        var employees = await employeeService.GetAllEmployees();

        //Add all of the dependents to the list of dependents
        foreach(var employee in employees)
        {
            if (employee.Dependents.Any())
            {
                dependents.AddRange(employee.Dependents);
            }
        }

        if (dependents == null)
        {
            result.Success = false;
            result.Error = $"No dependents were found";

            return NotFound(result);
        }

        result.Success = true;
        result.Data = dependents;

        return Ok(result);
    }

    /// <summary>
    /// This endpoint will get the specified dependent from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If successful, the data object will contain the dependent's record</returns>
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var result = new ApiResponse<GetDependentDto>();
        var dependent = await employeeService.GetDependentById(id);
        if (dependent == null)
        {
            result.Success = false;
            result.Error = $"Dependent was not found in the repository";

            return NotFound(result);
        }

        result.Success = true;
        result.Data = dependent;

        return result;
    }

    /// <summary>
    /// Added endpoint to allow consumer to update an employee with a new dependent after a qualifying life event
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="newDependent"></param>
    /// <returns>If successful, the data object will contain the updated employee record</returns>
    [SwaggerOperation(Summary = "Add a new dependent to the specified employee")]
    [HttpPost("{employeeId}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> AddNewDependent(int employeeId, [FromBody] GetDependentDto newDependent)
    {
        var result = new ApiResponse<GetEmployeeDto>();

        //Get the employee by ID
        var employee = await employeeService.GetEmployeeById(employeeId);
        if (employee == null)
        {
            result.Success = false;
            result.Error = $"Employee was not found in the repository";

            return result;
        }
        
        try
        {
            //Update the employee record with the new dependent
            employee = await employeeService.AddNewDependent(employeeId, newDependent);

            result.Success = true;
            result.Data = employee;

            return Ok(result);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Error = ex.Message;
            
            return BadRequest(result);
        }      
    }

    /// <summary>
    /// Added endpoint to allow consumer to update an employee's specific dependent record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="dependentId"></param>
    /// <param name="updatedDependent"></param>
    /// <returns>If successful, the data object will contain the updated employee record</returns>
    [SwaggerOperation(Summary = "Update the specified dependent")]
    [HttpPut("{employeeId}/{dependentId}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> UpdateDependent(int employeeId, int dependentId, [FromBody] GetDependentDto updatedDependent)
    {
        var result = new ApiResponse<GetEmployeeDto>();

        //Get the employee by ID
        var employee = await employeeService.GetEmployeeById(employeeId);
        if (employee == null)
        {
            result.Success = false;
            result.Error = $"Employee was not found in the repository";

            return result;
        }

        //Get the dependent by ID
        var dependent = employee.Dependents.Where(q => q.Id == dependentId).FirstOrDefault();
        if (dependent == null)
        {
            result.Success = false;
            result.Error = $"Dependent was not found in the repository";

            return NotFound(result);
        }

        //Update the dependent and update the employee record
        dependent = updatedDependent;
        var updatedEmployee = await employeeService.UpdateEmployee(employeeId, employee);
        if (updatedEmployee == null)
        {
            result.Success = false;
            result.Error = $"There was an error updating the dependent";

            return NotFound(result);
        }

        result.Success = true;
        result.Data = updatedEmployee;

        return Ok(result);
    }

    /// <summary>
    /// Added this endpoint to allow consumer to delete the specified dependent due to a qualifying life event
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="dependentId"></param>
    /// <returns>If successful, the data object will contain the updated employee record</returns>
    [SwaggerOperation(Summary = "Delete the specified dependent")]
    [HttpDelete("{employeeId}/{dependentId}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> DeleteDependent(int employeeId, int dependentId)
    {
        var result = new ApiResponse<GetEmployeeDto>();

        //Get the employee by ID
        var employee = await employeeService.GetEmployeeById(employeeId);
        if (employee == null)
        {
            result.Success = false;
            result.Error = $"Employee was not found in the repository";

            return result;
        }

        //Get the dependent by ID
        var dependent = employee.Dependents.Where(q => q.Id == dependentId).FirstOrDefault();
        if (dependent == null)
        {
            result.Success = false;
            result.Error = $"Dependent was not found in the repository";

            return NotFound(result);
        }

        //Remove the dependent and update the employee record
        employee.Dependents.Remove(dependent);
        var updatedEmployee = await employeeService.UpdateEmployee(employeeId, employee);
        if (updatedEmployee == null)
        {
            result.Success = false;
            result.Error = $"There was an error deleting the dependent";

            return NotFound(result);
        }

        result.Success = true;
        result.Data = updatedEmployee;

        return Ok(result);
    }
}
