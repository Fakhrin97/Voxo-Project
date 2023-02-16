
namespace Voxo.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand,BrandVM>().ReverseMap();    
            CreateMap<Brand, BrandUpdateVM>().ReverseMap();    
            CreateMap<Brand, BrandImgVM>().ReverseMap();    
            CreateMap<FooterLogo, FooterLogoVM>().ReverseMap();    
            CreateMap<FooterLogo, FooterLogoUpdateVM>().ReverseMap();    
            CreateMap<FooterLogo, FooterLogoImgVM>().ReverseMap();    
            CreateMap<Blog, BlogsVM>().ReverseMap();    
            CreateMap<Blog, BlogPageVM>().ReverseMap();    
            CreateMap<Blog, PopularBlogsVM>().ReverseMap();    
            CreateMap<Blog, BlogDetailsVM>().ReverseMap();    
            CreateMap<Blog, BlogUpdateVM>().ReverseMap();    
            CreateMap<Category, CategoryVM>().ReverseMap();    
            CreateMap<Category, CategoryUpdateVM>().ReverseMap();    
            CreateMap<Category, CategoryShopVM>().ReverseMap();    
            CreateMap<Slider, SliderVM>().ReverseMap();    
            CreateMap<Slider, SliderHomeVM>().ReverseMap();    
            CreateMap<Slider, SliderUpdateVM>().ReverseMap();    
            CreateMap<Product, ProductAdminVM>().ReverseMap();    
            CreateMap<Product, LatestProductsVM>().ReverseMap();    
            CreateMap<Product, ProductDetailsVM>().ReverseMap();    
            CreateMap<Product , WishListVM>().ReverseMap();    
            CreateMap<Product , ProductVM>().ReverseMap();    
            CreateMap<Product, MostPapularProduct>().ReverseMap();    
            CreateMap<User, RegisterVM>().ReverseMap();    
            CreateMap<ContactMessage, ContactMessageVM>().ReverseMap();    
        }
    }
}
