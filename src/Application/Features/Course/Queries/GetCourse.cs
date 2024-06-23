using System.Security.Claims;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;
using EduSphere.Application.Features.Lesson.Queries;
using Microsoft.AspNetCore.Http;

namespace EduSphere.Application.Features.Course.Queries;

[Authorize]
public record GetCourseQuery(int Id) : IRequest<CourseDto>;

public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, CourseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCourseQueryHandler(IApplicationDbContext context, IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CourseDto> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
        {
            throw new NotFoundException(nameof(Course), request.Id.ToString());
        }

        // Get the current userId from the ClaimsPrincipal
        var userId = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        // Check if the current user is enrolled in the course
        var isEnrolled = await _context.Enrollments
            .AnyAsync(e => e.Student.Id == userId && e.Course.Id == course.Id, cancellationToken);

        if (!isEnrolled)
        {
            // If the user is not enrolled, return a course with no lessons
            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Lessons = new List<LessonDto>()
            };
        }

        return course;
    }
}
