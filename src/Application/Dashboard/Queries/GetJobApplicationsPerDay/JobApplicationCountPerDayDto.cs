namespace Sumployable.Application.Dashboard.Queries.GetJobApplicationsPerDay;

public class JobApplicationCountPerDayDto
{
    public DateOnly Date { get; init; }
    public int Count { get; init; }
}
