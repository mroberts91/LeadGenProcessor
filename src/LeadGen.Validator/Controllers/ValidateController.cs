using LeadGen.Core.Events;
using LeadGen.Core.Events.Leads;
using LeadGen.Core.Exceptions;
using LeadGen.Core.Models;
using LeadGen.Core.Store;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LeadGen.Validator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidateController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public ValidateController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost]
        public async Task<IActionResult> Validate(Lead lead)
        {
            return ValidatedLead.Create(lead).Value switch
            {
                ValidatedLead validatedLead => await ValidatedLeadAsync(validatedLead),
                InvalidLeadPropertyException<string> ex => Ok(LeadValidationResult.CreateFailedResult(ex)),
                ArgumentNullException ex => Ok(LeadValidationResult.CreateFailedResult(ex.Message)),
                _ => StatusCode((int)HttpStatusCode.InternalServerError)
            };
        }

        private async Task<IActionResult> ValidatedLeadAsync(ValidatedLead validatedLead)
        {
            LeadValidated evt = new()
            {
                Lead = validatedLead
            };

            await _eventBus.PublishAsync(evt);
            return Ok(LeadValidationResult.CreateSuccessResult(validatedLead));
        }
    }
}
