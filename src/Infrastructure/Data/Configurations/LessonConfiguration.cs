using EduSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduSphere.Infrastructure.Data.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Content)
            .HasMaxLength(2000)
            .IsRequired();

        builder.HasOne(t => t.Course)
            .WithMany(c => c.Lessons)
            .HasForeignKey(t => t.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
