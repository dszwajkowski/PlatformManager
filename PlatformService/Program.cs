using Microsoft.EntityFrameworkCore;
using PlatformService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var serviceScope = app.Services.CreateScope())
    {
        var dbcontext = serviceScope.ServiceProvider.GetService<AppDbContext>();
        ArgumentNullException.ThrowIfNull(dbcontext, nameof(dbcontext));
        AppDbContextSeed.SeedData(dbcontext);
    }
}

// temporarily disable https redirections
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
