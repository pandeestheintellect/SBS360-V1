using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class ClaimDescriptionViewModel
    {
        public int ClaimDescID { get; set; }
        public Nullable<int> ClaimID { get; set; }
        [Display(Name = "Claim Category"), Required]
        public Nullable<int> ClaimTypeID { get; set; }
        [Display(Name = "Receipt Date"), Required]
        public string RecpDate { get; set; }
        [Display(Name = "Claim Description"), Required]
        public string ClaimDescription { get; set; }
        [Display(Name = "Receipt Amount"),
        RegularExpression(@"^(0|-?\d{0,16}(\.\d{0,2})?)$", ErrorMessage = "Invalid Amount"), Required]
        public Nullable<decimal> RecpAmount { get; set; }
        public string GST { get; set; }

        public ClaimTypeViewModel eng_sys_claimtype { get; set; }

    }
}