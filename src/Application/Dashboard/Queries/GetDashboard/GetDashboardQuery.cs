using Sumployable.Application.Common.Interfaces;

namespace Sumployable.Application.Dashboard.Queries.GetDashboard;

public record GetDashboardQuery : IRequest<DashboardVm>;

public class GetDashboardQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetDashboardQuery, DashboardVm>
{
    public async Task<DashboardVm> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var totalJobApplications = await context.JobApplications.CountAsync(cancellationToken);

        return new DashboardVm { TotalJobApplications = totalJobApplications };
    }
}
