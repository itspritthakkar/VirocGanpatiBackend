using VirocGanpati.Extensions;
using VirocGanpati.Seed;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
builder.Services.ConfigureAppSettings(builder.Configuration);

// Register services
builder.Services.RegisterDatabase(builder.Configuration);
builder.Services.RegisterApplicationServices();
builder.Services.RegisterJwtAuthentication(builder.Configuration);
builder.Services.RegisterSwagger();
builder.Services.RegisterAutoMapper();
builder.Services.AddHttpClient();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
        policy.WithOrigins("http://localhost:3000", "https://swasurvey.in")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();

var app = builder.Build();

// SeedAsync database
await SeedData.SeedAsync(app.Services);

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.UseCustomMiddleware();

app.MapControllers();

app.Run();
