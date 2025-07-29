using AutoMapper;
using GasHimApi.Data.Models;
using GasHimApi.Contracts.Processes;

namespace GasHimApi.Services.Mappings.Processes
{
    public class ProcessProfile : Profile
    {
        public ProcessProfile()
        {
            // 1) Создание → Entity
            CreateMap<ProcessCreateDto, Process>();

            // 2) Обновление → Entity (наследуем правила из CreateDto)
            CreateMap<ProcessUpdateDto, Process>()
                .IncludeBase<ProcessCreateDto, Process>();

            // 3) Entity → Read-DTO
            CreateMap<Process, ProcessDto>();
        }
    }
}
