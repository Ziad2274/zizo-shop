using zizo_shop.Domain.Enums;

namespace zizo_shop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }
        public decimal SubTotal { get; private set; }
        public decimal ShippingFee { get; private set; }
        public decimal TotalPrice { get; private set; }
        public OrderStatus Status { get; private set; }
        public Guid AddressId { get; set; }
        public Address ShippingAddress { get; set; } = null!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Order(Guid userId, decimal shippingFee = 0)
        {
            UserId = userId;
            ShippingFee = shippingFee;
            Status = OrderStatus.Pending;
        }
        private Order() { }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
            var existing = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null) existing.Quantity += quantity;
            else Items.Add(new OrderItem
            {
                OrderId = Id, ProductId = product.Id,
                ProductName = product.Name,
                Price = product.DiscountPrice ?? product.Price,
                Quantity = quantity
            });
            RecalculateSubTotal();
        }

        public void RecalculateSubTotal()
        {
            SubTotal = Items.Sum(i => i.Price * i.Quantity);
            TotalPrice = SubTotal + ShippingFee;
        }

        public void MarkAsPaid()
        {
            if (Status != OrderStatus.Pending) throw new InvalidOperationException("Only pending orders can be marked as paid.");
            Status = OrderStatus.Paid;
        }

        public void MarkAsShipped()
        {
            if (Status != OrderStatus.Paid) throw new InvalidOperationException("Only paid orders can be marked as shipped.");
            Status = OrderStatus.Shipped;
        }

        public void MarkAsDelivered()
        {
            if (Status != OrderStatus.Shipped) throw new InvalidOperationException("Only shipped orders can be marked as delivered.");
            Status = OrderStatus.Delivered;
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Shipped || Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot cancel a shipped or delivered order.");
            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order is already cancelled.");
            Status = OrderStatus.Cancelled;
        }
    }
}
