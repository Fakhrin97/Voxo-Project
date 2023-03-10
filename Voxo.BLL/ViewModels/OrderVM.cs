
namespace Voxo.BLL.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime Time { get; set; }
        public string Name { get; set; } 
        public bool? Status { get; set; }   
    }
}
