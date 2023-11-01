using Api.Dtos.Paycheck;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.IntegrationTests;

public class PaycheckIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task WhenAskedForAPaycheckForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync("/api/v1/paychecks/199");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenAskedForAPaycheckForAnEmployeeWithoutDependents_ShouldReturnCorrectPaycheckWithBaseDeduction()
    {
        var response = await HttpClient.GetAsync("/api/v1/paychecks/1");
        var paycheck = new GetPaycheckDto()
        {
            GrossAmount = 2900.81m,
            TotalDeductions = 1000.00m,
            NetAmount = 1900.81m

        };
        await response.ShouldReturn(HttpStatusCode.OK, paycheck);
    }

    [Fact]
    public async Task WhenAskedForAPaycheckForAnEmployeeWithDependents_ShouldReturnCorrectPaycheck()
    {
        var response = await HttpClient.GetAsync("/api/v1/paychecks/2");

        var deductions = 1000.00m;
        deductions += 3 * 600.00m;
        deductions += (92365.22m * .02m) / 26;
        deductions = decimal.Round(deductions, 2, System.MidpointRounding.AwayFromZero);

        var paycheck = new GetPaycheckDto()
        {
            GrossAmount = 3552.51m,
            TotalDeductions = deductions,
            NetAmount = 3552.51m - deductions
        };
        await response.ShouldReturn(HttpStatusCode.OK, paycheck);
    }

    [Fact]
    public async Task WhenAskedForAPaycheckForAnEmployeeWithAnElderlyDependent_ShouldReturnCorrectPaycheck()
    {
        var response = await HttpClient.GetAsync("/api/v1/paychecks/4");
        var paycheck = new GetPaycheckDto()
        {
            GrossAmount = 1132.53m,
            TotalDeductions = 1800.00m,
            NetAmount = -667.47m
        };
        await response.ShouldReturn(HttpStatusCode.OK, paycheck);
    }

}
