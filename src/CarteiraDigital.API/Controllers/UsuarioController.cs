using CarteiraDigital.Application.Usuarios.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> PostRegistrar([FromBody] RegistrarCommand request)
    {
        var result = await _mediator.Send(request);

        if (result.Falhou)
            return BadRequest(new { result.MensagemErro });

        return Ok(result.Dado);
    }

    [HttpPost("login")]
    public async Task<IActionResult> PostLogin([FromBody] LoginCommand request)
    {
        var result = await _mediator.Send(request);

        if (result.Falhou)
            return BadRequest(new { result.MensagemErro });

        return Ok(result.Dado);
    }
}
