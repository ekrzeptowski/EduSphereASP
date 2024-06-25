using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Lesson.Commands.CreateLesson;
using EduSphere.Application.Features.Lesson.Commands.DeleteLesson;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Lessons.Commands;

using static Testing;

public class DeleteLessonsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDeleteLesson()
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

        var command = new DeleteLessonCommand(lessonId);

        // Act
        await SendAsync(command);

        // Assert
        var lesson = await FindAsync<Lesson>(lessonId);

        lesson.Should().BeNull();
    }

    [Test]
    public async Task ShouldNotDeleteLessonIfNotAuthorized()
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

        var command = new DeleteLessonCommand(lessonId);

        // Act
        Func<Task> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}
