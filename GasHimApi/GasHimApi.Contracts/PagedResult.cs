namespace GasHimApi.Contracts;

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int? Total,         // ����� ���������� null, ����� �� ������� ������ ���
    string? NextCursor, // Base64-������ ��� ��������� ��������
    bool HasMore
);
