namespace EduSphere.Domain.Entities;

public class Course : BaseAuditableEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IList<Lesson> Lessons { get; set; } = new List<Lesson>();
    public IList<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual bool IsStudentEnrolled(string studentId)
    {
        return Enrollments.Any(e => e.Student.Id == studentId && e.Course.Id == Id);
    }
}
