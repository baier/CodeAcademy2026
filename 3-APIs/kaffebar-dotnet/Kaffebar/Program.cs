using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Registrer OpenAPI-tjenestene. Disse genererer spesifikasjonen
// automatisk basert på endepunktene og typene i prosjektet (code-first).
builder.Services.AddOpenApi();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Aktiverer validering for Minimal APIs (.NET 10) - gjør at .WithValidation() fungerer
builder.Services.AddValidation();


var app = builder.Build();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    // Eksponerer den genererte spesifikasjonen på /openapi/v1.json
    app.MapOpenApi();

    // Scalar gir et moderne, interaktivt UI på /scalar/v1
    // for å utforske og teste API-et.
    app.MapScalarApiReference();
}

// --- Hello Coffee --------------------------------------------------------
// Et minimal API-endepunkt som returnerer en hardkodet kaffemeny.
// Dette er utgangspunktet for workshopen. Bygg videre herfra!

var menu = new List<Coffee>
{
    new Coffee(Guid.Parse("c3d12ef1-e8cb-4162-993d-7172240a8e4d"), "Kaffe Latte", 48.50m),
    new Coffee(Guid.Parse("d5a9b1f2-2a1a-4f3b-9b2e-1a2b3c4d5e6f"), "Cappuccino", 45.00m),
    new Coffee(Guid.Parse("a1b2c3d4-e5f6-4789-abcd-0123456789ab"), "Espresso", 35.00m)
};

app.MapGet("/menu", () => menu)
.WithName("GetMenu")
.WithSummary("Hent kaffemeny")
.WithDescription("Returnerer en liste over alle tilgjengelige kaffedrikker i kaffebaren.");

/*
// Eksempel på Minimal API som bruker automatisk validering:
app.MapPost("/orders", (CreateOrderRequest req) =>
{
    var orderId = Guid.NewGuid();
    var created = new CreateOrderResponse(orderId, req.CoffeeId, DateTime.UtcNow);
    return Results.Created($"/orders/{orderId}", created);
})
.WithValidation();
*/

app.Run();
