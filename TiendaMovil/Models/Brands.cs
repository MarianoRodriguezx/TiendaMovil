using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaMovil.Models
{
    public class Brands
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string logo_path { get; set; }
        public Product[] products { get; set; }
    }
}
