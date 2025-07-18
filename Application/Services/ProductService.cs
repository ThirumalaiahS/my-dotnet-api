using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        public string GetProductName(int productId)
        {
            if(productId <= 0)
            {
                throw new ArgumentException("Product ID must be greater than zero.", nameof(productId));
            }
            return $"Product Name for ID {productId}";
        }
    }
}
