using Sumployable.Application.Common.Interfaces;
using Sumployable.Domain.Enums;

namespace Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByProcessStatus;

public record GetJobApplicationCountByProcessStatusQuery : IRequest<IReadOnlyList<ProcessStatusCountDto>>;

public class GetJobApplicationCountByProcessStatusQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetJobApplicationCountByProcessStatusQuery, IReadOnlyList<ProcessStatusCountDto>>
{
    public async Task<IReadOnlyList<ProcessStatusCountDto>> Handle(
        GetJobApplicationCountByProcessStatusQuery request, CancellationToken cancellationToken)
    {
        var counts = await context.JobApplications
            .GroupBy(j => j.ProcessStatus)
            .Select(g => new { ProcessStatus = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return Enum.GetValues<ProcessStatus>()
            .Select(ps =>
            {
                var match = counts.FirstOrDefault(c => c.ProcessStatus == ps);
                return new ProcessStatusCountDto
                {
                    ProcessStatus = (int)ps,
                    ProcessStatusName = ps.ToString(),
                    Count = match?.Count ?? 0
                };
            })
            .ToList();
    }
}
