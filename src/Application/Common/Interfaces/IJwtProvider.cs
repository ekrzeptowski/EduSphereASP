namespace EduSphere.Application.Common.Interfaces;
public interface IJwtProvider
{
    Task<string> GenerateJwtAsync(string userId);
}
