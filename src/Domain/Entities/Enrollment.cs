namespace EduSphere.Domain.Entities;

public class Enrollment : BaseAuditableEntity
{
    public ApplicationUser Student { get; set; } = null!;
    public string? StudentId { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public int CourseId { get; set; }
}
