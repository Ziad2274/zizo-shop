using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.DTOs.Product
{
    public class ProductDto
    {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

            public decimal Price { get; set; }
            public decimal? DiscountPrice { get; set; }

            public int Stock { get; set; }
            public string SKU { get; set; }

            public bool IsActive { get; set; } = true;
            public bool IsInWishlist { get; set; }
            public Guid CategoryId { get; set; }
            public string CategoryName { get; set; }

        public ICollection<ProductImage> Images { get; set; }
            public ICollection<Review> Reviews { get; set; }
        

    }
}
