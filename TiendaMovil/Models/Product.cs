using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaMovil.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image_path { get; set; }
        public int stock { get; set; }
        public float price { get; set; }
        public int brand_id { get; set; }
        public int category_id { get; set; }
        public brand brand { get; set; }
        public category category { get; set; }
    }
}
