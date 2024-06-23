using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Enrollments.Commands;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Enrollments.Commands;

using static Testing;

public class CreateEnrollmentsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateEnrollment()
    {
        // Arrange
        await RunAsTeacherAsync();

        var course = new Course { Title = "Test Course", Description = "Test Description" };

        await AddAsync(course);

        var studentId = await RunAsStudentAsync();

        var command = new CreateEnrollmentCommand { CourseId = course.Id, StudentId = studentId };
        // Act
        var enrollmentId = await SendAsync(command);

        // Assert
        var enrollment = await FindAsync<Enrollment>(enrollmentId);
        enrollment.Should().NotBeNull();
        enrollment?.CourseId.Should().Be(command.CourseId);
    }

    [Test]
    public async Task ShouldNotCreateEnrollmentIfNotAuthorized()
    {
        // Arrange
        await RunAsTeacherAsync();

        var command = new CreateEnrollmentCommand { CourseId = 1 };

        // Act
        Func<Task<int>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
