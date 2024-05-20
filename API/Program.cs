using AutoMapper;
using FinanceManagement.API.Helpers;
using FinanceManagement.Core.Managers;
using FinanceManagement.Core.Managers.Implementations;
using FinanceManagement.Core.UnitOfWork;
using FinanceManagement.DataAccess;
using FinanceManagement.Services.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Finance Management API",
        Description = "Web API for the Finance Management Application"
    });
});
builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FinanceManagementDatabase")));
builder.Services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase(databaseName: "TestDb"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoriesManager, CategoriesManager>();
builder.Services.AddScoped<IPeriodsManager, PeriodsManager>();
builder.Services.AddScoped<IFinancialTransactionsManager, FinancialTransactionsManager>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware(typeof(ExceptionHandler));

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
