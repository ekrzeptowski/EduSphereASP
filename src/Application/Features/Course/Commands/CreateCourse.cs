using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;
using EduSphere.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace EduSphere.Application.Features.Course.Commands;

[Authorize(Roles = Roles.Administrator + "," + Roles.Teacher)]
public record CreateCourseCommand : IRequest<int>
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
{
    private readonly IApplicationDbContext _context;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCourseCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Course { Title = request.Title, Description = request.Description, };

        _context.Courses.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
