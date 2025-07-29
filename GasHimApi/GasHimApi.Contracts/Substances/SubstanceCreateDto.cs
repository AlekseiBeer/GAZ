using System.ComponentModel.DataAnnotations;

namespace GasHimApi.Contracts.Substances;

public record SubstanceCreateDto
{
    [Required, StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; init; } = null!;
    [StringLength(500, ErrorMessage = "Synonyms cannot exceed 500 characters.")]
    public string? Synonyms { get; init; }
}

