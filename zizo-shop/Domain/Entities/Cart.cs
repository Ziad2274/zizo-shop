using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zizo_shop.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get;protected set; }
        private readonly List<CartItem> _items = new List<CartItem>();
        public virtual ICollection<CartItem> Items { get; set; }
        public Cart( Guid userId)
        {
           UserId = userId;
        }
        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
            var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = quantity,
                    CartId = this.Id,
                    Cart = this
                }
                );
            }
        }
        public void RemoveItem(Guid productId)
        {
            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                _items.Remove(existingItem);
            }
        }
    }

}
