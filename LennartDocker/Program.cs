using System.Net;
using System.Net.Sockets;

using Microsoft.Win32;

//docker build --force-rm -t lennartdocker:1.0 -f .\LennartDocker\Dockerfile .
//docker run --rm -it -p 12000:80 lennartdocker:1.0

//docker build --force-rm -t lennartdocker:latest -f .\LennartDocker\Dockerfile .
//docker tag lennartdocker:latest registry:5000/lennartdocker:1.1
//docker push registry:5000/lennartdocker:1.1
//helm upgrade --install lennartdocker lennartdockerdeploy --create-namespace --set Image=registry:5000/lennartdocker:1.1 --kubeconfig="${env:USERPROFILE}\.kube\config.ubk3s"
//curl.exe http://lennartdocker.ubk3s/weatherforecast


//docker build --force-rm -t lennartdocker:latest -f .\LennartDocker\Dockerfile .
//docker tag lennartdocker:latest registry.ubk3s:5000/lennartdocker:1.1
//docker push registry.ubk3s:5000/lennartdocker:1.1
//helm upgrade --install lennartdocker lennartdockerdeploy --create-namespace --set Image=registry:5000/lennartdocker:1.1
//curl.exe http://lennartdocker.ubk3s/weatherforecast

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
  _ = builder.WebHost.UseKestrel(options =>
  {
    options.Listen(IPAddress.Any, 80);
  });
}
else
{
  _ = builder.WebHost.UseKestrel(options =>
  {
    options.Listen(IPAddress.Any, 80);
  });
}
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
  var forecast = Enumerable.Range(1, 5).Select(index =>
      new WeatherForecast
      (
          DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
          Random.Shared.Next(-20, 55),
          summaries[Random.Shared.Next(summaries.Length)]
      ))
      .ToArray();
  return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
