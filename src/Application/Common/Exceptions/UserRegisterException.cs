namespace EduSphere.Application.Common.Exceptions;

public class UserRegisterException : Exception
{
    public UserRegisterException() : base("Wprowadzone dane mają błędy.")
    {
        Errors = [];
    }
    
    public UserRegisterException(string[] errors)
        : this()
    {
        Errors = errors;
    }

    public string[] Errors { get; }
}
