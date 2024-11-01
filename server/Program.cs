var builder = WebApplication.CreateBuilder(args);

var apiKey = builder.Configuration["MySettings:GptKey"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("gptClient", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/chat", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("gptClient");

    var payload = new
    {
        model = "gpt-4o-mini",
        messages = new[]
        {
            new { role = "user", content = "who are you?" }
        }
    };

    var result = await client.PostAsJsonAsync(
        "v1/chat/completions",
        payload
    );

    if (!result.IsSuccessStatusCode)
    {
        var error = await result.Content.ReadAsStringAsync();
        return Results.Problem($"API Error: {result.StatusCode} - {error}");
    }

    var content = await result.Content.ReadAsStringAsync();
    return Results.Content(content, "application/json");
})
.WithName("chat")
.WithOpenApi();

await app.RunAsync();
