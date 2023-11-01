using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Repositories.Employee;

namespace Api.Services;

public class EmployeeService : IEmployeeService
{
    //Use dependency injection for the repository layer
    private IEmployeeRepository employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }

    /// <summary>
    /// Method to get all of the employees from the database
    /// </summary>
    /// <returns>If successful, a list of employee records</returns>
    public async Task<List<GetEmployeeDto>> GetAllEmployees()
    {
        var employees = await employeeRepository.GetAllEmployees();

        return employees;
    }

    /// <summary>
    /// Method to get a specific employee record
    /// </summary>
    /// <param name="EmployeeId"></param>
    /// <returns>If successful, the employee record</returns>
    public async Task<GetEmployeeDto> GetEmployeeById(int EmployeeId)
    {
        var employees = await GetAllEmployees();
        var employee = employees.Where(q => q.Id == EmployeeId).FirstOrDefault();

        return employee;
    }

    /// <summary>
    /// Method to get the dependents for a specified employee by ID
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>If successful, a list of all the specified employee's dependents</returns>
    public async Task<List<GetDependentDto>> GetEmployeeDependents(int employeeId)
    {
        var employee = await GetEmployeeById(employeeId);
        var dependents = employee.Dependents.ToList();

        return dependents;
    }

    /// <summary>
    /// Method to get a specific dependent by ID
    /// </summary>
    /// <param name="dependentId"></param>
    /// <returns>If successful, the specified dependent record</returns>
    public async Task<GetDependentDto> GetDependentById(int dependentId)
    {
        var employees = await GetAllEmployees();
        var dependents = employees.SelectMany(q => q.Dependents).ToList();
        var dependent = dependents.Where(q => q.Id == dependentId).FirstOrDefault();

        return dependent;
    }

    /// <summary>
    /// Method to add a new employee record to the database
    /// </summary>
    /// <param name="employee"></param>
    /// <returns>If successful, the new employee record</returns>
    public async Task<GetEmployeeDto> AddNewEmployee(GetEmployeeDto employee)
    {
        var result = await employeeRepository.CreateEmployee(employee);

        return result;
    }

    /// <summary>
    /// Method to add a new dependent to the specified employee record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="dependent"></param>
    /// <returns>If successful, the updated employee record</returns>
    public async Task<GetEmployeeDto> AddNewDependent(int employeeId, GetDependentDto dependent)
    {
        var result = default(GetEmployeeDto);
        var employee = await GetEmployeeById(employeeId);

        if (!ValidateRelationship(employee, dependent))
        {
            throw new Exception("Cannot have more than one spouse or domestic partner");
        }
        
        employee.Dependents.Add(dependent);
        var updatedEmployee = await employeeRepository.UpdateEmployee(employeeId, employee);
        if (updatedEmployee != null)
        {
            result = employee;
        }

        return result;
    }

    /// <summary>
    /// Method to implement rule requirement: 1 spouse or domestic partner
    /// </summary>
    /// <param name="employee"></param>
    /// <param name="dependent"></param>
    /// <returns>True if valid, False if invalid</returns>
    private bool ValidateRelationship(GetEmployeeDto employee, GetDependentDto dependent)
    {
        var result = true;

        //If we're adding a new spouse or domestic partner        
        if (dependent.Relationship == Relationship.Spouse || dependent.Relationship == Relationship.DomesticPartner)
        {
            //Check to see if there is already a significant other
            if ((employee.Dependents.Any(q => q.Relationship == Relationship.Spouse)) ||
                (employee.Dependents.Any(q => q.Relationship == Relationship.DomesticPartner)))
            {
                result = false;
            }
        }

        return result;
    }

    /// <summary>
    /// Method to update an employee record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="employee"></param>
    /// <returns>If successful, the updated employee record</returns>
    public async Task<GetEmployeeDto> UpdateEmployee(int employeeId, GetEmployeeDto updatedEmployee)
    {
        var result = default(GetEmployeeDto);
        var employee = await GetEmployeeById(employeeId);
        if (employee != null)
        {
            result = await employeeRepository.UpdateEmployee(employeeId, updatedEmployee);
        }

        return result;
    }

    /// <summary>
    /// Method to update an employee's dependent record
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="dependentId"></param>
    /// <param name="dependent"></param>
    /// <returns>If successful, the updated dependent record</returns>
    public async Task<GetDependentDto> UpdateDependent(int employeeId, int dependentId, GetDependentDto updatedDependent)
    {
        var result = default(GetDependentDto);
        var employee = await GetEmployeeById(employeeId);
        var dependent = employee.Dependents.Where(q => q.Id == dependentId).FirstOrDefault();
        
        if (dependent != null)
        {
            if (!ValidateRelationship(employee, dependent))
            {
                throw new Exception("Cannot have more than one spouse or domestic partner");
            }

            dependent = updatedDependent;
            var updatedEmployee = await employeeRepository.UpdateEmployee(employeeId, employee);
            result = updatedEmployee.Dependents.Where(q => q.Id == dependentId).FirstOrDefault();
        }

        return result;
    }

    /// <summary>
    /// Method to delete an employee
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>If successful, true</returns>
    public async Task<bool> DeleteEmployee(int employeeId)
    {
        var success = await employeeRepository.DeleteEmployee(employeeId);

        return success;
    }

    /// <summary>
    /// Method to delete an employee's dependent
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="dependentId"></param>
    /// <returns>If successful, true</returns>
    public async Task<bool> DeleteDependent(int employeeId, int dependentId)
    {
        var result = false;
        var employee = await GetEmployeeById(employeeId);
        var dependent = employee.Dependents.Where(q => q.Id == dependentId).FirstOrDefault();

        if (dependent != null)
        {
            if (employee.Dependents.Remove(dependent))
            {
                await employeeRepository.UpdateEmployee(employeeId, employee);
                result = true;
            };
        }

        return result;
    }

    /// <summary>
    /// Method to calculate an employee's paycheck information
    /// </summary>
    /// <param name="employee"></param>
    /// <returns>If successful, the employee's paycheck data</returns>
    public async Task<GetPaycheckDto> GetPaycheckForEmployee(GetEmployeeDto employee)
    {
        var result = new GetPaycheckDto();

        //Determine gross
        result.GrossAmount = decimal.Round(employee.Salary / 26, 2, MidpointRounding.AwayFromZero);

        //Determine deductions
        result.TotalDeductions = await CalculateDeductions(employee);

        //Calculate net
        result.NetAmount = result.GrossAmount - result.TotalDeductions;

        return result;
    }

    /// <summary>
    /// Method to calculate employee deductions
    /// </summary>
    /// <param name="employee"></param>
    /// <returns>The total amount to be deducted from the employees paycheck</returns>
    private Task<decimal> CalculateDeductions(GetEmployeeDto employee)
    {
        //Base deduction
        decimal result = 1000.00m;

        //Dependents
        result += employee.Dependents.Count * 600m;

        //High Salary deduction
        result += employee.Salary > 80000 ? (employee.Salary * .02m) / 26 : 0.00m;

        //Elderly dependents
        var elderlyDependents = employee.Dependents.Where(q => GetDependentAge(q.DateOfBirth) > 50).ToList();
        result += elderlyDependents.Count * 200m;

        //Ensure the values are two decimals
        result = decimal.Round(result, 2, MidpointRounding.AwayFromZero);

        return Task.FromResult(result);
    }

    /// <summary>
    /// Method to determine the age given the date of birth
    /// </summary>
    /// <param name="dateOfBirth"></param>
    /// <returns>Age in years</returns>
    private int GetDependentAge(DateTime dateOfBirth)
    {
        var age = DateTime.Now.Year - dateOfBirth.Year;

        return age;
    }
}
