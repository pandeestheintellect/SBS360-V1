using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class SupplierServices : ISupplierServices
    {
        private readonly ISupplierRepository supplierRepository;
        public SupplierServices(ISupplierRepository _supplierRepository)
        {
            supplierRepository = _supplierRepository;
        }

       
        public int CreateSupplier(SupplierViewModel supplier)
        {
            return supplierRepository.CreateSupplier(supplier);
        }

        public int SaveSupplier(SupplierViewModel supplier)
        {
            return supplierRepository.SaveSupplier(supplier);
        }
        public int DeleteSupplier(int supplierID)
        {
            return supplierRepository.DeleteSupplier(supplierID);
        }

        public SupplierViewModel getSupplier(int supplierID)
        {

            return supplierRepository.getSupplier(supplierID);
        }
        public List<SupplierViewModel> getAllSuppliers()
        {
            return supplierRepository.getAllSuppliers();
        }
    }
}