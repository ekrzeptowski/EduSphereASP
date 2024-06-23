using EduSphere.Application.Features.Course.Queries;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Courses.Queries;

using static Testing;

public class GetCoursesTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllCourses()
    {
        await RunAsStudentAsync();
        // Arrange
        await AddAsync(new Course
        {
            Title = "Test Course",
            Description = "Test Description",
            Lessons = new List<Lesson>(),
            Enrollments = new List<Enrollment>()
        });

        // Act
        var courses = await SendAsync(new GetCoursesWithPaginationQuery());

        // Assert
        courses.Items.Should().NotBeEmpty();
    }

    [Test]
    public async Task ShouldReturnCourseById()
    {
        await RunAsStudentAsync();
        // Arrange
        await AddAsync(new Course { Title = "Test Course", Description = "Test Description" });

        var courses = await SendAsync(new GetCoursesWithPaginationQuery());
        var addedCourse = courses.Items.FirstOrDefault();

        if (addedCourse != null)
        {
            var query = new GetCourseQuery((int)addedCourse.Id!);

            // Act
            var course = await SendAsync(query);

            // Assert
            course.Should().NotBeNull();
        }
        else
        {
            Assert.Fail("Course not found");
        }
    }

    [Test]
    public async Task ShouldReturnCoursesWithLessons()
    {
        // Arrange
        var userId = await RunAsDefaultUserAsync();

        var course1 = new Course
        {
            Title = "Test Course",
            Description = "Test Description",
            Lessons = new List<Lesson> { new() { Title = "Test Lesson", Content = "Test Content" } },
        };

        var course2 = new Course
        {
            Title = "Test Course 2",
            Description = "Test Description 2",
            Lessons =
                new List<Lesson> { new() { Title = "Test Lesson 2", Content = "Test Content 2" } },
        };

        await AddAsync(course1);
        await AddAsync(course2);

        await AddAsync(new Enrollment { CourseId = course1.Id, StudentId = userId });

        var query = new GetCoursesWithPaginationQuery();

        // Act
        var courses = await SendAsync(query);

        // Assert
        courses.Items.Should().NotBeEmpty();
        courses.Items.Should().HaveCount(2);

        // Enrolled in course 1 should get lessons
        courses.Items.First().Title.Should().Be("Test Course");
        courses.Items.First().Lessons.Should().NotBeEmpty();

        // Not enrolled in course 2 should get lessons with empty content
        courses.Items.Last().Title.Should().Be("Test Course 2");
        courses.Items.Last().Lessons.Should().NotBeEmpty();
        courses.Items.Last().Lessons?.First().Content.Should().BeEmpty();
    }
}
