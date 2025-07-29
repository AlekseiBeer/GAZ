namespace GasHimApi.Data.Models;

public class Process
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? PrimaryFeedstocks { get; set; }
    public string? SecondaryFeedstocks { get; set; }
    public string? PrimaryProducts { get; set; }
    public string? ByProducts { get; set; }
    public double YieldPercentage { get; set; }
}