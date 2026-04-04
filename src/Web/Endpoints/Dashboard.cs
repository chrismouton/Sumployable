using Sumployable.Application.Dashboard.Queries.GetDashboard;
using Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByDate;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Sumployable.Web.Endpoints;

public class Dashboard : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetDashboard);
        groupBuilder.MapGet(GetJobApplicationCountByDate, "job-application-count-by-date");
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
}
