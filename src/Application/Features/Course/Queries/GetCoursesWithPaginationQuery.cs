using System.Security.Claims;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Mappings;
using EduSphere.Application.Common.Models;
using EduSphere.Application.Common.Security;
using EduSphere.Application.Features.Lesson.Queries;
using Microsoft.AspNetCore.Http;

namespace EduSphere.Application.Features.Course.Queries;

[Authorize]
public record GetCoursesWithPaginationQuery : IRequest<PaginatedList<CourseDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCoursesWithPaginationHandler : IRequestHandler<GetCoursesWithPaginationQuery, PaginatedList<CourseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public GetCoursesWithPaginationHandler(IApplicationDbContext context, IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PaginatedList<CourseDto>> Handle(GetCoursesWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        // for courses that the user is not enrolled in return a course with no lessons
        return await _context.Courses
            .OrderBy(x => x.Title)
            .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Lessons =
                    _context.Enrollments.Any(e => e.Student.Id == userId && e.Course.Id == c.Id) || c.Lessons == null
                        ? c.Lessons
                        : c.Lessons.Select(l => new LessonDto { Id = l.Id, Title = l.Title, Content = "" })
                            .ToList(),
                IsEnrolled = _context.Enrollments.Any(e => e.Student.Id == userId && e.Course.Id == c.Id)
            })
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
