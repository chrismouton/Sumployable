using Sumployable.Application.Common.Interfaces;
using Sumployable.Domain.Enums;

namespace Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByRoleType;

public record GetJobApplicationCountByRoleTypeQuery : IRequest<IReadOnlyList<RoleTypeCountDto>>;

public class GetJobApplicationCountByRoleTypeQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetJobApplicationCountByRoleTypeQuery, IReadOnlyList<RoleTypeCountDto>>
{
    public async Task<IReadOnlyList<RoleTypeCountDto>> Handle(
        GetJobApplicationCountByRoleTypeQuery request, CancellationToken cancellationToken)
    {
        // Group by RoleType in the DB
        var counts = await context.JobApplications
            .GroupBy(j => j.RoleType)
            .Select(g => new { RoleType = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        // Return all enum values, zero-filling those with no applications
        return Enum.GetValues<RoleType>()
            .Select(rt =>
            {
                var match = counts.FirstOrDefault(c => c.RoleType == rt);
                return new RoleTypeCountDto
                {
                    RoleType = (int)rt,
                    RoleTypeName = rt.ToString(),
                    Count = match?.Count ?? 0
                };
            })
            .ToList();
    }
}
