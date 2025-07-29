namespace GasHimApi.Contracts.Substances;

public record SubstanceQuery(
    string? Search,
    int Take = 50,
    string? Cursor = null
);
