using FrontEnd.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FrontEnd.HealthChecks;

public class BackendHealthCheck : IHealthCheck
{
    private readonly IApiClient _apiClient;

    public BackendHealthCheck(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext contex, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (await _apiClient.CheckHealthAsync())
        {
            return HealthCheckResult.Healthy();
        }

        return HealthCheckResult.Unhealthy();
    }
}
