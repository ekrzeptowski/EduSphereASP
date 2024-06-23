using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Features.Identity.Accounts.Commands;
using EduSphere.Domain;

namespace Application.IntegrationTests.Identity.Commands;

using static Testing;

public class RegisterRequestCommandsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRegisterUser()
    {
        // Arrange
        var command = new RegisterRequestCommand("student@local", "Student1234!");

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
        await SendAsync(new RegisterRequestCommand("student@local", "Student1234!"));

        var command = new RegisterRequestCommand("student@local", "Student1234!");

        // Act
        Func<Task<string>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<UserRegisterException>();
    }

    [Test]
    public async Task ShouldNotRegisterUserWithInvalidPassword()
    {
        // Arrange
        var command = new RegisterRequestCommand("student@local", "invalid");

        // Act
        Func<Task<string>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<UserRegisterException>();
    }
}
