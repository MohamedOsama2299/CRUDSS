using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
   public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Tax  { get; set; }
        public decimal Advertisement{ get; set; }
        public decimal Discount { get; set; }
        public int StockQuantity { get; set; }
        // Foreign Key
        public int CategoryID { get; set; }
        // Navigation Property
        // A product belongs to one category
        public Category Category { get; set; }
    }
}
