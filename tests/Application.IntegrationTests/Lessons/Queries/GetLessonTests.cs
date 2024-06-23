using EduSphere.Application.Features.Course.Queries;
using EduSphere.Application.Features.Lesson.Queries;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Lessons.Queries;

using static Testing;

public class GetLessonTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnLessonById()
    {
        var studentId = await RunAsStudentAsync();
        // Arrange
        var course = new Course
        {
            Title = "Test Course",
            Description = "Test Description",
            Lessons = new List<Lesson> { new Lesson { Title = "Test Lesson", Content = "Test Content" } },
            Enrollments = new List<Enrollment>()
        };

        await AddAsync(course);

        var courses = await SendAsync(new GetCoursesWithPaginationQuery());

        var addedCourse = courses.Items.FirstOrDefault();


        if (addedCourse != null && addedCourse.Id != null)
        {
            await AddAsync(new Enrollment { CourseId = (int)addedCourse.Id, StudentId = studentId });

            var query = new GetLessonQuery((int)course.Lessons.FirstOrDefault()?.Id!);

            // Act
            var lesson = await SendAsync(query);

            // Assert
            lesson.Should().NotBeNull();
            lesson.Title.Should().Be("Test Lesson");
            lesson.Content.Should().Be("Test Content");
        }
        else
        {
            Assert.Fail("Lesson not found");
        }
    }
}
