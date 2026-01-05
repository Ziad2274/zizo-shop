using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zizo_shop.Domain.Enums
{
    public class Enums
    {
        public enum OrderStatus
        {
            Pending,
            Paid,
            Shipped,
            Delivered,
            Cancelled
        }

        public enum PaymentStatus
        {
            Pending,
            Completed,
            Failed
        }

    }
}
