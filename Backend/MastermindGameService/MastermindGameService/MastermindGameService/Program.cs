using MastermindGameService.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration["ConnectionString"];

string dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
string dbUser = Environment.GetEnvironmentVariable("DB_USER");
string dbName = Environment.GetEnvironmentVariable("DB_NAME");
string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
string jwtkey = Environment.GetEnvironmentVariable("JWT_KEY");

if (dbServer != null && dbUser != null && dbName != null && dbPassword != null)
{
    connectionString = $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";

    builder.Services.AddDbContext<MastermindGameDBContext>(options =>
    options.UseSqlServer(connectionString));
}
else
{
    Console.WriteLine("Error: Environment variables are not set correctly");
}

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddDbContext<MastermindGameDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameDB"));
});*/

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey))
    };
    options.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();
            c.Response.StatusCode = 403;
            c.Response.ContentType = "text/plain";
            return c.Response.WriteAsync(c.Exception.ToString());
        },
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello from AWS Lambda!");
app.Run();