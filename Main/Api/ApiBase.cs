using Core.Features.GitHub.ViewModels;
using Core.Models;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[ApiController]
public class ApiBase
{
    private IAuth Auth { get; }
    protected AppUser User => Auth.User;
    protected GitHubTokenResponse GitHubTokenResponse => Auth.GitHubTokenResponse;

    protected ApiBase(IAuth auth)
    {
        Auth = auth;
    }
}