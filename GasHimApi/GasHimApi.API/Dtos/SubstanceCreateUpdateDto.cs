namespace GasHimApi.API.Dtos
{
    public record SubstanceCreateUpdateDto(
        string Name,
        string? Synonyms
    );
}
