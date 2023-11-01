using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;

namespace Api.Services;

public interface IEmployeeService
{
    /// <summary>
    /// Method to get all of the employee records
    /// </summary>
    /// <returns>If successful, a list of all employee records</returns>
    Task<List<GetEmployeeDto>> GetAllEmployees();

    /// <summary>
    /// Method to get a specific employee by ID
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>If successful, the specified employee record</returns>
    Task<GetEmployeeDto> GetEmployeeById(int employeeId);

    /// <summary>
    /// Method to get the dependents for a specified employee by ID
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>If successful, a list of all the specified employee's dependents</returns>
    Task<List<GetDependentDto>> GetEmployeeDependents(int employeeId);

    /// <summary>
    /// Method to get a specific dependent by ID
    /// </summary>
    /// <param name="dependentId"></param>
    /// <returns>If successful, the specified dependent record</returns>
    Task<GetDependentDto> GetDependentById(int dependentId);

    /// <summary>
    /// Method to add a new employee record to the database
    /// </summary>
    /// <param name="employee"></param>
    /// <returns>If successful, the new employee record</returns>
    Task<GetEmployeeDto> AddNewEmployee(GetEmployeeDto employee);

    /// <summary>
    /// Method to add a new dependent to the specified employee record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="dependent"></param>
    /// <returns>If successful, the updated employee record</returns>
    Task<GetEmployeeDto> AddNewDependent(int employeeId, GetDependentDto dependent);

    /// <summary>
    /// Method to update an employee record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="employee"></param>
    /// <returns>If successful, the updated employee record</returns>
    Task<GetEmployeeDto> UpdateEmployee(int employeeId, GetEmployeeDto employee);

    /// <summary>
    /// Method to update an employee's dependent record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="dependentId"></param>
    /// <param name="dependent"></param>
    /// <returns>If successful, the updated dependent record</returns>
    Task<GetDependentDto> UpdateDependent(int employeeId, int dependentId, GetDependentDto dependent);

    /// <summary>
    /// Method to delete an employee
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>If successful, true</returns>
    Task<bool> DeleteEmployee(int employeeId);

    /// <summary>
    /// Method to delete an employee's dependent
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="dependentId"></param>
    /// <returns>If successful, true</returns>
    Task<bool> DeleteDependent(int employeeId, int dependentId);
    
    /// <summary>
    /// Method to calculate an employee's paycheck information
    /// </summary>
    /// <param name="employee"></param>
    /// <returns>If successful, the employee's paycheck data</returns>
    Task<GetPaycheckDto> GetPaycheckForEmployee(GetEmployeeDto employee);
}