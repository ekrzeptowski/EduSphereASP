using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;
using EduSphere.Domain.Constants;

namespace EduSphere.Application.Features.Lesson.Commands.UpdateLesson;

[Authorize(Roles = Roles.Teacher)]
public record UpdateLessonCommand : IRequest
{
    public int LessonId { get; init; }
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
}

public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Lessons
            .FindAsync(new object[] { request.LessonId }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Lesson), request.LessonId.ToString());
        }

        entity.Title = request.Title;
        entity.Content = request.Content;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
