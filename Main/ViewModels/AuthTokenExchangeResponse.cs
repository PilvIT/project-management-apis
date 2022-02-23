namespace Main.ViewModels;

public class AuthTokenExchangeResponse
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = null!;
}
