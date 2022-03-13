using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Repository.Imp
{
    public interface IProductRepository
    {
        

        int CreateProduct(ProductViewModel product);
        int SaveProduct(ProductViewModel product);
        int DeleteProduct(int productID);
        ProductViewModel getProduct(int productID);
        List<ProductViewModel> getAllProducts();
        List<ProductViewModel> getFilterProducts(FilterViewModel filter);

    }
}