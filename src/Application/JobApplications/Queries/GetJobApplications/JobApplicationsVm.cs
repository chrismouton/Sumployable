using Sumployable.Application.Common.Models;

namespace Sumployable.Application.JobApplications.Queries.GetJobApplications;

public class JobApplicationsVm
{
    public IReadOnlyCollection<LookupDto> RoleTypes { get; init; } = [];

    public IReadOnlyCollection<LookupDto> Statuses { get; init; } = [];

    public IReadOnlyCollection<LookupDto> ProcessStatuses { get; init; } = [];

    public IReadOnlyCollection<LookupDto> Sources { get; init; } = [];

    public IReadOnlyCollection<LookupDto> Commutes { get; init; } = [];

    public IReadOnlyCollection<JobApplicationDto> JobApplications { get; init; } = [];
}
