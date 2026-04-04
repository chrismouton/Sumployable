namespace Sumployable.Application.JobApplications.Commands.UpdateJobApplication;

public class UpdateJobApplicationCommandValidator : AbstractValidator<UpdateJobApplicationCommand>
{
    public UpdateJobApplicationCommandValidator()
    {
        RuleFor(v => v.RoleName)
            .NotEmpty()
            .MaximumLength(200);
    }
}
