namespace GasHimApi.API.Dtos
{ 
    public record SubstanceQuery(
        string? Search,
        int Take = 50,
        string? Cursor = null
    );
}