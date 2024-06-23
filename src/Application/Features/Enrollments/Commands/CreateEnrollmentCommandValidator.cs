namespace EduSphere.Application.Features.Enrollments.Commands;

public class CreateEnrollmentCommandValidator : AbstractValidator<CreateEnrollmentCommand>
{
    public CreateEnrollmentCommandValidator()
    {
        RuleFor(v => v.CourseId)
            .NotEmpty();
    }
}
