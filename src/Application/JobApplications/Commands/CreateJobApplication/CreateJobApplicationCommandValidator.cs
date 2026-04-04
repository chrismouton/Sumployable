namespace Sumployable.Application.JobApplications.Commands.CreateJobApplication;

public class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
{
    public CreateJobApplicationCommandValidator()
    {
        RuleFor(v => v.RoleName)
            .NotEmpty()
            .MaximumLength(200);
    }
}
