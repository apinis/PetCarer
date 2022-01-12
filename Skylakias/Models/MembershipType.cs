using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skylakias.Models
{
    public class MembershipType
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int SignUpFee { get; set; }
        public int DurationInMonths { get; set; }
        public int DiscountRate { get; set; }
    }
}