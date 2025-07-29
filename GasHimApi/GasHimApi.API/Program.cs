using GasHimApi.API.Services;
using GasHimApi.Data;
using AutoMapper;
using GasHimApi.API.Services.Processes;
using GasHimApi.Services.Services.Substances;
using GasHimApi.Services.Mappings.Processes;
using GasHimApi.Services.Mappings.Substances;

var builder = WebApplication.CreateBuilder(args);

// 1) Конфигурируем AutoMapper — сканируем в текущей сборке все классы Profile
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ProcessProfile>();
    cfg.AddProfile<SubstanceProfile>();
});
// Регистрируем все зависимости, связанные с базой данных, через метод из модуля Data
builder.Services.AddDataModuleServices(builder.Configuration);

// Регистрируем наш сервис для синтеза цепочек
builder.Services.AddScoped<ChainsService>();

// Регестрируем сервисы процессов
builder.Services.AddScoped<IProcessQueryService, ProcessQueryService>();
builder.Services.AddScoped<IProcessCommandService, ProcessCommandService>();

// Регестрируем сервисы веществ
builder.Services.AddScoped<ISubstancesQueryService, SubstancesQueryService>();
builder.Services.AddScoped<ISubstancesCommandService, SubstancesCommandService>();

// Регестрируем тесты
builder.Services.AddScoped<ITestDataService, TestDataService>();

// Регистрируем контроллеры, Swagger, CORS и т.д.
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