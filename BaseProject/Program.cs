using Marketplace;
using Marketplace.Repositories;
using Marketplace.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using ViewModels.ants;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
builder.Services.AddCustomService();
builder.Services.AddAutoMapper(typeof(AutoMapConfig));

// ngambil token management dari appseting.json
builder.Services.Configure<JwtModel>(builder.Configuration.GetSection("TokenManagement"));
var token = builder.Configuration.GetSection("TokenManagement").Get<JwtModel>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidIssuer = token.Issuer,
            ValidAudience = token.Audience,
        };
    });

var app = builder.Build();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
context.Database.EnsureCreated();

app.UseExceptionHandler(options =>
{
    new ExceptionHandlerOptions()
    {
        AllowStatusCode404Response = true, // important!
        ExceptionHandlingPath = "/Home/Error"
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "API");

    });

}
app.UseSwaggerAuthorized();
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
