using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PolicyApi.Policy;
using PolicyApi.ResponseModel;

namespace PolicyApi.Controllers
{
    [Authorize]
    [Route("api/policy")]
    public class PolicyController : Controller
    {
        private readonly PolicyManager _policyManager;

        public PolicyController(PolicyManager policyManager)
        {
            _policyManager = policyManager;
        }

        [HttpGet("get-policy-token")]
        public IActionResult GetPolicyToken()
        {
            ClaimsIdentity userIdentity = HttpContext.User.Identity as ClaimsIdentity;
            string profileID = userIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            userIdentity.AddClaims(_policyManager.GetProfileRoleClaims(profileID));
            TokenResponse token = new TokenResponse();
            token.ACCESS = _policyManager.GenerateJwtToken(userIdentity);

            return Ok(JsonConvert.SerializeObject(token));
        }
    }
}
