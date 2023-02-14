﻿
namespace Voxo.DAL.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Count { get; set; }
          
    }
}
