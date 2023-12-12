using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaMovil.Models
{
    public class shoppingCart
    {
        public int id { get; set; }
        public Product product { get; set; }
        public int quantity { get; set; }
        public float total { get; set; }
    }
}
