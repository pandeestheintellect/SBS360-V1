using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class RALikelihoodViewModel
    {
        public int RMLHID { get; set; }
        public Nullable<int> Likelihood_Value { get; set; }
        public string Likelihood_Type { get; set; }
        public string Likelihood_Description { get; set; }

    }
}