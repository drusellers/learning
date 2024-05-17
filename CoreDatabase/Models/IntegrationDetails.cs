namespace CoreDatabase.Models;

public class Integration
{
    public Guid Id { get; set; }
    public IntegrationDetails Details { get; set; }
}

public enum IntegrationType
{
    GoogleSso
}

public class IntegrationDetails
{
    public Guid Id { get; set; }
    public IntegrationType IntegrationType { get; set; }
    public IntegrationDetails Details { get; set; }
}

public class GoogleSsoIntegrationDetails : IntegrationDetails
{
    public string Bob { get; set; }
}
