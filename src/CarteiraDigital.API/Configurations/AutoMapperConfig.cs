using AutoMapper;
using CarteiraDigital.Application.Transacoes.Results;
using CarteiraDigital.Core.Entities.Transacoes;

namespace CarteiraDigital.API.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Transacao, TransacaoResult>().ReverseMap();
    }
}
