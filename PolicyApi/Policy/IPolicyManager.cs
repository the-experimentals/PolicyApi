using System;
using System.Collections.Generic;
using System.Security.Claims;
using PolicyApi.DataModels;

namespace PolicyApi.Policy
{
    public interface IPolicyManager
    {
        public string AssignRole(Roles role, string profileID);
        public void AssignPermissions(List<Identifiers.IdentifierModel> permissions, string profileRoleID);
        public List<Claim> GetProfileRoleClaims(string profileID);
        public string GenerateJwtToken(ClaimsIdentity claimsIdentity);
    }
}
