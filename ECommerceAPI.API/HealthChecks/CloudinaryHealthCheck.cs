using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ECommerceAPI.API.HealthChecks;

public class CloudinaryHealthCheck : IHealthCheck
{
    private readonly Domain.Interfaces.Services.IPictureService _pictureService;
    private readonly ILogger<CloudinaryHealthCheck> _logger;

    public CloudinaryHealthCheck(Domain.Interfaces.Services.IPictureService pictureService, ILogger<CloudinaryHealthCheck> logger)
    {
        _pictureService = pictureService;
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_pictureService is not null)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Cloudinary service is available."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Cloudinary service is not configured."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cloudinary health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy("Cloudinary health check failed.", ex));
        }
    }
}
