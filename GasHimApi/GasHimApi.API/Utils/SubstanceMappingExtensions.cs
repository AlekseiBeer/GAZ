using GasHimApi.API.Dtos;
using GasHimApi.Data.Models;

namespace GasHimApi.API.Utils;

public static class SubstanceMappingExtensions
{
    public static SubstanceDto ToDto(this Substance s)
        => new(s.Id, s.Name ?? string.Empty, s.Synonyms);

    public static Substance ToEntity(this SubstanceCreateUpdateDto dto)
        => new()
        {
            Name = dto.Name,
            Synonyms = dto.Synonyms
        };

    public static void UpdateEntity(this Substance entity, SubstanceCreateUpdateDto dto)
    {
        entity.Name = dto.Name;
        entity.Synonyms = dto.Synonyms;
    }
}
