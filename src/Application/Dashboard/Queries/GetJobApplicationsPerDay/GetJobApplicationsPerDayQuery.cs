using Sumployable.Application.Common.Interfaces;

namespace Sumployable.Application.Dashboard.Queries.GetJobApplicationsPerDay;

public record GetJobApplicationsPerDayQuery(DateOnly From) : IRequest<IReadOnlyList<JobApplicationCountPerDayDto>>;

public class GetJobApplicationsPerDayQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetJobApplicationsPerDayQuery, IReadOnlyList<JobApplicationCountPerDayDto>>
{
    public async Task<IReadOnlyList<JobApplicationCountPerDayDto>> Handle(
        GetJobApplicationsPerDayQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.From.ToDateTime(TimeOnly.MinValue);

        // Group by date in the DB — only days that have applications
        var counts = await context.JobApplications
            .Where(j => j.ApplicationDate >= startDate)
            .GroupBy(j => j.ApplicationDate.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        // Zero-fill: generate every day from From to today
        var today = DateOnly.FromDateTime(DateTime.Today);
        var result = new List<JobApplicationCountPerDayDto>();

        for (var day = request.From; day <= today; day = day.AddDays(1))
        {
            var match = counts.FirstOrDefault(c => DateOnly.FromDateTime(c.Date) == day);
            result.Add(new JobApplicationCountPerDayDto { Date = day, Count = match?.Count ?? 0 });
        }

        return result;
    }
}
