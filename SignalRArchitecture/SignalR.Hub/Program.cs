using SignalR.HubServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configuración de CORS para permitir la comunicación segura con el cliente
builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy
        .WithOrigins("https://localhost:7002") // Puerto predeterminado del cliente web del taller
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Registrar los servicios de SignalR en el contenedor
builder.Services.AddSignalR();

var app = builder.Build();

// Configuración del pipeline HTTP
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("ClientPolicy");

// Mapear el Hub de Pedidos con su ruta correspondiente
app.MapHub<OrderHub>("/orderHub"); // <-- Cambiado de /chatHub a /orderHub

app.Run();