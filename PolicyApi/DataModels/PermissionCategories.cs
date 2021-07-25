using System;
using System.ComponentModel.DataAnnotations;

namespace PolicyApi.DataModels
{
    public class PermissionCategories
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string CODE { get; set; }
        [Required]
        public string DISPLAY_NAME { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public int POSITION { get; set; }
        public string DESCRIPTION { get; set; }
    }
}
