using System.ComponentModel.DataAnnotations;
using static Smarthub.API.Models.Enums.Shared;

namespace Smarthub.API.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string OrderNumber { get; set; }

        [Required]
        public eOrderType OrderType { get; set; }

        [Required]
        public eOrderStatus OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime OrderCreatedDate { get; set; } = DateTime.Now;

        public List<OrderLine>? OrderLines { get; set; }


    }
}
