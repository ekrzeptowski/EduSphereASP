namespace EduSphere.Application.Features.Lesson.Commands.CreateLesson;

public class CreateLessonCommandValidator : AbstractValidator<CreateLessonCommand>
{
    public CreateLessonCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();

        RuleFor(v => v.Content)
            .MaximumLength(2000)
            .NotEmpty();
    }
}
