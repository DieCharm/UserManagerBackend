using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserManager
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : 
        ControllerBase
    {
        
    }
}