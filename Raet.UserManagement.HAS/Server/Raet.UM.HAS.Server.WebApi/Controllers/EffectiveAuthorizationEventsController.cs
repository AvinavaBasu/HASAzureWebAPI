using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Core.Application;
using Raet.UM.HAS.Server.WebApi.Adapters;
using VismaRaet.API.AspNetCore.Authorization;

namespace Raet.UM.HAS.Server.WebApi.Controllers
{

    [Route("api/")]
    [Authorization("View HAS permission")]
    public class EffectiveAuthorizationEventsController : Controller
    {
        private readonly IEffectiveAuthorizationLogging _effectiveAuthorizationLogging;

        readonly ILogger _errorLogger;

        public EffectiveAuthorizationEventsController(IEffectiveAuthorizationLogging effectiveAuthorizationLogging, ILoggerFactory loggerFactory)
        {
            _effectiveAuthorizationLogging = effectiveAuthorizationLogging;
            _errorLogger = loggerFactory.CreateLogger("PushEffectiveAuthorization");
        }

        [HttpPost("[controller]/granted")]
        public async Task<IActionResult> LogEffectiveAutorizationGrantedEvent([FromBody] EffectiveAuthorizationGrantedEvent eAEvent)
        {
            var innerEvent = DomainAdapter.MapEvent(eAEvent);

            var id = await _effectiveAuthorizationLogging.AddAuthLogAsync(innerEvent);
            eAEvent.SetId(id);

            return Created("EffectiveAuthorizationGranted", eAEvent);

        }

        [HttpPost("[controller]/revoked")]
        public async Task<IActionResult> LogEffectiveAutorizationRevokedEvent([FromBody] EffectiveAuthorizationRevokedEvent eAEvent)
        {
            var innerEvent = DomainAdapter.MapEvent(eAEvent);

            var id = await _effectiveAuthorizationLogging.AddAuthLogAsync(innerEvent);
            eAEvent.SetId(id);

            return Created("EffectiveAuthorizationRevoked", eAEvent);

        }
                
    }
}
