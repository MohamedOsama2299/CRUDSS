    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace DAL.Models
    {
      public class Category : BaseEntity
        {
            public string Name { get; set; }
            // Navigation Property
            // A category can have multiple products
            public List<Product> Products { get; set; }
        }
    }
