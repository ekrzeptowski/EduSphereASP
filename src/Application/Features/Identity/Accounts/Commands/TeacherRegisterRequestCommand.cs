using EduSphere.Application.Common.Exceptions;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Application.Common.Security;
using EduSphere.Domain.Constants;

namespace EduSphere.Application.Features.Identity.Accounts.Commands;

[Authorize(Roles = Roles.Administrator)]
public record TeacherRegisterRequestCommand(string UserName, string Password) : IRequest<string>;

internal sealed class TeacherRegisterRequestCommandHandler : IRequestHandler<TeacherRegisterRequestCommand, string>
{
    private readonly IAuthAccountService _authAccountService;

    public TeacherRegisterRequestCommandHandler(IAuthAccountService authAccountService)
    {
        _authAccountService = authAccountService;
    }

    public async Task<string> Handle(TeacherRegisterRequestCommand request, CancellationToken cancellationToken)
    {
        var result = await _authAccountService.Register(request.UserName, request.Password, Roles.Teacher);
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
