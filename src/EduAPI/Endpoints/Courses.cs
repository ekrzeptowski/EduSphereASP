using EduSphere.Application.Common.Models;
using EduSphere.Application.Features.Course.Commands;
using EduSphere.Application.Features.Course.Queries;

namespace EduAPI.Endpoints;

public class Courses : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("/api/course")
            .WithTags(this.GetType().Name)
            .RequireAuthorization()
            .MapGet(GetCoursesWithPagination)
            .MapGet(GetCourse, "{courseId}")
            .MapPost(CreateCourse);
    }

    public async Task<PaginatedList<CourseDto>> GetCoursesWithPagination(ISender sender,
        [AsParameters] GetCoursesWithPaginationQuery getCoursesWithPaginationQuery)
    {
        return await sender.Send(getCoursesWithPaginationQuery);
    }

    public async Task<CourseDto> GetCourse(ISender sender, int courseId)
    {
        return await sender.Send(new GetCourseQuery(courseId));
    }

    public async Task<int> CreateCourse(ISender sender, CreateCourseCommand createCourseCommand)
    {
        return await sender.Send(createCourseCommand);
    }
}
