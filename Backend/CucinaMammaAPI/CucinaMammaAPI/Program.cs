using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CucinaMammaAPI.Data;
using Microsoft.EntityFrameworkCore;
using CucinaMammaAPI.Interfaces;
using CucinaMammaAPI.Services;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// 📌 1️⃣ Configura il DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 📌 2️⃣ Legge i parametri JWT da appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// 📌 3️⃣ Configura l'autenticazione JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services
       .AddFluentValidationAutoValidation()
        .AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<Program>();
        });

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 📌 4️⃣ Registra il Service
builder.Services.AddScoped<ICategoriaRepository, CategoriaService>();
builder.Services.AddScoped<IImmagineService, ImmagineService>();
builder.Services.AddScoped<IRicettaService, RicettaService>();
builder.Services.AddScoped<IPassaggioPreparazioneService, PassaggioPreparazioneService>();
builder.Services.AddScoped<IIngredienteService, IngredienteService>();

builder.Services.AddSingleton<RabbitMqService>();


// 📌 5️⃣ Abilita CORS per Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policyBuilder => policyBuilder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// 📌 6️⃣ Configura i controller e JSON Serializer
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// 📌 7️⃣ Abilita Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 📌 8️⃣ Abilita Autorizzazione (SPOSTATA QUI)
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseMiddleware<CucinaMammaAPI.Infrastructure.Middleware.GlobalErrorMiddleware>();


// 📌 9️⃣ Middleware
app.UseCors("AllowAllOrigins");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();  // ⬅️ Necessario per JWT
app.UseAuthorization();
app.MapControllers();
app.Run();
