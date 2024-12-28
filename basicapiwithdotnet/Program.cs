var builder = WebApplication.CreateBuilder(args);
// add controller
builder.Services.AddControllers(); // adds user defined controllers

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add Cors to FE
builder.Services.AddCors((options) => {
    options.AddPolicy("DevCors", (corsBuilder) =>
        {
            corsBuilder.WithOrigins("http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        }
    );

    // allow in prod
    options.AddPolicy("prodCors", (corsBuilder) =>
        {
            corsBuilder.WithOrigins("https://newProductionSite.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("prodCors");
    app.UseHttpsRedirection();
}

app.MapControllers();
// app.MapGet("/weatherforecast", () =>{}).WithName("GetWeatherForecast");
app.Run();

