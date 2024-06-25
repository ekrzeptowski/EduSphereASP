using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Course.Commands;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Courses.Commands;

using static Testing;

public class UpdateCourseTests : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateCourse()
    {
        // Arrange
        await RunAsTeacherAsync();

        var courseId =
            await SendAsync(new CreateCourseCommand { Title = "Test Course", Description = "Test Description" });

        var command = new UpdateCourseCommand
        {
            CourseId = courseId, Title = "Updated Course", Description = "Updated Description"
        };

        // Act
        await SendAsync(command);

        // Assert
        var course = await FindAsync<Course>(courseId);

        course.Should().NotBeNull();
        course?.Title.Should().Be(command.Title);
        course?.Description.Should().Be(command.Description);
    }

    [Test]
    public async Task ShouldNotUpdateCourseIfNotAuthorized()
    {
        // Arrange
        await RunAsTeacherAsync();

        var courseId =
            await SendAsync(new CreateCourseCommand { Title = "Test Course", Description = "Test Description" });

        await RunAsStudentAsync();
        
        var command = new UpdateCourseCommand
        {
            CourseId = courseId, Title = "Updated Course", Description = "Updated Description"
        };

        // Act
        Func<Task> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
