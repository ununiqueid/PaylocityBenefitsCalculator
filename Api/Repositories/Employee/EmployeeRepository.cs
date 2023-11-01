using Api.Dtos.Employee;
using Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Repositories.Employee;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IMongoCollection<GetEmployeeDto> employeeCollection;

    //Constructor to configure the database connection
    public EmployeeRepository(IOptions<MongoDbSettings> mongoDbSettings)
    { 
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        employeeCollection = mongoDatabase.GetCollection<GetEmployeeDto>(mongoDbSettings.Value.CollectionName);
    }

    /// <summary>
    /// Method to get all employees
    /// </summary>
    /// <returns>If successful, all employee records will be returned</returns>
    public async Task<List<GetEmployeeDto>> GetAllEmployees() => await employeeCollection.Find(_ => true).ToListAsync();

    /// <summary>
    /// Method to get a specific employee
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If successful, the specified employee record will be returned</returns>
    public async Task<GetEmployeeDto> GetEmployeeById(int id) => await employeeCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    /// <summary>
    /// Method to add a new employee
    /// </summary>
    /// <param name="newEmployee"></param>
    /// <returns>If successful, the new employee record will be returned</returns>
    public async Task<GetEmployeeDto> CreateEmployee(GetEmployeeDto newEmployee)
    {
        await employeeCollection.InsertOneAsync(newEmployee);
        return newEmployee;
    }

    /// <summary>
    /// Method to update an employee
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newEmployee"></param>
    /// <returns>If successful, the updated employee record will be returned</returns>
    public async Task<GetEmployeeDto> UpdateEmployee(int id, GetEmployeeDto newEmployee)
    {
        await employeeCollection.ReplaceOneAsync(x => x.Id == id, newEmployee);
        return newEmployee;
    }

    /// <summary>
    /// Method to delete an employee
    /// </summary>
    /// <param name="id"></param>
    /// <returns>If successful, true will be returned</returns>
    public async Task<bool> DeleteEmployee(int id)
    {
        var result = false;

        try
        {
            await employeeCollection.DeleteOneAsync(x => x.Id == id);
            result = true;
        }
        catch (Exception ex)
        {
            result = false;
        }

        return result;
    }
}
