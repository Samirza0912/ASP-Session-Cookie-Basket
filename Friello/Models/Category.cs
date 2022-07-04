using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Models
{
    public class Category
    {
        public int id { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Desc { get; set; }
        public List<Product> Products { get; set; }
    }
}
