using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zizo_shop.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get;protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public bool IsDeleted { get; protected set; }= false;
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        public void MarkAsDeleted()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }
        public void MarkAsUpdate()
        {
            UpdatedAt = DateTime.UtcNow;
        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public void Restore()
        {
            IsDeleted = false;
            UpdatedAt = DateTime.UtcNow;
        }
        
    }
}
