using EduSphere.Application.Common.Behaviours;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Features.Course.Commands;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace EduSphere.Application.UnitTests.Common.Behaviours;

public class RequestLoggerTests
{
    private Mock<ILogger<CreateCourseCommand>> _logger = null!;
    private Mock<IUser> _user = null!;
    private Mock<IIdentityService> _identityService = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateCourseCommand>>();
        _user = new Mock<IUser>();
        _identityService = new Mock<IIdentityService>();
    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {
        _user.Setup(x => x.Id).Returns(Guid.NewGuid().ToString());

        var requestLogger =
            new LoggingBehaviour<CreateCourseCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(
            new CreateCourseCommand { Title = "Test course", Description = "Test course description" },
            new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger =
            new LoggingBehaviour<CreateCourseCommand>(_logger.Object, _user.Object, _identityService.Object);

        await requestLogger.Process(
            new CreateCourseCommand { Title = "Test course", Description = "Test course description" },
            new CancellationToken());

        _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Never);
    }
}
