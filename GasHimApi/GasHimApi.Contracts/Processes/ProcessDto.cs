namespace GasHimApi.Contracts.Processes;

public record ProcessDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? PrimaryFeedstocks { get; init; }
    public string? SecondaryFeedstocks { get; init; }
    public string? PrimaryProducts { get; init; }
    public string? ByProducts { get; init; }
    public double YieldPercentage { get; init; }
}