using Smarthub.Models;
using static Smarthub.API.Models.Enums.Shared;

namespace Smarthub.ViewModels
{
    public class OrderLineDTO
    {
        public Guid OrderLineId { get; set; }

        public int LineNumber { get; set; } 

        public string ProductCode { get; set; }

        public eProductType ProductType { get; set; }

        public double ProductCostPrice { get; set; }

        public double ProductSalesPrice { get; set; }

        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
    }
}
