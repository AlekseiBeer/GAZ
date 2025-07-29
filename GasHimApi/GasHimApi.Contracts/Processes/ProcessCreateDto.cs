using System.ComponentModel.DataAnnotations;

namespace GasHimApi.Contracts.Processes;

public record ProcessCreateDto
{
    [Required, StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string Name { get; init; } = null!;

    [StringLength(500, ErrorMessage = "Primary feedstocks list is too long.")]
    public string? PrimaryFeedstocks { get; init; }

    [StringLength(500, ErrorMessage = "Secondary feedstocks list is too long.")]
    public string? SecondaryFeedstocks { get; init; }

    [StringLength(500, ErrorMessage = "Primary products list is too long.")]
    public string? PrimaryProducts { get; init; }

    [StringLength(500, ErrorMessage = "By-products list is too long.")]
    public string? ByProducts { get; init; }

    [Range(0, 100, ErrorMessage = "Yield percentage must be between 0 and 100.")]
    public double YieldPercentage { get; init; }
}
