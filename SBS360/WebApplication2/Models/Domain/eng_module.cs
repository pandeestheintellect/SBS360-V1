//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Eng360Web.Models.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class eng_module
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public eng_module()
        {
            this.eng_permission = new HashSet<eng_permission>();
            this.eng_screens = new HashSet<eng_screens>();
        }
    
        public int ModuleID { get; set; }
        public int AccessType { get; set; }
        public string ModuleName { get; set; }
        public int Order_By { get; set; }
        public string Icon { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_permission> eng_permission { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_screens> eng_screens { get; set; }
    }
}