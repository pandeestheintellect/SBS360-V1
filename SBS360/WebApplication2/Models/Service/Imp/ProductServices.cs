using Eng360Web.Models.Repository.Imp;
using Eng360Web.Models.Service.Imp;
using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Interface
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository productRepository;
        public ProductServices(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
        }

        public List<ProductViewModel> getAllProducts()
        {
            return productRepository.getAllProducts();
        }


        public int CreateProduct(ProductViewModel product)
        {
            return productRepository.CreateProduct(product);
        }

        public int SaveProduct(ProductViewModel product)
        {
            return productRepository.SaveProduct(product);
        }
        public int DeleteProduct(int productID)
        {
            return productRepository.DeleteProduct(productID);
        }

        public ProductViewModel getProduct(int productID)
        {

            return productRepository.getProduct(productID);
        }

        public List<ProductViewModel> getFilterProducts(FilterViewModel filter)
        {
            return productRepository.getFilterProducts(filter);
        }
    }
}