namespace GasHimApi.API.Dtos;

public record ProcessQuery(
    string? Search,
    int Take = 50,
    string? Cursor = null
);