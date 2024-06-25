using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Lesson.Commands.CreateLesson;
using EduSphere.Application.Features.Lesson.Commands.UpdateLesson;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Lessons.Commands;

using static Testing;

public class UpdateLessonsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldUpdateLesson()
    {
        // Arrange
        await RunAsTeacherAsync();

        var course = new Course { Title = "Test Course", Description = "Test Description" };
        await AddAsync(course);

        var lessonId =
            await SendAsync(new CreateLessonCommand
            {
                Title = "Test Lesson", Content = "Test Description", CourseId = course.Id
            });

        var command = new UpdateLessonCommand
        {
            LessonId = lessonId, Title = "Updated Lesson", Content = "Updated Description"
        };

        // Act
        await SendAsync(command);

        // Assert
        var lesson = await FindAsync<Lesson>(lessonId);

        lesson.Should().NotBeNull();
        lesson?.Title.Should().Be(command.Title);
        lesson?.Content.Should().Be(command.Content);
    }

    [Test]
    public async Task ShouldNotUpdateLessonIfNotAuthorized()
    {
        // Arrange
        await RunAsTeacherAsync();

        var course = new Course { Title = "Test Course", Description = "Test Description" };
        await AddAsync(course);

        var lessonId =
            await SendAsync(new CreateLessonCommand
            {
                Title = "Test Lesson", Content = "Test Description", CourseId = course.Id
            });

        await RunAsStudentAsync();

        var command = new UpdateLessonCommand
        {
            LessonId = lessonId, Title = "Updated Lesson", Content = "Updated Description"
        };

        // Act
        Func<Task> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
