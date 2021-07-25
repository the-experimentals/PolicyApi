using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PolicyApi.Data;
using PolicyApi.DataModels;
using PolicyApi.Services.SQLServer;
using PolicyApi.Utilities;
using Token = Microsoft.IdentityModel.Tokens;

namespace PolicyApi.Policy
{
    public class PolicyManager : IPolicyManager
    {
        private readonly PolicyStore _store;
        private readonly JwtSecretKey _jwtSecretKey;
        private readonly TMCache _cache;
        public const string CLAIMS_CACHE_KEY = "CACHE_CLAIMS_";
        public PolicyManager(IOptions<JwtSecretKey> jwtSecretKey, PolicyStore store, TMCache cache)
        {
            _store = store;
            _jwtSecretKey = jwtSecretKey.Value;
            _cache = cache;
        }

        public string AssignRole(Roles role, string profileID)
        {
            // check if role for this profile already exist
            bool hasRole = (from profileRole in _store.PROFILE_ROLES
                           join roles in _store.ROLES
                           on profileRole.ROLE_ID equals roles.ID
                           where roles.CODE == role.CODE select profileRole).Any();
            ProfileRoles newProfileRole = new();

            if (!hasRole)
            {
                Roles storedRole = _store.ROLES.Where(r => r.CODE.Equals(role.CODE)).FirstOrDefault();

                if(storedRole != null)
                {
                    newProfileRole.ID = Guid.NewGuid().ToString();
                    newProfileRole.PROFILE_ID = profileID;
                    newProfileRole.ROLE_ID = storedRole.ID;

                    _store.PROFILE_ROLES.Add(newProfileRole);
                    _store.SaveChanges();
                }
                
            }

            return newProfileRole.ID;
            
        }

        /// <summary>
        /// Assign permissions assigned to user's profile
        /// </summary>
        /// <param name="permissions">Permissions assigned to user's profile</param>
        /// <param name="profileID">User's profile ID</param>
        public void AssignPermissions(List<Identifiers.IdentifierModel> permissions, string profileRoleID)
        {
            var myPermissions = _store.PROFILE_ROLE_PERMISSIONS.Where(x => x.PROFILE_ROLE_ID.Equals(profileRoleID)).ToList();
            var permissionsCodes = permissions.Select(x => x.CODE).ToList();
            var storedPermissions = _store.PERMISSIONS.Where(x => permissionsCodes.Contains(x.CODE)).ToList();

            foreach (var permission in storedPermissions)
            {
                bool hasPermission = myPermissions.Any(x => x.PERMISSION_ID.Equals(permission.ID));
                ProfileRolePermissions profileRolePermissions = new ProfileRolePermissions();
                if(!hasPermission)
                {
                    profileRolePermissions.ID = Guid.NewGuid().ToString();
                    profileRolePermissions.PERMISSION_ID = permission.ID;
                    profileRolePermissions.PROFILE_ROLE_ID = profileRoleID;

                    _store.PROFILE_ROLE_PERMISSIONS.Add(profileRolePermissions);
                    _store.SaveChanges();
                }
            }
        }

        public List<Claim> GetProfileRoleClaims(string profileID)
        {

            List<Claim> claims = _cache.Get<List<Claim>>(CLAIMS_CACHE_KEY + profileID);

            if(claims == null || claims.Count() == 0)
            {
                claims = new();

                var roles = (from pr in _store.PROFILE_ROLES
                             join role in _store.ROLES
                             on pr.ROLE_ID equals role.ID
                             where pr.PROFILE_ID == profileID
                             select new
                             {
                                 dataRole = role,
                                 dataProfileRoleIDs = pr.ID
                             }).ToList();



                foreach (var role in roles.Select(x => x.dataRole))
                {
                    claims.Add(new(ClaimTypes.Role, role.CODE));
                }

                claims.AddRange(GetProfileRolePermissions(roles.Select(x => x.dataProfileRoleIDs).ToList()));

                _cache.Add<List<Claim>>(CLAIMS_CACHE_KEY + profileID, claims);
            }
            

            return claims;
        }

        private List<Claim> GetProfileRolePermissions(List<string> roleIDs)
        {
            List<Claim> claims = new();

            var permissions = (from profileRolePermission in _store.PROFILE_ROLE_PERMISSIONS
                               join permission in _store.PERMISSIONS
                               on profileRolePermission.PERMISSION_ID equals permission.ID
                               where roleIDs.Contains(profileRolePermission.PROFILE_ROLE_ID)
                               select permission).ToList();

            foreach (var permission in permissions)
            {
                claims.Add(new("Permission", permission.CODE));
            }

            return claims;
        }

        public string GenerateJwtToken(ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSecretKey.SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = _jwtSecretKey.ISSUER,
                Audience = _jwtSecretKey.AUDIENCE,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSecretKey.TTL),
                SigningCredentials = new Token.SigningCredentials(new Token.SymmetricSecurityKey(key), Token.SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        
    }
}
