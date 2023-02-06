using Voxo.DAL.Base;

namespace Voxo.DAL.Entities
{
    public class Blog : TimeStample, IEntity
    {
        public int Id { get ; set ; }
        public bool Published { get ; set ; }
        public string ImageUrl { get ; set ; } 
        public string Title { get ; set ; } 
        public string Content { get ; set ; }    
        public int ViewersCount { get ; set ; }        

    }
}
