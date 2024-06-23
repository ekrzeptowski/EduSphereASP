using EduSphere.Application.Features.Lesson.Queries;

namespace EduAPI.Endpoints;

public class Lessons : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("/api/lesson")
            .WithTags(this.GetType().Name)
            .RequireAuthorization()
            .MapGet(GetLesson, "{lessonId}");
    }

    public async Task<LessonDto> GetLesson(ISender sender, int lessonId)
    {
        return await sender.Send(new GetLessonQuery(lessonId));
    }
}
