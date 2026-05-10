namespace zizo_shop.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; protected set; }
        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        public Cart(Guid userId) { UserId = userId; }
        private Cart() { }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
            var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
                existingItem.Quantity += quantity;
            else
                Items.Add(new CartItem { ProductId = product.Id, Product = product, Quantity = quantity, CartId = this.Id, Cart = this });
        }

        public void RemoveItem(Guid productId)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null) Items.Remove(existingItem);
        }

        public void ClearItems() => Items.Clear();
    }
}
