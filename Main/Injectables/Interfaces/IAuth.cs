using Core.Features.GitHubApp.ApiModels;
using Core.Features.Users.Models;

namespace Main.Injectables.Interfaces;

public interface IAuth
{
    AppUser User { get; }
    GitHubTokens GitHubTokens { get; }
}
