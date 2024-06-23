using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Lesson.Commands.CreateLesson;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Lessons.Commands;

using static Testing;

public class CreateLessonsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateLesson()
    {
        // Arrange
        await RunAsTeacherAsync();

        var course = new Course { Title = "Test Course", Description = "Test Description", };

        await AddAsync(course);

        var command = new CreateLessonCommand { Title = "Test Lesson", Content = "Test Content", CourseId = course.Id };
        // Act
        var lessonId = await SendAsync(command);

        // Assert
        var lesson = await FindAsync<Lesson>(lessonId);
        lesson.Should().NotBeNull();
        lesson?.Title.Should().Be(command.Title);
        lesson?.Content.Should().Be(command.Content);
    }

    [Test]
    public async Task ShouldNotCreateLessonIfNotAuthorized()
    {
        // Arrange
        await RunAsStudentAsync();

        var command = new CreateLessonCommand { Title = "Test Lesson", Content = "Test Content" };

        // Act
        Func<Task<int>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
