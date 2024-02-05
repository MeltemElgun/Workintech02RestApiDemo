using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using Workintech02RestApiDemo;
using Workintech02RestApiDemo.Business;
using Workintech02RestApiDemo.Business.Blog;
using Workintech02RestApiDemo.Business.City;
using Workintech02RestApiDemo.Business.Currency;
using Workintech02RestApiDemo.Infrastructure;
using Workintech02RestApiDemo.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); //sonsuz döngüye girmeyi engelle
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//db resolve and initialize
builder.Services.AddDbContext<WorkintechBlogDemoContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("WorkintechBlogDemo")));
builder.Services.AddDbContext<Workintech02CodeFirstContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("WorkintechCodeFirstDemo")));


//add authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("SecretKey"))),
        };
    });

#region currency service resolver


/*builder.Services.AddSingleton<ICurrencyService,CurrencyService>(); //proje request yapýdýðýnda
builder.Services.AddScoped<ICurrencyService,CurrencyService>(); //peoje proje ilk çalýþtýðýnda
builder.Services.AddTransient<ICurrencyService, CurrencyService>(); //proje tran iþlemler  yapýdýðýnda
*/
#endregion

#region Scrutor resolvers

var typeBaseService = typeof(BaseService);

var assembly = typeBaseService.Assembly;

builder.Services.Scan(selector =>
        selector
            .FromAssemblies(assembly)
            .AddClasses(classSelector => classSelector.AssignableTo(typeof(BaseService)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );



var singletonBaseAssembly = typeof(BaseSingletonService).Assembly;
builder.Services.Scan(selector =>
        selector
            .FromAssemblies(singletonBaseAssembly)
            .AddClasses(classSelector => classSelector.AssignableTo(typeof(BaseSingletonService)))
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );

#endregion

var app = builder.Build();




app.UseTimeElapsedCalculate();



//Debug-->Information-->Warning-->Error-->Fatal
#region SerilogConfiguration

var logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Information()
#endif
#if RELEASE
.MinimumLevel.Error()
#endif
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Logger = logger;

#endregion


// Configure the HTTP request p
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCustomException();

DataSeeder.SeedCodeFirst(app);
DataSeeder.Seed(app);

app.Run();
