using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Enrollments.Commands;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Enrollments.Commands;

using static Testing;

public class DeleteEnrollmentsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteEnrollment()
    {
        // Arrange
        await RunAsTeacherAsync();

        var course = new Course { Title = "Test Course", Description = "Test Description" };

        await AddAsync(course);

        var studentId = await RunAsStudentAsync();

        var command = new CreateEnrollmentCommand { CourseId = course.Id, StudentId = studentId };
        var enrollmentId = await SendAsync(command);

        // Act
        await SendAsync(new DeleteEnrollmentCommand { Id = enrollmentId, StudentId = studentId });

        // Assert
        var enrollment = await FindAsync<Enrollment>(enrollmentId);
        enrollment.Should().BeNull();
    }

    [Test]
    public async Task ShouldNotDeleteEnrollmentIfNotAuthorized()
    {
        // Arrange
        var studentId = await RunAsStudentAsync();

        var course = new Course { Title = "Test Course", Description = "Test Description" };

        await AddAsync(course);

        var command = new CreateEnrollmentCommand { CourseId = course.Id, StudentId = studentId };
        var enrollmentId = await SendAsync(command);

        await RunAsDefaultUserAsync();

        // Act
        Func<Task> act = async () => await SendAsync(new DeleteEnrollmentCommand { Id = enrollmentId });

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
