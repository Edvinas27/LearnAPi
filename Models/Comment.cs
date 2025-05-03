using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // Foreign key to Stock
        public Guid? StockId { get; set; }

        //Navigation property to Stock
        public Stock? Stock { get; set; }
        
    }
}