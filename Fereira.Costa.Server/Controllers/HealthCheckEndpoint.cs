using FastEndpoints;

namespace Fereira.Costa.Server.Controllers;
public class HealthCheckEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
    }


    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new { Status = "API is running" }, cancellation: ct);
    }
}