
namespace Voxo_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantsData.AdminRole)]
    public class BaseController : Controller
    {        
    }
}
