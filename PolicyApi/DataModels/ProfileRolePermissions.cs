using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyApi.DataModels
{
    public class ProfileRolePermissions
    {
        [Key]
        public string ID { get; set; }

        [Required]
        public string PROFILE_ROLE_ID { get; set; }

        [Required]
        public string PERMISSION_ID { get; set; }

        [ForeignKey("PROFILE_ROLE_ID")]
        public virtual ProfileRoles PROFILE_ROLES { get; set; }

        [ForeignKey("PERMISSION_ID")]
        public virtual Permissions PERMISSIONS { get; set; }
    }
}
