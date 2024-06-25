using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;
using EduSphere.Domain.Constants;

namespace EduSphere.Application.Features.Lesson.Commands.DeleteLesson;

[Authorize(Roles = Roles.Teacher)]
public record DeleteLessonCommand(int LessonId) : IRequest;

public class DeleteLessonCommandHandler : IRequestHandler<DeleteLessonCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Lessons
            .FindAsync(new object[] { request.LessonId }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Lesson), request.LessonId.ToString());
        }

        _context.Lessons.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
