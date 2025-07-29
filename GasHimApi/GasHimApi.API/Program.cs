using GasHimApi.API.Services;
using GasHimApi.Data;
using AutoMapper;
using GasHimApi.API.Services.Processes;
using GasHimApi.Services.Services.Substances;
using GasHimApi.Services.Mappings.Processes;
using GasHimApi.Services.Mappings.Substances;

var builder = WebApplication.CreateBuilder(args);

// 1) ������������� AutoMapper � ��������� � ������� ������ ��� ������ Profile
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ProcessProfile>();
    cfg.AddProfile<SubstanceProfile>();
});
// ������������ ��� �����������, ��������� � ����� ������, ����� ����� �� ������ Data
builder.Services.AddDataModuleServices(builder.Configuration);

// ������������ ��� ������ ��� ������� �������
builder.Services.AddScoped<ChainsService>();

// ������������ ������� ���������
builder.Services.AddScoped<IProcessQueryService, ProcessQueryService>();
builder.Services.AddScoped<IProcessCommandService, ProcessCommandService>();

// ������������ ������� �������
builder.Services.AddScoped<ISubstancesQueryService, SubstancesQueryService>();
builder.Services.AddScoped<ISubstancesCommandService, SubstancesCommandService>();

// ������������ �����
builder.Services.AddScoped<ITestDataService, TestDataService>();

// ������������ �����������, Swagger, CORS � �.�.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();