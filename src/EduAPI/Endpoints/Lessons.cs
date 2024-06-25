using EduSphere.Application.Features.Lesson.Commands.CreateLesson;
using EduSphere.Application.Features.Lesson.Queries;

namespace EduAPI.Endpoints;

public class Lessons : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("/api/lesson")
            .WithTags(this.GetType().Name)
            .RequireAuthorization()
            .MapGet(GetLesson, "{lessonId}")
            .MapPost(CreateLesson);
    }

    public async Task<LessonDto> GetLesson(ISender sender, int lessonId)
    {
        return await sender.Send(new GetLessonQuery(lessonId));
    }

    public async Task<int> CreateLesson(ISender sender, CreateLessonCommand command)
    {
        return await sender.Send(command);
    }
}
