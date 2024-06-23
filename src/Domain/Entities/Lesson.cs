namespace EduSphere.Domain.Entities;

public class Lesson : BaseAuditableEntity
{
    public required string Title { get; set; }
    public required string Content { get; set; }
    public Course Course { get; set; } = null!;
    public int CourseId { get; set; }
}
