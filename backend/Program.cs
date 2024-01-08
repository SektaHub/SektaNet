using backend;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://example.com",
                                "http://localhost:5173",
                                "http://localhost:8000",
                                "http://127.0.0.1:3000"
                                ).AllowAnyHeader()
                                .AllowAnyMethod(); // Allow any HTTP method
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient
builder.Services.AddHttpClient();

//builder.Services.AddDbContext<ApplicationDbContext>();

var configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ReelService>();
builder.Services.AddScoped<ImageService>();


builder.Services.AddAutoMapper(typeof(MyMappingProfile));


var app = builder.Build();

// Resolve the service to call the method
using (var scope = app.Services.CreateScope())
{
    //Initialise the services so that they create the necessary folders if they are not present in the project
    var imageService = scope.ServiceProvider.GetRequiredService<ImageService>();
    imageService.InitDirectories();

    var reelService = scope.ServiceProvider.GetRequiredService<ReelService>();
    reelService.InitDirectories();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
