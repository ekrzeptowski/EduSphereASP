using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;
using EduSphere.Domain.Constants;

namespace EduSphere.Application.Features.Course.Commands;

[Authorize(Roles = Roles.Teacher)]
public record UpdateCourseCommand : IRequest
{
    public int CourseId { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
}

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCourseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Courses
            .FindAsync(new object[] { request.CourseId }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Course), request.CourseId.ToString());
        }

        entity.Title = request.Title;
        entity.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
