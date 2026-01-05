using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zizo_shop.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; } // 1–5
        public string Comment { get; set; }

        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }

}
