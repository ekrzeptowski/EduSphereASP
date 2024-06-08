using EduSphere.Application.Features.Identity.Accounts.Commands;

namespace EduSphere.Web.Endpoints;

public class AuthEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("api/auth")
            .AllowAnonymous()
            .MapPost(Login);
    }

    public async Task<string> Login(ISender sender, LoginRequestCommand command)
    {
        return await sender.Send(command);
    }
}
