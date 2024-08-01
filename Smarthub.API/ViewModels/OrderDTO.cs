

using Smarthub.API.Models;
using static Smarthub.API.Models.Enums.Shared;

namespace Smarthub.API.ViewModels
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }

        public string CustomerName { get; set; }

        public string OrderNumber { get; set; }

        public eOrderType OrderType { get; set; }

        public eOrderStatus OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime OrderCreatedDate { get; set; }

        public List<OrderLine>? OrderLines { get; set; }


    }


}
