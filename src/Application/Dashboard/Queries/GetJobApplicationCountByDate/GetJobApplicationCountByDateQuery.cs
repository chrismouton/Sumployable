using Sumployable.Application.Common.Interfaces;

namespace Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByDate;

public record GetJobApplicationCountByDateQuery(DateOnly Date) : IRequest<int>;

public class GetJobApplicationCountByDateQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetJobApplicationCountByDateQuery, int>
{
    public async Task<int> Handle(GetJobApplicationCountByDateQuery request, CancellationToken cancellationToken)
    {
        var endDate = request.Date.ToDateTime(TimeOnly.MaxValue);

        return await context.JobApplications
            .CountAsync(j => j.ApplicationDate <= endDate, cancellationToken);
    }
}
