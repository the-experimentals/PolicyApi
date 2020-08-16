using System;
using System.ComponentModel.DataAnnotations;

namespace PolicyApi.DataModels
{
    public class Roles
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string CODE { get; set; }
        [Required]
        public string NAME { get; set; }

        public int? POSITION { get; set; }
    }
}
