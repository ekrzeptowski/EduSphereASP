namespace EduSphere.Application.Features.Enrollments.Queries;

public class EnrollmentDto
{
    public int Id { get; init; }
    public string StudentId { get; init; } = string.Empty;
    public int CourseId { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Enrollment, EnrollmentDto>();
        }
    }
}
