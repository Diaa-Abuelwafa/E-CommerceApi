using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Product_Module
{
    public class ProductParams
    {
        public string? OrderByAsec { get; set; }
        public string? OrderByDesc { get; set; }
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? SearchByName { get; set; }
    }
}
