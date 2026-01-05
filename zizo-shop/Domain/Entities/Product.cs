using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zizo_shop.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }

        public int Stock { get; set; }
        public string SKU { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductImage> Images { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

}
