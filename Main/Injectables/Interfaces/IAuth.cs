using Core.Features.GitHub.ViewModels;
using Core.Models;

namespace Main.Injectables.Interfaces;

public interface IAuth
{
    AppUser User { get; }
    GitHubTokenResponse GitHubTokenResponse { get; }
}
