using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public interface ISupplierServices
    {
       
        int CreateSupplier(SupplierViewModel supplier);
        int SaveSupplier(SupplierViewModel supplier);
        int DeleteSupplier(int supplierID);
        SupplierViewModel getSupplier(int supplierID);
        List<SupplierViewModel> getAllSuppliers();
    }
}