using Sumployable.Application.JobApplications.Commands.CreateJobApplication;
using Sumployable.Application.JobApplications.Commands.UpdateJobApplication;
using Sumployable.Application.JobApplications.Queries.GetJobApplications;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Sumployable.Web.Endpoints;

public class JobApplications : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetJobApplications);
        groupBuilder.MapPost(CreateJobApplication);
        groupBuilder.MapPut(UpdateJobApplication, "{id}");
    }

    [EndpointSummary("Get all Job Applications")]
    [EndpointDescription("Retrieves all job applications.")]
    public static async Task<Ok<JobApplicationsVm>> GetJobApplications(ISender sender)
    {
        var vm = await sender.Send(new GetJobApplicationsQuery());

        return TypedResults.Ok(vm);
    }

    [EndpointSummary("Create a new Job Application")]
    [EndpointDescription("Creates a new job application and returns the ID of the created record.")]
    public static async Task<Created<int>> CreateJobApplication(ISender sender, CreateJobApplicationCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(JobApplications)}/{id}", id);
    }

    [EndpointSummary("Update a Job Application")]
    [EndpointDescription("Updates the specified job application. The ID in the URL must match the ID in the payload.")]
    public static async Task<Results<NoContent, BadRequest>> UpdateJobApplication(ISender sender, int id, UpdateJobApplicationCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }
}
