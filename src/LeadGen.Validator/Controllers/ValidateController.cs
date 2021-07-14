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
        private readonly IStateStore _stateStore;

        public ValidateController(IEventBus eventBus, IStateStore stateStore)
        {
            _eventBus = eventBus;
            _stateStore = stateStore;
        }

        [HttpPost]
        public async Task<IActionResult> Validate([FromBody] Lead lead)
        {
            return ValidatedLead.Create(lead).Value switch
            {
                ValidatedLead validatedLead             => await ValidatedLeadAsync(validatedLead),
                InvalidLeadPropertyException<string> ex => await RejectedLeadAsync(ex),
                ArgumentNullException ex                => await RejectedLeadAsync(ex),
                _                                       => StatusCode((int)HttpStatusCode.InternalServerError)
            };
        }

        private async Task<IActionResult> ValidatedLeadAsync(ValidatedLead validatedLead)
        {
            LeadValidated evt = new() { Lead = validatedLead };

            await Task.WhenAll(_stateStore.SaveValidatedLeadTokenAsync(validatedLead), _eventBus.PublishAsync(evt));

            return Ok(LeadValidationResult.CreateSuccessResult(validatedLead));
        }

        private async Task<IActionResult> RejectedLeadAsync(InvalidLeadPropertyException<string> exception)
        {
            return await RejectedLeadAsync(LeadValidationResult.CreateFailedResult(exception));
        }

        private async Task<IActionResult> RejectedLeadAsync(ArgumentNullException exception)
        {
            return await RejectedLeadAsync(LeadValidationResult.CreateFailedResult(exception.Message));
        }

        private async Task<IActionResult> RejectedLeadAsync(LeadValidationResult result)
        {
            LeadRejected evt = new()
            {
                ValidationResult = result,
                RejectedUtc = DateTime.UtcNow
            };

            await _eventBus.PublishAsync(evt);

            return Ok(result);
        }
    }
}
