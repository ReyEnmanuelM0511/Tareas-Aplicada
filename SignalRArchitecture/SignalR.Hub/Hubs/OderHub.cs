using Microsoft.AspNetCore.SignalR;
using SignalR.Shared.Contracts;

namespace SignalR.HubServer.Hubs;

public sealed class OrderHub : Hub
{
    public async Task UpdateOrderStatus(int orderId, string cliente, EstadoPedido estado)
    {
        if (orderId <= 0)
        {
            throw new HubException("El ID del pedido debe ser válido.");
        }

        if (string.IsNullOrWhiteSpace(cliente))
        {
            throw new HubException("El nombre del cliente es obligatorio.");
        }

        cliente = cliente.Trim();

        if (cliente.Length > 40)
        {
            throw new HubException("El nombre del cliente supera el límite permitido.");
        }

        var orderNotification = new OrderNotification(
            orderId,
            cliente,
            estado,
            DateTimeOffset.UtcNow
        );

        await Clients.All.SendAsync("ReceiveOrderUpdate", orderNotification);
    }
}