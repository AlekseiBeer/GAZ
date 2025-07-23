namespace GasHimApi.API.Dtos;

public record ProcessDto(
    int Id,
    string Name,
    string? MainInputs,
    string? AdditionalInputs,
    string? MainOutputs,
    string? AdditionalOutputs,
    double YieldPercent
);
