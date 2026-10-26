using System;
using System.Collections.Generic;
using System.Text;

namespace SignalR.Shared.Contracts;

public enum EstadoPedido { Pendiente, EnPreparacion, Enviado, Entregado }

public sealed record OrderNotification(int OrderId, string Cliente, EstadoPedido Estado, DateTimeOffset Actualizado);
