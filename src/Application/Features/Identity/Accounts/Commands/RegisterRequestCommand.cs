using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Domain.Constants;

namespace EduSphere.Application.Features.Identity.Accounts.Commands;

public record RegisterRequestCommand(string UserName, string Password) : IRequest<string>;

internal sealed class RegisterRequestCommandHandler : IRequestHandler<RegisterRequestCommand, string>
{
    private readonly IAuthAccountService _authAccountService;

    public RegisterRequestCommandHandler(IAuthAccountService authAccountService)
    {
        _authAccountService = authAccountService;
    }

    public async Task<string> Handle(RegisterRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await _authAccountService.Register(request.UserName, request.Password, Roles.Student);
        if (!result.Succeeded)
        {
            if (result.Errors.Any())
            {
                throw new UserRegisterException(result.Errors);
            }
        }

        return result.Data;
    }
}
