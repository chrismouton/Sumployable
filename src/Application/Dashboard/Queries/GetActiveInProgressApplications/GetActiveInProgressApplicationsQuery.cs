using Sumployable.Application.Common.Interfaces;
using Sumployable.Domain.Enums;

namespace Sumployable.Application.Dashboard.Queries.GetActiveInProgressApplications;

public record GetActiveInProgressApplicationsQuery : IRequest<IReadOnlyList<ActiveInProgressApplicationDto>>;

public class GetActiveInProgressApplicationsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetActiveInProgressApplicationsQuery, IReadOnlyList<ActiveInProgressApplicationDto>>
{
    private static readonly ProcessStatus[] ExcludedStatuses =
        [ProcessStatus.Applied, ProcessStatus.Hired, ProcessStatus.Rejected, ProcessStatus.Retracted];

    public async Task<IReadOnlyList<ActiveInProgressApplicationDto>> Handle(
        GetActiveInProgressApplicationsQuery request, CancellationToken cancellationToken)
    {
        return await context.JobApplications
            .Where(j => j.Status == Status.Active && !ExcludedStatuses.Contains(j.ProcessStatus))
            .OrderBy(j => j.ProcessStatus)
            .ThenBy(j => j.ApplicationDate)
            .Select(j => new ActiveInProgressApplicationDto
            {
                CompanyName = j.CompanyName,
                RoleName = j.RoleName,
                ProcessStatusName = j.ProcessStatus.ToString(),
                ApplicationDate = j.ApplicationDate,
                Location = j.Location,
                CommuteName = j.Commute.ToString()
            })
            .ToListAsync(cancellationToken);
    }
}
