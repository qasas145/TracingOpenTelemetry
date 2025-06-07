using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TracingOpenTelemetry.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        public StudentController(ILogger<StudentController> logger) => _logger = logger;

        public const string TelemetrySource = "MuhApplication";
        [HttpGet("getdata")]
        public async Task<IActionResult> GetData()
        {
            _logger.LogTrace("Starting the activity");

            ActivitySource _source = new ActivitySource(TelemetrySource);

            using (var activity = _source.StartActivity("FirstCallActivity"))
            {
                DateTime d1 = DateTime.Now;
                activity?.SetTag("TimeOfCall", d1.ToString());
                Thread.Sleep(4000);
                DateTime d2 = DateTime.Now;
                activity?.SetTag("TimeAfterCall", d2.ToString());
                activity?.SetTag("Duration", d2 - d1);
            }

            // 
            using (var activity = _source.StartActivity("SecondCallActivity"))
            {
                DateTime d1 = DateTime.Now;
                activity?.SetTag("TimeOfCall", d1.ToString());
                Thread.Sleep(4000);
                DateTime d2 = DateTime.Now;
                activity?.SetTag("TimeAfterCall", d2.ToString());
                activity?.SetTag("Duration", d2 - d1);
            }

            return Ok(new { weather = "Sunny", data = "Some sort of data" });
        }
    }
}
