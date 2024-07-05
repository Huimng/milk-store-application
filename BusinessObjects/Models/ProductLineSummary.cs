using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class ProductLineSummary
    {
        public int Quantity { get; set; }
        public DateTime ExpireDate { get; set; }
        public string? AgeGroup { get; set; }
    }
}
