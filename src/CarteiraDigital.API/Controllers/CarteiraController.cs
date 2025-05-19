using CarteiraDigital.Application.Carteiras.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CarteiraController : BaseController
{
    private readonly IMediator _mediator;

    public CarteiraController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("saldo")]
    public async Task<IActionResult> GetSaldo()
    {
        var result = await _mediator.Send(new ObterSaldoCommand(UsuarioId));

        if (result.Falhou)
            return BadRequest(new { result.MensagemErro });

        return Ok(result.Dado);
    }

    [HttpPost("deposito")]
    public async Task<IActionResult> PostDeposito([FromBody] DepositarCommand request)
    {
        request.SetUsuarioId(UsuarioId);

        var result = await _mediator.Send(request);

        if(result.Falhou)
            return BadRequest(new { result.MensagemErro });

        return Ok(result.Dado);
    }

    [HttpPost("Saque")]
    public async Task<IActionResult> PostSaque([FromBody] SacarCommand request)
    {
        request.SetUsuarioId(UsuarioId);

        var result = await _mediator.Send(request);

        if (result.Falhou)
            return BadRequest(new { result.MensagemErro });

        return Ok(result.Dado);
    }

    [HttpPost("Transferencia")]
    public async Task<IActionResult> PostTransferencia([FromBody] TransferirCommand request)
    {
        request.SetUsuarioId(UsuarioId);

        var result = await _mediator.Send(request);

        if (result.Falhou)
            return BadRequest(new { result.MensagemErro });

        return Ok(result.Dado);
    }
}
