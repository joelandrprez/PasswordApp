using Cuentas.Backend.Aplication.Cuentas;
using Cuentas.Backend.Aplication.Token;
using Cuentas.Backend.Domain.Cuentas.Interfaces;
using Cuentas.Backend.Infraestruture;
using Cuentas.Backend.Infraestruture.Cuentas;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;
using Cuentas.Backend.API.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Cuentas.Backend.Aplication.Usuario;
using Cuentas.Backend.Infraestruture.Usuario;
using Cuentas.Backend.Domain.Usuario.Interfaces;
using Cuentas.Backend.Aplication.EstadoProyecto;
using Cuentas.Backend.Infraestruture.EstadoProyecto;
using Cuentas.Backend.Domain.EstadoProyecto.Interfaces;
using Cuentas.Backend.Aplication.Proyecto;
using Cuentas.Backend.Infraestruture.Proyecto;
using Cuentas.Backend.Domain.Proyectos.Interfaces;
using Cuentas.Backend.Aplication.Menu;
using Cuentas.Backend.Infraestruture.Menu;
using Cuentas.Backend.Domain.Menu.Interfaces;

string AllAllowSpecificOrigins = "_AllAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("appsettings.local.json", true, true);



builder.Services.ConfigureJWT(true);

builder.Services.AddAuthenticationCore();
builder.Services.AddDataProtection();
builder.Services.AddWebEncoders();
builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
//END::AddAuthentication.cs


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });
    c.DocInclusionPredicate((name, api) => true);
});




builder.Services.AddScoped<ICustomConnection, CustomConnection>();

builder.Services.AddTransient<Cuentas.Backend.Aplication.Cuentas.CuentaApp>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();

builder.Services.AddTransient<AuthApp>();

builder.Services.AddTransient<Cuentas.Backend.Aplication.Usuario.UsuarioApp>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddTransient<EstadoProyectoApp>();
builder.Services.AddScoped<IEstadoProyectoRepository, EstadoProyectoRepository>();

builder.Services.AddTransient<ProyectoApp>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();

builder.Services.AddTransient<MenuApp>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
