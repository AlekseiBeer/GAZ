using GasHimApi.API.Services;
using GasHimApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ������������ ��� �����������, ��������� � ����� ������, ����� ����� �� ������ Data
builder.Services.AddDataModuleServices(builder.Configuration);

// ������������ ��� ������ ��� ������� �������
builder.Services.AddScoped<ChainsService>();

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
