using EduSphere.Domain.Entities;

namespace EduSphere.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Course> Courses { get; }

    DbSet<Enrollment> Enrollments { get; }

    DbSet<Lesson> Lessons { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
