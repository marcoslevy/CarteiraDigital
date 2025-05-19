using CarteiraDigital.Application.Transacoes.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransacaoController : BaseController
{
    private readonly IMediator _mediator;

    public TransacaoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("historico")]
    public async Task<IActionResult> GetHisorico([FromQuery] ObterPorUsuarioEhDataCommand request)
    {
        request.SetUsuarioId(UsuarioId);

        var result = await _mediator.Send(request);

        if (result.Falhou)
            return BadRequest(new { result.MensagemErro });

        return Ok(result.Dado);
    }

    [HttpGet("{transacaoId}")]
    public async Task<IActionResult> GetTransacao(Guid transacaoId)
    {
        var result = await _mediator.Send(new ObterPorIdCommand(transacaoId));

        if (result.Falhou)
            return BadRequest(new { result.MensagemErro });

        if (result.Dado is null)
            return NotFound(new { Message = "Transação não encontrada." });

        return Ok(result.Dado);
    }
}
