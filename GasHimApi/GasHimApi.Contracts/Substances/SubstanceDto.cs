namespace GasHimApi.Contracts.Substances;

public record SubstanceDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Synonyms { get; init; }
}