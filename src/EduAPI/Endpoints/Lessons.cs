using EduSphere.Application.Features.Lesson.Commands.CreateLesson;
using EduSphere.Application.Features.Lesson.Commands.DeleteLesson;
using EduSphere.Application.Features.Lesson.Commands.UpdateLesson;
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
            .MapDelete(DeleteLesson, "{lessonId}")
            .MapPut(UpdateLesson, "{lessonId}")
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

    public async Task<IResult> DeleteLesson(ISender sender, int lessonId)
    {
        await sender.Send(new DeleteLessonCommand(lessonId));
        return Results.NoContent();
    }

    public async Task<IResult> UpdateLesson(ISender sender, UpdateLessonCommand command, int lessonId)
    {
        await sender.Send(command);
        return Results.NoContent();
    }
}
