namespace CoreSecurity;

public class AuthenticationConfig
{
    public const string ConfigKey = "Authentication";

    public GitHubAuthenticationConfig GitHub { get; set; } = new();
    public GoogleAuthenticationConfig Google { get; set; } = new();
}

public class GitHubAuthenticationConfig
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
}

public class GoogleAuthenticationConfig
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
}
