using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace LeadGen.Processor.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> logger;

        public SampleController(ILogger<SampleController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            logger.LogInformation("Information World");
            logger.LogWarning("Warning World");
            logger.LogCritical("Critical World");
            logger.LogError("Error World");
            return "Hello, World!";
        }
    }
}