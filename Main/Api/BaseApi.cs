using Core.Features.GitHub.ViewModels;
using Core.Models;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[ApiController]
public class BaseApi
{
    private IAuthService AuthService { get; }
    protected AppUser User => AuthService.User;
    protected GitHubTokenDetail GitHubTokenDetail => AuthService.GitHubTokenDetail;

    protected BaseApi(IAuthService authService)
    {
        AuthService = authService;
    }
}