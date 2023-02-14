
namespace Voxo.BLL.ViewModels
{
    public class BlogPageVM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

    }
}
