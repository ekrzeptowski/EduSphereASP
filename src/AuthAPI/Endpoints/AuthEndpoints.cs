using EduSphere.Application.Features.Identity.Accounts.Commands;

namespace AuthAPI.Endpoints;

public class AuthEndpoints : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup("/api/auth")
            .WithTags(this.GetType().Name)
            .AllowAnonymous()
            .MapPost(Login, "/login")
            .MapPost(Register, "/register");
    }

    public async Task<string> Login(ISender sender, LoginRequestCommand command)
    {
        return await sender.Send(command);
    }
    
    public async Task<string> Register(ISender sender, RegisterRequestCommand command)
    {
        return await sender.Send(command);
    }
}
