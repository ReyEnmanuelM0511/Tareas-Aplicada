# Registro-De-Libros

# Reto Independiente: Pedidos en tiempo Real
## Guía de Pasos de Ejecución: Sistema de Pedidos en Tiempo Real con SignalR
### Prerrequisitos

Tener instalado .NET SDK (versión 8.0 o superior).

Tener configurado el entorno de desarrollo (Visual Studio, VS Code o terminal con PowerShell).

### Paso 1: Clonar o abrir la solución

Abrir la terminal o el explorador de archivos en la carpeta raíz del proyecto (SignalRArchitecture).

Verificar que la solución contenga los tres proyectos estructurados de manera modular:

  SignalR.Shared (Contratos y enums compartidos)

  SignalR.Hub (Servidor de SignalR / Backend)

  SignalR.Client (Cliente web Razor / Frontend)

### Paso 2: Iniciar el Servidor de SignalR (SignalR.Hub)

Abrir una terminal exclusiva para el servidor.

Ejecutar el comando para iniciar el proyecto apuntando explícitamente al puerto asignado (7001):
    PowerShell

dotnet run --project SignalR.Hub --urls https://localhost:7001

Verificar en la consola que el servidor indique que está escuchando en las URLs configuradas.

### Paso 3: Iniciar el Cliente Web (SignalR.Client)

Abrir una segunda terminal (separada de la del servidor).

Ejecutar el comando para iniciar el proyecto del cliente en su respectivo puerto (7002):
  PowerShell

    dotnet run --project SignalR.Client --urls https://localhost:7002

### Paso 4: Pruebas de funcionamiento en tiempo real

Abrir el navegador web de preferencia (Chrome, Firefox, Edge).

Ingresar a la URL del cliente: https://localhost:7002.

Verificar que el indicador de estado cambie automáticamente a "Conectado" (lo que confirma que la conexión con el Hub en el puerto 7001 fue exitoso).

Prueba multi-pestaña: Abrir la misma dirección (https://localhost:7002) en una segunda pestaña o ventana del navegador en paralelo.

Introducir un ID de pedido, un nombre de cliente y seleccionar un estado en cualquiera de las pestañas, luego hacer clic en "Actualizar Estado".

 Comprobar que la notificación se refleja instantáneamente en todas las pestañas abiertas de forma sincronizada y sin necesidad de recargar la página web.
