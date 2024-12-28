using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
    [ApiController]
    [Authorize]
public class SecuredController : ControllerBase
{
    
    
}