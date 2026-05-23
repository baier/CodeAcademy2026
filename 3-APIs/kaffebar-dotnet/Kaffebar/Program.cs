using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Registrer OpenAPI-tjenestene. Disse genererer spesifikasjonen
// automatisk basert på endepunktene og typene i prosjektet (code-first).
builder.Services.AddOpenApi();

var app = builder.Build();

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

app.MapPost("/orders", (CreateOrderRequest request) =>
{
    var orderId = Guid.NewGuid();
    var created = new CreateOrderResponse(orderId, request.CoffeeId, DateTime.UtcNow);
    var location = $"/orders/{orderId}";
    return TypedResults.Created(location, created);
})
.WithName("CreateOrder")
.WithSummary("Opprett en ny bestilling")
.WithDescription("Oppretter en ny bestilling for valgt kaffedrikk.");

app.Run();

// DTO-er kan ligge i Program.cs når prosjektet er lite.
// Etter hvert er det ryddig å flytte dem til egne filer i en Models-mappe.
public record Coffee(Guid Id, string Name, decimal Price);

public record CreateOrderRequest(Guid CoffeeId);

public record CreateOrderResponse(Guid OrderId, Guid CoffeeId, DateTime CreatedAt);
