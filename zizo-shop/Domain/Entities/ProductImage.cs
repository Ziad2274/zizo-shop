using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zizo_shop.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public bool IsCover { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }

}
