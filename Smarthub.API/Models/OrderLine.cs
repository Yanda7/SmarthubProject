using static Smarthub.API.Models.Enums.Shared;

namespace Smarthub.API.Models
{
    public class OrderLine
    {
        public Guid OrderLineId { get; set; }

        public int LineNumber { get; set; } = 1;

        public string ProductCode { get; set; }

        public eProductType ProductType { get; set; }

        public double ProductCostPrice { get; set; }

        public double ProductSalesPrice { get; set; }

        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
    }
}
