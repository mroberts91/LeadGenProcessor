using LeadGen.Core.Events;
using LeadGen.Core.Models;
using LeadGen.Core.Store;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadGen.Validator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VerifyController : ControllerBase
    {
        private readonly IEventBus _eventBus;
        private readonly IStateStore _stateStore;

        public VerifyController(IEventBus eventBus, IStateStore stateStore)
        {
            _eventBus = eventBus;
            _stateStore = stateStore;
        }

        [HttpPost]
        public async Task<IActionResult> Verify(ValidatedLead lead)
        {
            var token = await _stateStore.GetValidateLeadTokenAsync(lead);

            return token switch
            {
                null => NotFound(),
                byte[] t when lead.VerifyValidationToken(t) => Ok(),
                _ => Forbid(),
            };
        }
    }
}
