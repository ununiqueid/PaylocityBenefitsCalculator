namespace Api.Dtos.Paycheck;

/// <summary>
/// DTO to contain paycheck data
/// </summary>
public class GetPaycheckDto
{
    public decimal GrossAmount { get; set; } = 0;
    public decimal TotalDeductions { get; set; } = 0;
    public decimal NetAmount { get; set; } = 0;
}
