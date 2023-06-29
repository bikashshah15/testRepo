
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TestWeb.API.DataContext;
using TestWeb.API.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//added
builder.Services.AddSwaggerGen( c =>
{
   
    c.AddSecurityDefinition("JWT Bearer", new OpenApiSecurityScheme
    {
        Description = "This is a JWT bearer authentication scheme",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "JWT Bearer",
                    Type = ReferenceType.SecurityScheme
                    
                }
            },
            new List<string> ()
        }
    });
        
        
   
});

builder.Services.AddDbContext<EmployeeDbContext>(options =>
 {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddTransient<TokenHelper>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration.GetValue<string>("JWTToken:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("JWTToken:Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration.GetValue<string>("JWTToken:SecretKey")))
    };
});


builder.Services.AddAuthorization(); //add this

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test JWT"); //added remove c as well
    });
}

app.UseHttpsRedirection();
app.UseAuthentication(); //added
app.UseAuthorization();

app.MapControllers();

app.Run();
