using System.Text.Json.Serialization;
using GptClient.Models;
using GPTClient.RequestHandlers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

var apiKey = builder.Configuration["MySettings:GptKey"];

builder.Logging.AddConsole(); // Add console logging

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "GptClient API", Version = "v1" });

    // Add a schema filter to enforce enums as strings in Swagger UI
    options.SchemaFilter<EnumSchemaFilter>();
});

builder.Services.AddHttpClient("gptClient", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
});

builder.Services.AddTransient<IGptChatHandler, GptChatHandler>();
builder.Services.AddTransient<IGptChatApiClient, GptChatApiClient>();
builder.Services.AddTransient<IContextHandler, ContextHandler>();

 builder.Services.AddCors();
 

var app = builder.Build();

app.UseCors(builder => builder
 .AllowAnyOrigin()
 .AllowAnyMethod()
 .AllowAnyHeader()
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/chat", async (GptRequest gptRequest, IGptChatHandler chatHandler) =>
{
    var content = await chatHandler.Chat(gptRequest);

    return Results.Content(content, "application/json");
})
.WithName("chat")
.WithOpenApi();

await app.RunAsync();

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum = Enum.GetNames(context.Type)
                              .Select(n => new OpenApiString(n))
                              .ToList<IOpenApiAny>();
            schema.Type = "string";
        }
    }
}