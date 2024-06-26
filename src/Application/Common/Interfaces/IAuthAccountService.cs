﻿using EduSphere.Application.Common.Models;

namespace EduSphere.Application.Common.Interfaces;
public interface IAuthAccountService
{
    Task<(Result Result, string Token)> Login(string username, string password);
    Task<Result> Register(string email, string password, string role);
    Task<(Result Result, string UserId)> ForgotPassword(string email);
    Task<(Result Result, string UserId)> ResetPassword(string email);
}
