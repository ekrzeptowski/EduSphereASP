using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Course.Commands;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Courses.Commands;

using static Testing;

public class CreateCourseTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateCourse()
    {
        // Arrange
        await RunAsTeacherAsync();

        var command = new CreateCourseCommand { Title = "Test Course", Description = "Test Description" };

        // Act
        var courseId = await SendAsync(command);

        // Assert
        var course = await FindAsync<Course>(courseId);

        course.Should().NotBeNull();
        course?.Title.Should().Be(command.Title);
        course?.Description.Should().Be(command.Description);
    }

    [Test]
    public async Task ShouldNotCreateCourseIfNotAuthorized()
    {
        // Arrange
        await RunAsStudentAsync();

        var command = new CreateCourseCommand { Title = "Test Course", Description = "Test Description" };

        // Act
        Func<Task<int>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
