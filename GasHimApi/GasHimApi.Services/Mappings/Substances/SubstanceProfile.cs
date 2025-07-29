using AutoMapper;
using GasHimApi.Data.Models;
using GasHimApi.Contracts.Substances;

namespace GasHimApi.Services.Mappings.Substances
{
    public class SubstanceProfile : Profile
    {
        public SubstanceProfile()
        {
            // 1) Создание → Entity
            CreateMap<SubstanceCreateDto, Substance>();
            // 2) Обновление → Entity
            CreateMap<SubstanceUpdateDto, Substance>();
            // 3) Entity → Read-DTO
            CreateMap<Substance, SubstanceDto>();
        }
    }
}
