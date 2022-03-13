using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eng360Web.Models.Domain;
using AutoMapper;
using System.Data.Entity;
using Eng360Web.Models.Utility;

namespace Eng360Web.Models.Repository.Interface
{
    public class SupplierRepository : ISupplierRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

       public int CreateSupplier(SupplierViewModel supplier)
        {

            eng_address_master address = new eng_address_master()
            {
                Address1 = supplier.eng_address_master.Address1,
                Address2 = supplier.eng_address_master.Address2,
                City = supplier.eng_address_master.City,
                Country = supplier.eng_address_master.Country,
                Fax1 = supplier.eng_address_master.Fax1,
                Email = supplier.eng_address_master.Email,
                Web = supplier.eng_address_master.Web,
                Tel = supplier.eng_address_master.Tel,
                Mobile = supplier.eng_address_master.Mobile,
                Postal_Code = supplier.eng_address_master.Postal_Code
            };
            supplier.AddressID = address.AddressID;

            var _db_supplier = Mapper.Map<eng_supplier_master>(supplier);

           Eng360DB.eng_supplier_master.Add(_db_supplier);
           return Eng360DB.SaveChanges();
        }

        public int SaveSupplier(SupplierViewModel supplier)
        {

            var _db_supplier = Mapper.Map<eng_supplier_master>(supplier);
            Eng360DB.Entry(_db_supplier).State = EntityState.Modified;

            var domainAddress = Eng360DB.eng_address_master.First(a => a.AddressID == supplier.AddressID);

            Eng360DB.Entry(domainAddress).State = EntityState.Modified;

            return Eng360DB.SaveChanges();


        }

        public int DeleteSupplier(int supplierID)
        {


            var _db_supplier = Eng360DB.eng_supplier_master.First(a => a.SupplierID == supplierID);

            _db_supplier.UpdatedBy = AppSession.GetCurrentUserId();
            _db_supplier.UpdatedDate = DateTime.Now;
            _db_supplier.IsActive = 0;


            Eng360DB.Entry(_db_supplier).State = EntityState.Modified;
            return Eng360DB.SaveChanges();
        }

        public SupplierViewModel getSupplier(int supplierID)
        {
            eng_supplier_master eng_supplier_master = Eng360DB.eng_supplier_master.Find(supplierID);

            return Mapper.Map<SupplierViewModel>(eng_supplier_master);
        }

        public List<SupplierViewModel> getAllSuppliers()
        {
            var eng_supplier_master = Eng360DB.eng_supplier_master.ToList();
            var lstSupplierView = Mapper.Map<List<SupplierViewModel>>(eng_supplier_master);
            return lstSupplierView;
        }
    }
}