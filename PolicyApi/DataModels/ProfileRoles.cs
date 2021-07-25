using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyApi.DataModels
{
    public class ProfileRoles
    {
        [Key]
        public string ID { get; set; }

        [Required]
        public string PROFILE_ID { get; set; }

        [Required]
        public string ROLE_ID { get; set; }

        [ForeignKey("ROLE_ID")]
        public virtual Roles ROLES { get; set; }
    }
}
