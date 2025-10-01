using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OrderingServiceData;
using OrderingServiceData.DataAccess;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using OrderingServiceEngine;
using OrderingServiceEngine.Managers;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceWeb;
using OrderingServiceWeb.Helpers;
using OrderingServiceWeb.Helpers.Abstractions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderingServiceDbContext>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.SaveToken = true;
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

//Data Access
builder.Services.AddScoped<IApplicationLogDataAccess, ApplicationLogDataAccess>();
builder.Services.AddScoped<ICustomerDataAccess, CustomerDataAccess>();
builder.Services.AddScoped<IItemDataAccess, ItemDataAccess>();
builder.Services.AddScoped<IOrderDataAccess, OrderDataAccess>();

//Managers
builder.Services.AddScoped<IApplicationLogManager, ApplicationLogManager>();
builder.Services.AddScoped<ICustomerManager, CustomerManager>();
builder.Services.AddScoped<IItemManager, ItemManager>();
builder.Services.AddScoped<IOrderManager, OrderManager>();

//Helpers
builder.Services.AddScoped<ISecurityHelper, SecurityHelper>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


//DEFAULT CUSTOMERS


using (var scope = app.Services.CreateScope())
{
    IServiceProvider service = scope.ServiceProvider;
    OrderingServiceDbContext context = service.GetService<OrderingServiceDbContext>()!;

    if (context.Customers.Count() == 0)
    {
        context.Customers.Add(new Customer()
        {
            Name="Antonio",
            Email= "semaan.antonio@hotmail.com",
            PasswordHash = CustomerManager.Sha256Hash("123123"),
        });
        context.SaveChanges();
    }

    if (context.Items.Count() == 0)
    {
        context.Items.Add(new Item()
        {
            Name="Table",
            Price= 12.5,
        });

        context.Items.Add(new Item()
        {
            Name="Chair",
            Price= 7.25,
        });

        context.Items.Add(new Item()
        {
            Name="Tent",
            Price= 30,
        });

        context.SaveChanges();
    }

}


app.Run();
