using System;
using System.Collections.Generic;
using System.Text;

namespace PL.DTOS
{
   public class ProductDTO : BaseDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal Advertisement { get; set; }
        public decimal Discount { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

    }
}
