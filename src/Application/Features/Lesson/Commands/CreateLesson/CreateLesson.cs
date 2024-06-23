using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;

namespace EduSphere.Application.Features.Lesson.Commands.CreateLesson;

[Authorize(Roles = "Teacher")]
public record CreateLessonCommand : IRequest<int>
{
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public int CourseId { get; init; }
}

public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

        if (course == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Course), request.CourseId.ToString());
        }

        var entity = new Domain.Entities.Lesson
        {
            Title = request.Title, Content = request.Content, Course = course
        };

        _context.Lessons.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
