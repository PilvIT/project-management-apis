using Core.Features.GitHub.ViewModels;
using Core.Models;

namespace Main.Injectables.Interfaces;

public interface IAuthService
{
    AppUser User { get; }
    GitHubTokenDetail GitHubTokenDetail { get; }
}
