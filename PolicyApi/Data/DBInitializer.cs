using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PolicyApi.DataModels;
using PolicyApi.Policy;
using PolicyApi.Services.SQLServer;

namespace PolicyApi.Data
{
    public class DBInitializer
    {
        private readonly PolicyStore _store;
        private readonly IPolicyManager _policyManager;
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        public const string ADMIN_GUID = "af8f3217-02b2-4cc2-b536-74f99def2a39";

        public DBInitializer(PolicyStore store, IPolicyManager policyManager)
        {
            _store = store;
            _policyManager = policyManager;
        }

        public void Initialize()
        {
            SeedRoles();
            SeedPermissionCategories();
            SeedPermissions();
            SeedAdminPolicy();
        }

        /// <summary>
        /// Initial seed all or missing roles
        /// </summary>
        public void SeedRoles()
        {
            var roles = _store.ROLES.ToList();
            var allRoles = typeof(Identifiers.Roles).GetFields(bindingFlags);

            Roles newRole = null;
            if (!roles.Any())
            {
                foreach (FieldInfo role in allRoles)
                {
                    newRole = new Roles()
                    {
                        ID = Guid.NewGuid().ToString(),
                        CODE = role.Name,
                        NAME = (string)role.GetValue(newRole)
                    };
                    _store.ROLES.Add(newRole);
                }
                _store.SaveChanges();
            }
            else
            {
                foreach (FieldInfo role in allRoles)
                {
                    if (!roles.Any(ct => ct.CODE.Equals(role.Name)))
                    {
                        // seed this role
                        newRole = new Roles()
                        {
                            ID = Guid.NewGuid().ToString(),
                            CODE = role.Name,
                            NAME = (string)role.GetValue(newRole)
                        };

                        _store.ROLES.Add(newRole);
                    }
                }
                _store.SaveChanges();
            }
        }

        private void SeedPermissionCategories()
        {
            var permissionCategories = _store.PERMISSION_CATEGORIES.ToList();
            var allpermissionCategories = typeof(Identifiers.PermissionCategory).GetFields(bindingFlags);

            PermissionCategories newPermissionCategory = null;

            if (permissionCategories.Count == 0)
            {
                foreach (FieldInfo permissionCategory in allpermissionCategories)
                {
                    var permissionCategoryValue = (Identifiers.IdentifierModel)permissionCategory.GetValue(newPermissionCategory);
                    newPermissionCategory = new PermissionCategories()
                    {
                        ID = Guid.NewGuid().ToString(),
                        CODE = permissionCategory.Name,
                        NAME = permissionCategoryValue.NAME,
                        DISPLAY_NAME = permissionCategoryValue.DISPLAY_NAME,
                        POSITION = permissionCategoryValue.POSITION
                    };
                    _store.Add(newPermissionCategory);
                }
                _store.SaveChanges();
            }
            else
            {
                foreach (FieldInfo permissionCategory in allpermissionCategories)
                {
                    if (!permissionCategories.Any(ct => ct.CODE.Equals(permissionCategory.Name)))
                    {
                        var permissionCategoryValue = (Identifiers.IdentifierModel)permissionCategory.GetValue(newPermissionCategory);
                        newPermissionCategory = new PermissionCategories()
                        {
                            ID = Guid.NewGuid().ToString(),
                            CODE = permissionCategory.Name,
                            NAME = permissionCategoryValue.NAME,
                            DISPLAY_NAME = permissionCategoryValue.DISPLAY_NAME,
                            POSITION = permissionCategoryValue.POSITION
                        };

                        _store.Add(newPermissionCategory);
                    }
                }
                _store.SaveChanges();
            }
        }

        /// <summary>
        /// Initial seed all or missing permissions
        /// </summary>
        private void SeedPermissions()
        {
            var permissions = _store.PERMISSIONS.ToList();
            var allPermission = typeof(Identifiers.Permissions).GetFields(bindingFlags);
            var allPermissionCategories = _store.PERMISSION_CATEGORIES.ToList();

            Permissions newPermission = null;
            if (permissions == null) // seed all permissions
            {
                foreach (FieldInfo permission in allPermission)
                {
                    var permissionAvailable = (Identifiers.IdentifierModel)permission.GetValue(newPermission);
                    var permissionCategory = allPermissionCategories.Where(pc => pc.CODE.Equals(permissionAvailable.CATEGORY)).FirstOrDefault();
                    newPermission = new Permissions()
                    {
                        ID = Guid.NewGuid().ToString(),
                        CODE = permission.Name,
                        NAME = permissionAvailable.NAME,
                        DISPLAY_NAME = permissionAvailable.DISPLAY_NAME,
                        DESCRIPTION = permissionAvailable.DESCRIPTION,
                        POSITION = permissionAvailable.POSITION,
                        PERMISSION_CATEDGORY_ID = permissionCategory.ID
                    };
                    _store.PERMISSIONS.Add(newPermission);
                }
                _store.SaveChanges();
            }
            else
            {
                foreach (FieldInfo permission in allPermission)
                {
                    if (!permissions.Any(ct => ct.CODE.Equals(permission.Name)))
                    {
                        var permissionAvailable = (Identifiers.IdentifierModel)permission.GetValue(newPermission);
                        var permissionCategory = allPermissionCategories.Where(pc => pc.CODE.Equals(permissionAvailable.CATEGORY)).FirstOrDefault();
                        newPermission = new Permissions()
                        {
                            ID = Guid.NewGuid().ToString(),
                            CODE = permission.Name,
                            NAME = permissionAvailable.NAME,
                            DISPLAY_NAME = permissionAvailable.DISPLAY_NAME,
                            DESCRIPTION = permissionAvailable.DESCRIPTION,
                            POSITION = permissionAvailable.POSITION,
                            PERMISSION_CATEDGORY_ID = permissionCategory.ID
                        };

                        _store.PERMISSIONS.Add(newPermission);
                    }
                }
                _store.SaveChanges();
            }
        }

        private void SeedAdminPolicy()
        {
            Roles adminRole = _store.ROLES.Where(x => x.CODE.Equals("ADMIN")).FirstOrDefault();
            // add admin profile role
            string profileRoleID = _policyManager.AssignRole(adminRole, ADMIN_GUID);
            if(!string.IsNullOrWhiteSpace(profileRoleID))
            {
                List<Identifiers.IdentifierModel> permissions = new List<Identifiers.IdentifierModel>();
                permissions.Add(Identifiers.Permissions.ADMIN_PERMISSION);
                _policyManager.AssignPermissions(permissions, profileRoleID);
            }
            
        }
    }
}
