using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static zizo_shop.Domain.Enums.Enums;

namespace zizo_shop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }

        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public Guid AddressId { get; set; }
        public Address ShippingAddress { get; set; }

        public ICollection<OrderItem> Items { get; set; }
    }

}
