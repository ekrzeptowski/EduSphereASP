using System.Security.Claims;
using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EduSphere.Application.Features.Enrollments.Commands;

public record DeleteEnrollmentCommand : IRequest
{
    public string StudentId { get; init; } = null!;
    public int Id { get; init; }
}

public class DeleteEnrollmentCommandHandler : IRequestHandler<DeleteEnrollmentCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteEnrollmentCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(DeleteEnrollmentCommand request, CancellationToken cancellationToken)
    {
        if (request.StudentId is null)
        {
            if (_httpContextAccessor.HttpContext?.User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value is null)
            {
                throw new ForbiddenAccessException();
            }
        }

        var entity = await _context.Enrollments.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Enrollment), request.Id.ToString());
        }

        _context.Enrollments.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
