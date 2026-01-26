using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zizo_shop.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get;private set; }
        public string Description { get; private set; }

        public decimal Price { get; private set; }
        public decimal? DiscountPrice { get; private set; }

        public int StockQuantity { get; private set; }
        public string SKU { get; set; }

        public bool IsActive { get; set; } = true;
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public Guid? MainImageId { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<Review> Reviews { get; set; }


        public Product(string name, string description, decimal price, int stockQuantity, Guid categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            CategoryId = categoryId;
        }
        private Product() { }
        public void AddDiscount(decimal discountPrice)
        {
            if (discountPrice >= Price)
            {
                throw new ArgumentException("Discount price must be less than the original price.");
            }
            DiscountPrice = discountPrice;
        }

        public void UpdateStock(int quantity)
        {
            if (quantity < 0 && Math.Abs(quantity) > StockQuantity)
            {
                throw new InvalidOperationException("Insufficient stock to reduce.");
            }
            StockQuantity += quantity;
        }
        public void RemoveStock(int quantity )
        {
           UpdateStock(-quantity);
        }

    }

}
