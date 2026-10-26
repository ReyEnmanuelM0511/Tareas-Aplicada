"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7001/orderHub")
    .withAutomaticReconnect()
    .build();

const updateButton = document.getElementById("updateButton");
const status = document.getElementById("status");
const ordersList = document.getElementById("ordersList");

connection.on("ReceiveOrderUpdate", (order) => {
    const item = document.createElement("li");

    // Corregido: 'actualizado' (tal como viene del record C#)
    const fechaHora = order.actualizado ? new Date(order.actualizado) : new Date();
    const hora = fechaHora.toLocaleTimeString("es-DO", {
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit"
    });

    // Mapeo del número de enum que devuelve el servidor a texto legible para la interfaz
    const textosEstados = ["Pendiente", "En Preparación", "Enviado", "Entregado"];
    const estadoTexto = textosEstados[order.estado] ?? "Desconocido";

    item.textContent = `[${hora}] Pedido #${order.orderId} (${order.cliente}) - Estado: ${estadoTexto}`;
    ordersList.appendChild(item);
});

connection.onreconnecting(() => {
    status.textContent = "Reconectando con el servidor de pedidos...";
    updateButton.disabled = true;
});

connection.onreconnected(() => {
    status.textContent = "Conectado";
    updateButton.disabled = false;
});

async function startConnection() {
    try {
        await connection.start();
        status.textContent = "Conectado";
        updateButton.disabled = false;
    } catch (error) {
        status.textContent = "Error de conexion";
        console.error(error);
    }
}

updateButton.addEventListener("click", async () => {
    const orderId = document.getElementById("orderIdInput").value.trim();
    const cliente = document.getElementById("clientInput").value.trim();
    const estadoSelect = document.getElementById("statusSelect").value;

    if (!orderId || !cliente || estadoSelect === "") {
        alert("Completa todos los datos del pedido.");
        return;
    }

    if (connection.state !== signalR.HubConnectionState.Connected) {
        alert("No existe una conexion activa");
        return;
    }

    try {
        // Invocamos enviando el estado como número entero (0, 1, 2, 3)
        await connection.invoke("UpdateOrderStatus", parseInt(orderId, 10), cliente, parseInt(estadoSelect, 10));

        document.getElementById("orderIdInput").value = "";
    } catch (error) {
        console.error("No se pudo actualizar el estado del pedido: ", error);
    }
});

startConnection();