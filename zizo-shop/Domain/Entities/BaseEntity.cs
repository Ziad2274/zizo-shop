using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zizo_shop.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
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
        internal void SetId(Guid id)
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
