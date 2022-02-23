namespace Main.ApiModels;

public class AuthorizationTokenResponse
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
}