namespace Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByProcessStatus;

public class ProcessStatusCountDto
{
    public int ProcessStatus { get; init; }
    public string ProcessStatusName { get; init; } = string.Empty;
    public int Count { get; init; }
}
