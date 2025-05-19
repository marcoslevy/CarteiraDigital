using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarteiraDigital.API.Controllers;

public class BaseController : ControllerBase
{
    protected string UsuarioId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
