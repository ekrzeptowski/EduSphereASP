using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;
using EduSphere.Domain.Constants;

namespace EduSphere.Application.Features.Course.Commands;

[Authorize(Roles = Roles.Teacher)]
public record DeleteCourseCommand(int Id) : IRequest;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCourseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Courses
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Course), request.Id.ToString());
        }

        //find if course has any enrollments

        if (await _context.Enrollments.AnyAsync(x => x.CourseId == request.Id, cancellationToken))
        {
            throw new ConflictException("Nie można usunąć kursu z zapisanymi uczestnikami.");
        }

        _context.Courses.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
