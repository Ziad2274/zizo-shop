using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static zizo_shop.Domain.Enums.Enums;

namespace zizo_shop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; private set; }

        public decimal SubTotal { get; private set; }
        public decimal ShippingFee { get; private set; }
        [NotMapped]
        public decimal TotalPrice => SubTotal + ShippingFee;

        public OrderStatus Status { get; private set; }

        public Guid AddressId { get; set; }
        public Address ShippingAddress { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public Order(Guid userId)
        {
            UserId = userId;
            Status = OrderStatus.Pending;
            Items = new List<OrderItem>();
        }
        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                RecalculateSubTotal();
                return;
            }
            else
            {
                Items.Add(new OrderItem
                {
                    OrderId = this.Id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.DiscountPrice ?? product.Price,
                    Quantity = quantity
                });
            }
            RecalculateSubTotal();
        }
        public void RecalculateSubTotal()
        {
            SubTotal = Items.Sum(item => item.Price * item.Quantity);
        }
    
    public void MarkAsPaid() {
            if (Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be marked as paid.");
            }
            Status = OrderStatus.Paid;
        }
        public void MarkAsShipped()
        {
            if (Status != OrderStatus.Paid)
            {
                throw new InvalidOperationException("Only paid orders can be marked as shipped.");
            }
            Status = OrderStatus.Shipped;
        }


        }


    }
