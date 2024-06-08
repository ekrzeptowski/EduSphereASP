using System.Security.Authentication;
using EduSphere.Application.Common.Interfaces;

namespace EduSphere.Application.Features.Identity.Accounts.Commands;

public record LoginRequestCommand(string UserName, string Password) : IRequest<string>;

internal sealed class LoginRequestCommandHandler : IRequestHandler<LoginRequestCommand, string>
{
    private readonly IAuthAccountService _authAccountService;

    public LoginRequestCommandHandler(IAuthAccountService authAccountService)
    {
        _authAccountService = authAccountService;
    }

    public async Task<string> Handle(LoginRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await _authAccountService.Login(request.UserName, request.Password);
        if (!result.Result.Succeeded)
        {
            throw new InvalidCredentialException();
        }

        return result.Token;
    }
}
