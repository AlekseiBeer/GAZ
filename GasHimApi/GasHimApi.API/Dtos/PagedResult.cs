namespace GasHimApi.API.Dtos
{ 
    public record PagedResult<T>(
        IReadOnlyList<T> Items,
        int? Total,         // Можно возвращать null, чтобы не считать каждый раз
        string? NextCursor, // Base64-курсор для следующей страницы
        bool HasMore
    );
}


