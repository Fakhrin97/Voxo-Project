using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voxo.BLL.Data;

namespace Voxo_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ConstantsData.AdminRole)]
    public class BaseController : Controller
    {        
    }
}
