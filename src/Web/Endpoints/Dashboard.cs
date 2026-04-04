using Sumployable.Application.Dashboard.Queries.GetDashboard;
using Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByDate;
using Sumployable.Application.Dashboard.Queries.GetJobApplicationsPerDay;
using Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByRoleType;
using Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByProcessStatus;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Sumployable.Web.Endpoints;

public class Dashboard : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetDashboard);
        groupBuilder.MapGet(GetJobApplicationCountByDate, "job-application-count-by-date");
        groupBuilder.MapGet(GetJobApplicationsPerDay, "job-applications-per-day");
        groupBuilder.MapGet(GetJobApplicationCountByRoleType, "job-application-count-by-role-type");
        groupBuilder.MapGet(GetJobApplicationCountByProcessStatus, "job-application-count-by-process-status");
    }

    [EndpointSummary("Get Dashboard")]
    [EndpointDescription("Retrieves aggregated statistics for the dashboard.")]
    public static async Task<Ok<DashboardVm>> GetDashboard(ISender sender)
    {
        var vm = await sender.Send(new GetDashboardQuery());

        return TypedResults.Ok(vm);
    }

    [EndpointSummary("Get Job Application Count By Date")]
    [EndpointDescription("Returns the cumulative count of job applications with an ApplicationDate on or before the specified date.")]
    public static async Task<Ok<int>> GetJobApplicationCountByDate(ISender sender, DateOnly date)
    {
        var count = await sender.Send(new GetJobApplicationCountByDateQuery(date));

        return TypedResults.Ok(count);
    }

    [EndpointSummary("Get Job Applications Per Day")]
    [EndpointDescription("Returns the count of job applications per day from the given start date up to today, with zero-filled days included.")]
    public static async Task<Ok<IReadOnlyList<JobApplicationCountPerDayDto>>> GetJobApplicationsPerDay(ISender sender, DateOnly from)
    {
        var result = await sender.Send(new GetJobApplicationsPerDayQuery(from));

        return TypedResults.Ok(result);
    }

    [EndpointSummary("Get Job Application Count By Role Type")]
    [EndpointDescription("Returns the count of job applications grouped by role type. All role types are included, with zero counts where there are no applications.")]
    public static async Task<Ok<IReadOnlyList<RoleTypeCountDto>>> GetJobApplicationCountByRoleType(ISender sender)
    {
        var result = await sender.Send(new GetJobApplicationCountByRoleTypeQuery());

        return TypedResults.Ok(result);
    }

    [EndpointSummary("Get Job Application Count By Process Status")]
    [EndpointDescription("Returns the count of job applications grouped by process status. All process statuses are included, with zero counts where there are no applications.")]
    public static async Task<Ok<IReadOnlyList<ProcessStatusCountDto>>> GetJobApplicationCountByProcessStatus(ISender sender)
    {
        var result = await sender.Send(new GetJobApplicationCountByProcessStatusQuery());

        return TypedResults.Ok(result);
    }
}
