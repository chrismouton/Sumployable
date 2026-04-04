using Sumployable.Application.Common.Interfaces;
using Sumployable.Application.Common.Models;
using Sumployable.Domain.Enums;

namespace Sumployable.Application.JobApplications.Queries.GetJobApplications;

public record GetJobApplicationsQuery : IRequest<JobApplicationsVm>;

public class GetJobApplicationsQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetJobApplicationsQuery, JobApplicationsVm>
{
    public async Task<JobApplicationsVm> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken)
    {
        return new JobApplicationsVm
        {
            RoleTypes = Enum.GetValues(typeof(RoleType))
                .Cast<RoleType>()
                .Select(r => new LookupDto { Id = (int)r, Title = r.ToString() })
                .ToList(),

            Statuses = Enum.GetValues(typeof(Status))
                .Cast<Status>()
                .Select(s => new LookupDto { Id = (int)s, Title = s.ToString() })
                .ToList(),

            ProcessStatuses = Enum.GetValues(typeof(ProcessStatus))
                .Cast<ProcessStatus>()
                .Select(p => new LookupDto { Id = (int)p, Title = p.ToString() })
                .ToList(),

            Sources = Enum.GetValues(typeof(Source))
                .Cast<Source>()
                .Select(s => new LookupDto { Id = (int)s, Title = s.ToString() })
                .ToList(),

            Commutes = Enum.GetValues(typeof(Commute))
                .Cast<Commute>()
                .Select(c => new LookupDto { Id = (int)c, Title = c.ToString() })
                .ToList(),

            JobApplications = await context.JobApplications
                .AsNoTracking()
                .ProjectTo<JobApplicationDto>(mapper.ConfigurationProvider)
                .OrderByDescending(j => j.ApplicationDate)
                .ToListAsync(cancellationToken)
        };
    }
}
