using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static zizo_shop.Domain.Enums.Enums;

namespace zizo_shop.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }
        public string Provider { get; set; } // Stripe, Cash, etc.
    }

}
