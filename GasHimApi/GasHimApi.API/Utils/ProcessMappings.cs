using GasHimApi.API.Dtos;
using GasHimApi.Data.Models;

namespace GasHimApi.API.Utils;

public static class ProcessMappings
{
    public static ProcessDto ToDto(this Process p) =>
        new(p.Id, p.Name!, p.MainInputs, p.AdditionalInputs,
            p.MainOutputs, p.AdditionalOutputs, p.YieldPercent);

    public static void UpdateEntity(this Process entity, ProcessDto dto)
    {
        entity.Name = dto.Name;
        entity.MainInputs = dto.MainInputs;
        entity.AdditionalInputs = dto.AdditionalInputs;
        entity.MainOutputs = dto.MainOutputs;
        entity.AdditionalOutputs = dto.AdditionalOutputs;
        entity.YieldPercent = dto.YieldPercent;
    }
}
