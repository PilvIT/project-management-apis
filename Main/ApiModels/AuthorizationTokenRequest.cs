namespace Main.ApiModels;

public class AuthorizationTokenRequest
{
    public string Code { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string State { get; set; } = null!;
}
