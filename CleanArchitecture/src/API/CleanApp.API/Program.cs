using App.Persistence.Extensions;
using App.Application.Extensions;
using CleanApp.API.Filters;
using CleanApp.API.ExceptionHandler;
using App.Application.Contracts.Caching;
using App.Caching;
using CleanApp.API.Extensions;
using App.Bus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithFiltersExt().AddSwaggerGenExt().AddExceptionHandlerExt().AddCachingExt();


builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration).AddBusExt(builder.Configuration);

var app = builder.Build();

app.UseConfigurePipelineExt();

app.MapControllers();

app.Run();
