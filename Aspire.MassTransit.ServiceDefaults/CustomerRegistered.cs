namespace Microsoft.Extensions.Hosting;

public record CustomerRegistered(string CustomerName, DateTime RegisteredAt);

public record CustomerSignedUp(Guid CustomerId, string CustomerName, DateTime RegisteredAt, DateTime RegistrationComplete);

public record CustomerProvisioned(Guid CustomerId, string CustomerName, string DatabaseName, DateTime ProvisionedAt);

public static class MyObservability
{
    public const string ActivitySourceName = "Aspire.MassTransit.Test";
}