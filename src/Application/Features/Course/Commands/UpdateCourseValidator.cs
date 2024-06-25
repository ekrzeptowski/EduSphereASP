namespace EduSphere.Application.Features.Course.Commands;

public class UpdateCourseValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseValidator()
    {
        RuleFor(v => v.CourseId)
            .NotEmpty().WithMessage("CourseId is required.");

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.");
    }
}
