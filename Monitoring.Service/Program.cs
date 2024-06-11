var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecksUI(settings =>{
    settings.AddHealthCheckEndpoint("Service A", "https://localhost:7079/health");
    settings.AddHealthCheckEndpoint("Service B", "https://localhost:7102/health");
    settings.SetEvaluationTimeInSeconds(3);
    settings.SetApiMaxActiveRequests(3);
    settings.ConfigureApiEndpointHttpclient((serviceProvider, httpClient) =>
    {
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "...");
    }).ConfigureWebhooksEndpointHttpclient((serviceProvice,httpClient) =>
    {
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "...");
    });
}).AddSqlServerStorage("Server=.;Database=HealthCheckUIDb;Trusted_Connection=True;TrustServerCertificate=True;");

var app = builder.Build();

app.UseHealthChecksUI(options => options.UIPath = "/health-ui");

app.Run();
