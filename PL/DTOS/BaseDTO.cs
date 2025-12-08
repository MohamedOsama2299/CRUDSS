using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace PL.DTOS
{
   public class BaseDTO
    {
      [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
