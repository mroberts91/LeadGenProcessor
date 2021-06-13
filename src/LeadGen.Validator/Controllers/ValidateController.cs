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
        private readonly IStateStore _stateStore;

        public ValidateController(IStateStore stateStore)
        {
            _stateStore = stateStore;
        }

        [HttpPost]
        public async Task<IActionResult> Validate(Lead lead)
        {
            return ValidatedLead.Create(lead).Value switch
            {
                ValidatedLead validatedLead => await ValidatedLeadAsync(validatedLead),
                InvalidLeadPropertyException<string> ex => BadRequest(new { ex.PropertyName, ex.Message }),
                ArgumentNullException ex => BadRequest(new { ex.ParamName, ex.Message }),
                _ => StatusCode((int)HttpStatusCode.InternalServerError)
            };
        }

        private async Task<IActionResult> ValidatedLeadAsync(ValidatedLead validatedLead)
        {
            await _stateStore.SaveValidatedLeadTokenAsync(validatedLead);
            return Ok(validatedLead);
        }
    }
}
