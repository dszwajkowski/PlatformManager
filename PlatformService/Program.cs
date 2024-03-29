using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.MessageBus;
using PlatformService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("Starting production environment.");
    var connectionString = builder.Configuration.GetConnectionString("PlatformsDb") 
            ?? throw new NullReferenceException("Connection string for production environment not specified."); 
    Console.WriteLine($"Using connection string: {connectionString}");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
}
else
{
    Console.WriteLine("Starting development environment.");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
}
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();


var app = builder.Build();

// Configure the HTTP request pipeline.

using (var serviceScope = app.Services.CreateScope())
{
    var dbcontext = serviceScope.ServiceProvider.GetService<AppDbContext>();
    ArgumentNullException.ThrowIfNull(dbcontext, nameof(dbcontext));
    
    if (app.Environment.IsProduction())
    {
        dbcontext.Database.Migrate();
    }

    AppDbContextSeed.SeedData(dbcontext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async ctx =>
{
    await ctx.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});


// temporarily disable https redirections
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
