using Sumployable.Application.Common.Interfaces;
using Sumployable.Domain.Entities;
using Sumployable.Domain.Enums;

namespace Sumployable.Application.JobApplications.Commands.CreateJobApplication;

public record CreateJobApplicationCommand : IRequest<int>
{
    public required string RoleName { get; init; }

    public string? CompanyName { get; init; }

    public RoleType RoleType { get; init; } = RoleType.Permanent;

    public Status Status { get; init; } = Status.Active;

    public ProcessStatus ProcessStatus { get; init; } = ProcessStatus.Applied;

    public Source Source { get; init; } = Source.JobBoard;

    public string? AdvertisedSalary { get; init; }

    public string? Url { get; init; }

    public string? Location { get; init; }

    public Commute Commute { get; init; } = Commute.Hybrid;

    public DateTime ApplicationDate { get; init; }

    public string? Note { get; init; }
}

public class CreateJobApplicationCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CreateJobApplicationCommand, int>
{
    public async Task<int> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var entity = new JobApplication
        {
            RoleName = request.RoleName,
            CompanyName = request.CompanyName,
            RoleType = request.RoleType,
            Status = request.Status,
            ProcessStatus = request.ProcessStatus,
            Source = request.Source,
            AdvertisedSalary = request.AdvertisedSalary,
            Url = request.Url,
            Location = request.Location,
            Commute = request.Commute,
            ApplicationDate = request.ApplicationDate,
            Note = request.Note
        };

        context.JobApplications.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
