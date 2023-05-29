using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Dtos
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
