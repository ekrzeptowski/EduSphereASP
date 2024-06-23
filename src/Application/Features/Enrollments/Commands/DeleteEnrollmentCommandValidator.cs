namespace EduSphere.Application.Features.Enrollments.Commands;

public class DeleteEnrollmentCommandValidator : AbstractValidator<DeleteEnrollmentCommand>
{
    public DeleteEnrollmentCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
    }
}
