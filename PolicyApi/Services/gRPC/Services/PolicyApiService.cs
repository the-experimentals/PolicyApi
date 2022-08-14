using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using PolicyApi.Policy;
using PolicyApi.Protos;
using static PolicyApi.Protos.PolicyApi;

namespace PolicyApi.Services.gRPC.Services
{
    [Authorize]
    public class PolicyApiService : PolicyApiBase
    {
        private readonly IPolicyManager _policyManager;
        private readonly IMapper _mapper;

        public PolicyApiService(IPolicyManager policyManager, IMapper mapper)
        {
            _policyManager = policyManager;
            _mapper = mapper;
        }

        public override Task<TokenResponse> GetPolicyToken(Empty request, ServerCallContext context)
        {
            ClaimsIdentity userIdentity = context.GetHttpContext().User.Identity as ClaimsIdentity;
            string profileID = userIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            userIdentity.AddClaims(_policyManager.GetProfileRoleClaims(profileID));
            TokenResponse token = new();
            token.ACCESS = _policyManager.GenerateJwtToken(userIdentity);

            return Task.FromResult(_mapper.Map<TokenResponse>(token));
        }
    }
}
