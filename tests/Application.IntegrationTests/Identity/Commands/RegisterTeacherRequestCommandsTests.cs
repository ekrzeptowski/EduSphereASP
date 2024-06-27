using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Identity.Accounts.Commands;
using EduSphere.Domain;

namespace Application.IntegrationTests.Identity.Commands;

using static Testing;

public class RegisterTeacherRequestCommandsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRegisterUser()
    {
        // Arrange
        await RunAsAdministratorAsync();
        var command = new TeacherRegisterRequestCommand("teacher@local", "Teacher1234!");

        // Act
        var userId = await SendAsync(command);

        // Assert
        var user = await FindAsync<ApplicationUser>(userId);
        user.Should().NotBeNull();
        user?.Email.Should().Be(command.UserName);
    }

    [Test]
    public async Task ShouldNotRegisterUserWithDuplicateEmail()
    {
        // Arrange
        await RunAsAdministratorAsync();
        await SendAsync(new TeacherRegisterRequestCommand("teacher@local", "Teacher1234!"));

        var command = new TeacherRegisterRequestCommand("teacher@local", "Teacher1234!");

        // Act
        Func<Task<string>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<UserRegisterException>();
    }

    [Test]
    public async Task ShouldNotRegisterUserWithInvalidPassword()
    {
        // Arrange
        await RunAsAdministratorAsync();
        var command = new TeacherRegisterRequestCommand("teacher@local", "invalid");

        // Act
        Func<Task<string>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<UserRegisterException>();
    }
}
