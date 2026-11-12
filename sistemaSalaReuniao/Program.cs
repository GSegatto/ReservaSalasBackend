using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaSalas.Api.Data;
using ReservaSalas.Api.Middleware;
using ReservaSalas.Api.Repositories;
using ReservaSalas.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISalaRepository, SalaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<ISalaService, SalaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Padroniza erros de validaÃ§Ã£o (DataAnnotations) no formato { mensagem, erros }
        options.InvalidModelStateResponseFactory = ctx =>
        {
            var erros = ctx.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return new BadRequestObjectResult(new { mensagem = string.Join(" ", erros), erros });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Frontend");
app.UseStaticFiles();
app.MapControllers();

app.Run();
