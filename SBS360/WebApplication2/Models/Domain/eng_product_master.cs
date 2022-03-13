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
    
    public partial class eng_product_master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public eng_product_master()
        {
            this.eng_mm_inwdesc = new HashSet<eng_mm_inwdesc>();
            this.eng_mm_outdesc = new HashSet<eng_mm_outdesc>();
            this.eng_mm_stockadj_master = new HashSet<eng_mm_stockadj_master>();
            this.eng_mm_trmaster = new HashSet<eng_mm_trmaster>();
        }
    
        public int ProductID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Type { get; set; }
        public string Product_Company_Name { get; set; }
        public string Product_Description { get; set; }
        public string Dimension { get; set; }
        public string Measuring_Unit { get; set; }
        public Nullable<decimal> Unit_Price { get; set; }
        public string Product_Code { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string Barcode1 { get; set; }
        public string Barcode2 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_mm_inwdesc> eng_mm_inwdesc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_mm_outdesc> eng_mm_outdesc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_mm_stockadj_master> eng_mm_stockadj_master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eng_mm_trmaster> eng_mm_trmaster { get; set; }
    }
}