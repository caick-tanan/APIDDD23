using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Generics;
using Domain.Interfaces.InterfceServices;
using Domain.InterfacesExternal;
using Domain.Services;
using Entities.Entities;
using Infraestructure.Configuration;
using Infraestructure.Repository.Generics;
using Infraestructure.Repository.Repositories;
using Infrastructure.RepositoriesExternal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebAPIs.Models;
using WebAPIs.Token;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>

{

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API_DDD_2023", Version = "v1" });



    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme

    {

        Description =

            "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +

            "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +

            "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",

        Name = "Authorization",

        In = ParameterLocation.Header,

        Type = SecuritySchemeType.ApiKey,

        Scheme = "Bearer",

        BearerFormat = "JWT",

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement

                {

                    {

                        new OpenApiSecurityScheme

                        {

                            Reference = new OpenApiReference

                            {

                                Type = ReferenceType.SecurityScheme,

                                Id = "Bearer"

                            }

                        },

                        new string[] {}

                    }

                });

});

// ConfigServices
builder.Services.AddDbContext<ContextBase>(options =>
              options.UseSqlServer(
                  builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ContextBase>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// INTERFACE E REPOSITORIO
builder.Services.AddSingleton(typeof(IGeneric<>), typeof(RepositoryGenerics<>));
builder.Services.AddSingleton<IMessage, RepositoryMessage>();
builder.Services.AddSingleton<IProduto, RepositoryProduto>();

// SERVIÇO DOMINIO
builder.Services.AddSingleton<IServiceMessage, ServiceMessage>();


// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(option =>
      {
          option.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = false,
              ValidateAudience = false,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,

              ValidIssuer = "Teste.Securiry.Bearer",
              ValidAudience = "Teste.Securiry.Bearer",
              IssuerSigningKey = JwtSecurityKey.Create("Secret_Key-12345678")
          };

          option.Events = new JwtBearerEvents
          {
              OnAuthenticationFailed = context =>
              {
                  Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                  return Task.CompletedTask;
              },
              OnTokenValidated = context =>
              {
                  Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                  return Task.CompletedTask;
              }
          };
      });


var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.CreateMap<MessageViewModel, Message>();
    cfg.CreateMap<Message, MessageViewModel>();
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//var urlDev = "https://dominiodocliente.com.br";
//var urlHML = "https://dominiodocliente2.com.br";
//var urlPROD = "https://dominiodocliente3.com.br";

//app.UseCors(b => b.WithOrigins(urlDev, urlHML, urlPROD));

/*var devClient = "http://localhost:4200";*/
var devClient = "http://localhost:3000";

app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader().WithOrigins(devClient));


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerUI();

app.Run();