using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using EduSphere.Application.Features.Identity.Accounts.Commands;

namespace Application.IntegrationTests.Identity.Commands;

using static Testing;

public class LoginRequestCommandsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldLoginUser()
    {
        var studentId = await RunAsStudentAsync();
        // Arrange
        var command = new LoginRequestCommand("student@local", "Student1234!");

        // Act
        var token = await SendAsync(command);

        // Assert
        token.Should().NotBeNullOrEmpty();

        // decode jwt token and check claims
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        jsonToken.Should().NotBeNull();
        jsonToken?.Claims.Should().ContainSingle(c => c.Type == "sub" && c.Value == studentId);
    }

    [Test]
    public async Task ShouldNotLoginUserWithInvalidPassword()
    {
        await RunAsStudentAsync();
        // Arrange
        var command = new LoginRequestCommand("student@local", "InvalidPassword");

        // Act
        Func<Task<string>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialException>();
    }
    
    [Test]
    public async Task ShouldNotLoginUserWithInvalidUserName()
    {
        await RunAsStudentAsync();
        // Arrange
        var command = new LoginRequestCommand("InvalidUserName", "Student1234!");

        // Act
        Func<Task<string>> act = async () => await SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialException>();
    }
}
