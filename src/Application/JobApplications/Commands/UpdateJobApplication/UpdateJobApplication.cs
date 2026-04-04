using Sumployable.Application.Common.Interfaces;
using Sumployable.Domain.Enums;

namespace Sumployable.Application.JobApplications.Commands.UpdateJobApplication;

public record UpdateJobApplicationCommand : IRequest
{
    public int Id { get; init; }

    public required string RoleName { get; init; }

    public string? CompanyName { get; init; }

    public RoleType RoleType { get; init; }

    public Status Status { get; init; }

    public ProcessStatus ProcessStatus { get; init; }

    public Source Source { get; init; }

    public string? AdvertisedSalary { get; init; }

    public string? Url { get; init; }

    public string? Location { get; init; }

    public Commute Commute { get; init; }

    public DateTime ApplicationDate { get; init; }

    public string? Note { get; init; }
}

public class UpdateJobApplicationCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateJobApplicationCommand>
{
    public async Task Handle(UpdateJobApplicationCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.JobApplications
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.RoleName = request.RoleName;
        entity.CompanyName = request.CompanyName;
        entity.RoleType = request.RoleType;
        entity.Status = request.Status;
        entity.ProcessStatus = request.ProcessStatus;
        entity.Source = request.Source;
        entity.AdvertisedSalary = request.AdvertisedSalary;
        entity.Url = request.Url;
        entity.Location = request.Location;
        entity.Commute = request.Commute;
        entity.ApplicationDate = request.ApplicationDate;
        entity.Note = request.Note;

        await context.SaveChangesAsync(cancellationToken);
    }
}
