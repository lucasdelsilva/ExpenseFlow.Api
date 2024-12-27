using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ExpenseFlow.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;
    private readonly ILogger<HealthController> _logger;

    public HealthController(HealthCheckService healthCheckService, ILogger<HealthController> logger)
    {
        _healthCheckService = healthCheckService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var report = await _healthCheckService.CheckHealthAsync();

        var response = new
        {
            Status = report.Status.ToString(),
            Duration = report.TotalDuration,
            Info = report.Entries.Select(x => new
            {
                Name = x.Key,
                Status = x.Value.ToString(),
                x.Value.Duration,
                Error = x.Value.Exception?.Message,
                x.Value.Data
            })
        };

        var result = report.Status == HealthStatus.Healthy ? Ok(response) : StatusCode(503, response);

        return result;
    }

    [HttpGet("database")]
    public async Task<IActionResult> GetDatabaseHealth()
    {
        var report = await _healthCheckService.CheckHealthAsync(
            c => c.Tags.Contains("database"));

        return Ok(new
        {
            Status = report.Status.ToString(),
            Duration = report.TotalDuration
        });
    }

    [HttpGet("external")]
    public async Task<IActionResult> GetExternalServicesHealth()
    {
        var report = await _healthCheckService.CheckHealthAsync(
            c => c.Tags.Contains("external"));

        return Ok(new
        {
            Status = report.Status.ToString(),
            Duration = report.TotalDuration
        });
    }
}
