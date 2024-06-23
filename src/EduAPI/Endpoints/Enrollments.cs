using EduSphere.Application.Features.Enrollments.Commands;
using EduSphere.Application.Features.Enrollments.Queries;

namespace EduAPI.Endpoints;

public class Enrollments : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("/api/enrollment")
            .WithTags(this.GetType().Name)
            .RequireAuthorization()
            .MapGet(GetEnrollments)
            .MapDelete(DeleteEnrollment, "{enrollmentId}")
            .MapPost(CreateEnrollment);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetEnrollments(ISender sender)
    {
        return await sender.Send(new GetEnrollmentsQuery());
    }

    public async Task<int> CreateEnrollment(ISender sender, CreateEnrollmentCommand createEnrollmentCommand)
    {
        return await sender.Send(createEnrollmentCommand);
    }

    public async Task DeleteEnrollment(ISender sender, int enrollmentId)
    {
        await sender.Send(new DeleteEnrollmentCommand { Id = enrollmentId });
    }
}
