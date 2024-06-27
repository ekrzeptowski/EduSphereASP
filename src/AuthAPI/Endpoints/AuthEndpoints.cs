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
            .MapPost(Register, "/register")
            .MapPost(TeacherRegister, "/register-teacher");
    }

    public async Task<string> Login(ISender sender, LoginRequestCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<string> Register(ISender sender, TeacherRegisterRequestCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<string> TeacherRegister(ISender sender, TeacherRegisterRequestCommand command)
    {
        return await sender.Send(command);
    }
}
