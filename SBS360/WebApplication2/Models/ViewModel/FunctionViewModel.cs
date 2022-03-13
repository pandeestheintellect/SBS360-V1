using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.ViewModel
{
    public class FunctionViewModel
    {
        public int Id { get; set; }
        public string Fn_code { get; set; }
        public string Fn_Title { get; set; }
        public Nullable<int> Fn_Value { get; set; }
    }
}