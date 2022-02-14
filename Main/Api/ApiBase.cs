using Core.Features.GitHubApp.ApiModels;
using Core.Features.Users.Models;
using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[ApiController]
public class ApiBase
{
    private IAuth Auth { get; }
    protected AppUser User => Auth.User;
    protected GitHubTokens GitHubTokens => Auth.GitHubTokens;

    protected ApiBase(IAuth auth)
    {
        Auth = auth;
    }
}