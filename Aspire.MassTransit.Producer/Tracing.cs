using System.Diagnostics;

namespace Aspire.MassTransit.Producer;

public static class Tracing
{
    public static readonly ActivitySource ActivitySource = new ActivitySource(MyObservability.ActivitySourceName);
}