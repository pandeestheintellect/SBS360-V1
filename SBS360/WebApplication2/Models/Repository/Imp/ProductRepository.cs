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
    public class ProductRepository : IProductRepository
    {
        Eng360Entities1 Eng360DB = new Eng360Entities1();
         Logger logger = LogManager.GetCurrentClassLogger();

       public int CreateProduct(ProductViewModel product)
        {
            var _db_product = Mapper.Map<eng_product_master>(product);

           Eng360DB.eng_product_master.Add(_db_product);
           return Eng360DB.SaveChanges();
        }

        public int SaveProduct(ProductViewModel product)
        {

            var _db_product = Mapper.Map<eng_product_master>(product);
            Eng360DB.Entry(_db_product).State = EntityState.Modified;

           return Eng360DB.SaveChanges();


        }

        public int DeleteProduct(int productID)
        {
            var _db_product = Eng360DB.eng_product_master.First(a => a.ProductID == productID);

            _db_product.UpdatedBy = AppSession.GetCurrentUserId();
            _db_product.UpdatedDate = DateTime.Now;
            _db_product.IsActive = 0;

            Eng360DB.Entry(_db_product).State = EntityState.Modified;
            return Eng360DB.SaveChanges();
        }

        public ProductViewModel getProduct(int productID)
        {
            eng_product_master eng_product_master = Eng360DB.eng_product_master.Find(productID);

            return Mapper.Map<ProductViewModel>(eng_product_master);
        }

        public List<ProductViewModel> getAllProducts()
        {
            var eng_product_master = Eng360DB.eng_product_master.ToList();
            var lstProductView = Mapper.Map<List<ProductViewModel>>(eng_product_master);
            return lstProductView;
        }

        public List<ProductViewModel> getFilterProducts(FilterViewModel filter)
        {
            var sql = "select * from eng_product_master ";
            var where = "";
            
            if (filter.Product_Name != "Select")
                if (where == "")
                    where += " Product_Name ='" + filter.Product_Name + "'";
                else
                    where += " and Product_Name ='" + filter.Product_Name + "'";

            if (filter.Product_Company_Name != "Select")
                if (where == "")
                    where += " Product_Company_Name ='" + filter.Product_Company_Name + "'";
                else
                    where += " and Product_Company_Name ='" + filter.Product_Company_Name + "'";

            if (filter.Product_Code != "Select")
                if (where == "")
                    where += " Product_Code ='" + filter.Product_Code + "'";
                else
                    where += " and Product_Code ='" + filter.Product_Code + "'";

            if (where != "")
                sql = sql + "Where " + where;

            // DbSqlQuery<eng_employee_profile> data = Eng360DB.eng_employee_profile.SqlQuery(sql);
            var data = Eng360DB.eng_product_master.SqlQuery(sql).ToList();

            var pdtView = Mapper.Map<List<ProductViewModel>>(data);
            return pdtView;
        }

    }

}