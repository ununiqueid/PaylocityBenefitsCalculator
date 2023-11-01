using Api.Dtos.Employee;

namespace Api.Repositories.Employee;

public interface IEmployeeRepository
{
    /// <summary>
    /// Method to retrieve all employee records from the database 
    /// </summary>
    /// <returns>A list of employee records</returns>
    Task<List<GetEmployeeDto>> GetAllEmployees();

    /// <summary>
    /// Method to retrieve specified employee record from the database 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If found, the specific employee record</returns>
    Task<GetEmployeeDto> GetEmployeeById(int id);

    /// <summary>
    /// Method to add a new employee record to the database
    /// </summary>
    /// <param name="newEmployee"></param>
    /// <returns>If successful, the new employee record</returns>
    Task<GetEmployeeDto> CreateEmployee(GetEmployeeDto newEmployee);

    /// <summary>
    /// Method to update a specified employee record in the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newEmployee"></param>
    /// <returns>If successful, the updated employee record</returns>
    Task<GetEmployeeDto> UpdateEmployee(int id, GetEmployeeDto newEmployee);

    /// <summary>
    /// Method to delete the specified employee record from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If successful, true</returns>
    Task<bool> DeleteEmployee(int id);
}
