using backend;
using backend.Models;
using backend.Repo;
using backend.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
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

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient
builder.Services.AddHttpClient();

//builder.Services.AddDbContext<ApplicationDbContext>();

var configuration = builder.Configuration;

builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),  o => o.UseVector()));

builder.Services.AddScoped<MongoDBRepository>(provider => new MongoDBRepository("mongodb://admin:admin123@localhost:27017/MongoBaza?authSource=admin", "MongoBaza"));

builder.Services.AddScoped<AnyFileRepository>();

builder.Services.AddScoped<ReelService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<FfmpegService>();
builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<GenericFileService>();
builder.Services.AddScoped<LongVideoService>();
builder.Services.AddScoped<AudioService>();

//builder.Services.AddScoped<MongoDBService>();


builder.Services.AddAutoMapper(typeof(MyMappingProfile));

builder.Services.Configure<FormOptions>(options =>
{
    //// Set various options to their maximum values
    options.MultipartBodyLengthLimit = long.MaxValue;
    //options.MultipartBoundaryLengthLimit = int.MaxValue;
    //options.MultipartHeadersLengthLimit = int.MaxValue;
    //options.MultipartHeadersCountLimit = int.MaxValue;
    //options.BufferBodyLengthLimit = long.MaxValue;
    options.ValueCountLimit = 5000;
    //options.ValueLengthLimit = int.MaxValue;
}
);;



var app = builder.Build();

// Resolve the service to call the method
using (var scope = app.Services.CreateScope())
{
    var ffmpegService = scope.ServiceProvider.GetRequiredService<FfmpegService>();
    await ffmpegService.DownloadFFmpeg();
    ffmpegService.SetFFmpegPermissions();
}

app.MapGroup("/api/identity").MapIdentityApi<ApplicationUser>();

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
