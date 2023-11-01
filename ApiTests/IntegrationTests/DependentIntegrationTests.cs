using Api.Dtos.Dependent;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents");
        var dependents = new List<GetDependentDto>
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
            },
            new()
            {
                Id = 4,
                FirstName = "DP",
                LastName = "Jordan",
                Relationship = Relationship.DomesticPartner,
                DateOfBirth = new DateTime(1974, 1, 2, 0, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                Id = 6,
                FirstName = "Daisy",
                LastName = "Duke",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(1971, 10, 31, 0, 0, 0, DateTimeKind.Utc)
            }
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependents);
    }

    [Fact]
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents/1");
        var dependent = new GetDependentDto
        {
            Id = 1,
            FirstName = "Spouse",
            LastName = "Morant",
            Relationship = Relationship.Spouse,
            DateOfBirth = new DateTime(1998, 3, 3, 0, 0, 0, DateTimeKind.Utc)
        };
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    [Fact]
    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenCreatingADependentWithInvalidRelationshipStatus_ShouldReturnBadRequest()
    {
        var dependent = new GetDependentDto()
        {
            Id = 99,
            FirstName = "Domestic",
            LastName = "Partner",
            Relationship = Relationship.DomesticPartner,
            DateOfBirth = new DateTime(2002, 5, 8, 0, 0, 0, DateTimeKind.Utc)
        };
        var response = await HttpClient.PostAsJsonAsync("/api/v1/dependents/2", dependent);

        await response.ShouldReturn(HttpStatusCode.BadRequest);
    }
}

