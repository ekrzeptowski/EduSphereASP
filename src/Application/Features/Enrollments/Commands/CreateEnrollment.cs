using System.Security.Claims;
using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;
using EduSphere.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace EduSphere.Application.Features.Enrollments.Commands;

[Authorize(Roles = Roles.Student)]
public record CreateEnrollmentCommand : IRequest<int>
{
    public string StudentId { get; init; } = null!;
    public int CourseId { get; init; }
}

public class CreateEnrollmentCommandHandler : IRequestHandler<CreateEnrollmentCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContext;

    public CreateEnrollmentCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }

    public async Task<int> Handle(CreateEnrollmentCommand request, CancellationToken cancellationToken)
    {
        if (request.StudentId is null)
        {
            if (_httpContext.HttpContext?.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value is null)
            {
                throw new ForbiddenAccessException();
            }
        }

        var entity = new Domain.Entities.Enrollment
        {
            StudentId = request.StudentId ?? _httpContext.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
            CourseId = request.CourseId,
        };

        _context.Enrollments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
