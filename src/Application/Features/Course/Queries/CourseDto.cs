using EduSphere.Application.Features.Lesson.Queries;

namespace EduSphere.Application.Features.Course.Queries;

public class CourseDto
{
    public int? Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public ICollection<LessonDto>? Lessons { get; set; }
    public bool IsEnrolled { get; init; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Course, CourseDto>().ForMember(
                dest => dest.IsEnrolled,
                opt => opt.Ignore());
        }
    }
}
