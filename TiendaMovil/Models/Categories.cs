using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaMovil.Models
{
    public class Categories
    {
        public int id { get; set; }
        public string name { get; set; }
        public Product[] products { get; set; }
    }
}
