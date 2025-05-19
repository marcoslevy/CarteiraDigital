using CarteiraDigital.API.Configurations;
using CarteiraDigital.Application.Carteiras.Commands;
using CarteiraDigital.Application.Carteiras.Validators;
using CarteiraDigital.Application.Transacoes.Commands;
using CarteiraDigital.Application.Transacoes.Validations;
using CarteiraDigital.Application.Usuarios.Commands;
using CarteiraDigital.Application.Usuarios.Results;
using CarteiraDigital.Application.Usuarios.Validations;
using CarteiraDigital.Core.Entities.Transacoes;
using CarteiraDigital.Core.Entities.Usuarios;
using CarteiraDigital.Core.Interfaces;
using CarteiraDigital.Core.Interfaces.Repositories;
using CarteiraDigital.Core.Results;
using CarteiraDigital.Infra.Data;
using CarteiraDigital.Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CarteiraDigitalDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("CarteiraDigital.Infra")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICarteiraRepository, CarteiraRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<DepositarCommand>());
builder.Services.AddTransient<IPipelineBehavior<DepositarCommand, ResultadoOperacaoTransacao>, DepositarValidator>();
builder.Services.AddTransient<IPipelineBehavior<SacarCommand, ResultadoOperacaoTransacao>, SacarValidator>();
builder.Services.AddTransient<IPipelineBehavior<TransferirCommand, ResultadoOperacaoTransacao>, TransferirValidator>();
builder.Services.AddTransient<IPipelineBehavior<ObterPorIdCommand, ResultadoOperacaoTransacao>, ObterPorIdValidator>();
builder.Services.AddTransient<IPipelineBehavior<ObterPorUsuarioEhDataCommand, ResultadoOperacao<IEnumerable<Transacao>>>, ObterPorUsuarioEhDataValidator>();
builder.Services.AddTransient<IPipelineBehavior<RegistrarCommand, ResultadoOperacao<UsuarioResult>>, RegistrarValidator>();

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddDefaultIdentity<Usuario>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 3;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;

        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
.AddEntityFrameworkStores<CarteiraDigitalDbContext>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira um token válido",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetService<CarteiraDigitalDbContext>();
    context.Database.Migrate();
}

app.Run();
