namespace Main.JsonModels;

public class AuthorizationTokenRequest
{
    public string Code { get; set; }
    public string RedirectUri { get; set; }
    public string State { get; set; }
}
