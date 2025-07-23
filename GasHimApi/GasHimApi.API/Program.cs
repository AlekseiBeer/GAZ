using GasHimApi.API.Services;
using GasHimApi.Data;

var builder = WebApplication.CreateBuilder(args);

// –егистрируем все зависимости, св€занные с базой данных, через метод из модул€ Data
builder.Services.AddDataModuleServices(builder.Configuration);

// –егистрируем наш сервис дл€ синтеза цепочек
builder.Services.AddScoped<ChainsService>();

// –егистрируем контроллеры, Swagger, CORS и т.д.
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
