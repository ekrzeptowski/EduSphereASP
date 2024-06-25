namespace EduSphere.Application.Features.Lesson.Commands.UpdateLesson;

public class UpdateLessonValidator : AbstractValidator<UpdateLessonCommand>
{
    public UpdateLessonValidator()
    {
        RuleFor(v => v.LessonId)
            .NotEmpty().WithMessage("LessonId is required.");

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(v => v.Content)
            .NotEmpty().WithMessage("Content is required.");
    }
}
