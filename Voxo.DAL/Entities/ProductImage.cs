﻿using Voxo.DAL.Base;

namespace Voxo.DAL.Entities
{
    public class ProductImage : IEntity
    {
        public int Id { get ; set ; }
        public bool Published { get ; set ; }
        public string Name { get ; set ; }  
        public int ProductId { get ; set ; }    
        public Product Product {  get ; set ; }       
        
    }
}
