using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetCare.API.Data;
using PetCare.API.Helpers;
using PetCare.API.Middleware;
using PetCare.API.Repositories;
using PetCare.API.Repositories.Interfaces;
using PetCare.API.Services;
using PetCare.API.Services.Interfaces;
using Scalar.AspNetCore;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── PostgreSQL ────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── JWT ───────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" // ← agrega esto
        };
    });

// ── OpenAPI + Scalar ──────────────────────────────────────
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new Microsoft.OpenApi.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };
        return Task.CompletedTask;
    });
});

// ── Dependencias ──────────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();
builder.Services.AddScoped<JwtHelper>();

// ── API Versioning ────────────────────────────────────────
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true; // devuelve headers api-supported-versions
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),   // /api/v1/...
        new HeaderApiVersionReader("PetCare-Api-Version") // opcional: por header
    );
})
.AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
builder.Services.AddAuthorization();


//Cords
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



var app = builder.Build();

var version = System.Reflection.Assembly
    .GetExecutingAssembly()
    .GetName()
    .Version?.ToString() ?? "3.2.0";

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = $"PetCare API v{version}";
        options.Theme = ScalarTheme.DeepSpace;
        options.AddPreferredSecuritySchemes("Bearer")
            .AddHttpAuthentication("Bearer", bearer =>
            {
                bearer.Token = "";
            });
    });
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("cors");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => Results.Redirect("/scalar/v1"));
app.MapControllers();
app.Run();
