namespace Sumployable.Application.Dashboard.Queries.GetActiveInProgressApplications;

public class ActiveInProgressApplicationDto
{
    public string? CompanyName { get; init; }
    public required string RoleName { get; init; }
    public required string ProcessStatusName { get; init; }
    public DateTime ApplicationDate { get; init; }
    public string? Location { get; init; }
    public required string CommuteName { get; init; }
}
