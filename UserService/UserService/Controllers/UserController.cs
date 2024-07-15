using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public String Get()
        {
            return "Nall";
        }
    }
}
