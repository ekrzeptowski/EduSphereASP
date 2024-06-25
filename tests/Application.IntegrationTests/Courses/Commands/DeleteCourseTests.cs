using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Course.Commands;
using EduSphere.Application.Features.Enrollments.Commands;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Courses.Commands;

using static Testing;

public class DeleteCourseTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteCourse()
    {
        // Arrange
        await RunAsTeacherAsync();

        var courseId =
            await SendAsync(new CreateCourseCommand { Title = "Test Course", Description = "Test Description" });

        var command = new DeleteCourseCommand(courseId);

        // Act
        await SendAsync(command);

        // Assert
        var course = await FindAsync<Course>(courseId);

        course.Should().BeNull();
    }

    [Test]
    public async Task ShouldNotDeleteCourseIfNotAuthorized()
    {
        // Arrange
        await RunAsTeacherAsync();

        var courseId =
            await SendAsync(new CreateCourseCommand { Title = "Test Course", Description = "Test Description" });

        await RunAsStudentAsync();
        var command = new DeleteCourseCommand(courseId);

        // Act
        Func<Task> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Test]
    public async Task ShouldNotDeleteCourseIfCourseNotFound()
    {
        // Arrange
        await RunAsTeacherAsync();

        var courseId = 999;

        var command = new DeleteCourseCommand(courseId);

        // Act
        Func<Task> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldNotDeleteCourseIfThereAreEnrollments()
    {
        var studentId = await RunAsStudentAsync();

        // Arrange
        var teacherId = await RunAsTeacherAsync();

        var course = new Course { Title = "Test Course", Description = "Test Description" };
        await AddAsync(course);
        var enrollment = new Enrollment { CourseId = course.Id, StudentId = studentId };
        await AddAsync(enrollment);

        var command = new DeleteCourseCommand(course.Id);

        // Act
        Func<Task> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }
}
