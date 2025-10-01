using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using OrderingServiceWeb.Attributes;
using OrderingServiceWeb.Helpers.Abstractions;
using System.Security.Claims;

namespace OrderingServiceWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationLogController : ControllerBase
    {
        private readonly IApplicationLogManager _applicationLogManager;
        private readonly ISecurityHelper _securityHelper;

        public ApplicationLogController(IApplicationLogManager applicationLogManager, ISecurityHelper securityHelper)
        {
            _applicationLogManager = applicationLogManager;
            _securityHelper = securityHelper;
        }

        [HttpGet("GetLogs")]
        public ActionResult<List<ApplicationLogModel>> GetLogs([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null, [FromQuery] long? customerID = null, [FromQuery][ApplicationLogEvent] string? logEvent = null)
        {
            long retrieverID = _securityHelper.GetCustomerID();

            var logs = _applicationLogManager.GetLogs(retrieverID, startDate, endDate, customerID, logEvent);
            return logs;
        }
    }
}
