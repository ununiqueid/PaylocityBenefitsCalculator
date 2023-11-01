using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest
{
    //[Fact]
    //public async Task WhenCreatingAnEmployee_ShouldReturn200Ok()
    //{
    //    var employee = new GetEmployeeDto()
    //    {
    //        Id = 1,
    //        FirstName = "LeBron",
    //        LastName = "James",
    //        Salary = 75420.99m,
    //        DateOfBirth = new DateTime(1984, 12, 30)
    //    };
    //    var response = await HttpClient.PostAsJsonAsync("/api/v1/employees", employee);

    //    await response.ShouldReturn(HttpStatusCode.OK, employee);
    //}

    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        var employees = new List<GetEmployeeDto>
        {
            new()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30, 6, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1998, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23, 0, 0, 0, DateTimeKind.Utc)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17, 0, 0, 0, DateTimeKind.Utc),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            },
            new()
            {
                Id = 4,
                FirstName = "Jesse",
                LastName = "Duke",
                Salary = 29445.85m,
                DateOfBirth = new DateTime(1966,2,21,0,0,0, DateTimeKind.Utc),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {
                        Id = 6,
                        FirstName = "Daisy",
                        LastName = "Duke",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(1971,10,31,0,0,0, DateTimeKind.Utc)
                    }
                }
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, employees);
    }

    [Fact]
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees/1");
        var employee = new GetEmployeeDto
        {
            Id = 1,
            FirstName = "LeBron",
            LastName = "James",
            Salary = 75420.99m,
            DateOfBirth = new DateTime(1984, 12, 30, 6, 0, 0, DateTimeKind.Utc)
        };
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }
    
    [Fact]
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}
