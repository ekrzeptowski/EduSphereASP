namespace EduSphere.Application.Features.Lesson.Queries;

public class LessonDto
{
    public int? Id { get; init; }
    public string? Title { get; init; }
    public string? Content { get; init; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Lesson, LessonDto>();
        }
    }
}
