namespace GasHimApi.API.Dtos
{
    public record SubstancesPage(List<SubstanceDto> Items, string? NextCursor);
}
