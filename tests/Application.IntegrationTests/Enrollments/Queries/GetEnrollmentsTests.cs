using EduSphere.Application.Features.Enrollments.Queries;
using EduSphere.Domain.Entities;

namespace Application.IntegrationTests.Enrollments.Queries;

using static Testing;

public class GetEnrollmentsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllEnrollments()
    {
        var studentId = await RunAsStudentAsync();
        // Arrange
        await AddAsync(new Enrollment
        {
            Course = new Course { Title = "Test Course", Description = "Test Course Description" },
            StudentId = studentId
        });

        // Act
        var enrollments = await SendAsync(new GetEnrollmentsQuery());

        // Assert
        enrollments.Should().NotBeEmpty();
    }

    [Test]
    public async Task ShouldReturnEnrollmentById()
    {
        var studentId = await RunAsStudentAsync();
        // Arrange
        await AddAsync(new Enrollment
        {
            Course = new Course { Title = "Test Course", Description = "Test Course Description" },
            StudentId = studentId
        });

        // get id of the enrollment

        var enrollments = await SendAsync(new GetEnrollmentsQuery());
        var addedEnrollment = enrollments.FirstOrDefault();

        if (addedEnrollment != null)
        {
            var query = new GetEnrollmentQuery((int)addedEnrollment.Id!);

            // Act
            var enrollment = await SendAsync(query);

            // Assert
            enrollment.Should().NotBeNull();
        }
        else
        {
            Assert.Fail("Enrollment not found");
        }
    }
}
