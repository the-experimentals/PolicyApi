using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolicyApi.DataModels
{
    public class Permissions
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string CODE { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string DISPLAY_NAME { get; set; }
        public string DESCRIPTION { get; set; }

        [Required]
        public string PERMISSION_CATEDGORY_ID { get; set; }

        [Required]
        public int POSITION { get; set; }

        [ForeignKey("PERMISSION_CATEDGORY_ID")]
        public virtual PermissionCategories PERMISSION_CATEGORIES { get; set; }
    }
}
