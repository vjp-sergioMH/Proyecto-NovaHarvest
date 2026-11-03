### CUESTIONES UT2

1. **Cliente y servidor.** Indica qué se ejecuta en el **cliente** y qué en el **servidor** cuando abres `/status`. Cuando NovaHarvest permita reservar producto, la regla *"no se pueden reservar más kilos de los disponibles"*, ¿dónde debe comprobarse obligatoriamente? ¿Por qué?

   Al abrir `/status` el navegador actua como cliente y realiza una petición HTTP al servidor. En el servidor ASP.NET Core usa Kestrel para recibir la petición y ejecutar el código mapeado en program.cs. En cuanto a reservar kilos, deberá comprobarse en el servidor obligatoriamente, ya que será la fuente fiable sobre las cantidades disponibles (stock) y no permitirá reservas incorrectas.

2. **Estático o dinámico.** ¿Cuál de tus tres páginas podría ser un HTML estático y cuál no? Justifícalo. Pon dos ejemplos de páginas que NovaHarvest necesitará y que **no** podrían ser estáticas.

   `/`(Inicio) y `/about`(Acerca de) podrían ser HTML estáticos ya que muestra información fija. Sin embargo, `/status` muestra la fecha y la hora actuales del servidor, que cambian continuamente.

   En cuanto a páginas estáticas podrían ser aquellas en las que se puedan comprar o hacer reservas de productos, ya que se deberán mostrar la disponibilidad en tiempo real.

   Y por último, las páginas estáticas podrían ser las que ofrezcan información acerca de la empresa, como sus contactos o información.

3. **Ejecución en el servidor.** Explica paso a paso cómo una petición a `http://localhost:8080/status` acaba ejecutando tu código C\#. ¿Qué cambiaría si NovaHarvest funcionara con **CGI**? ¿Y con un **módulo embebido** en el servidor web?

   Cuando accedo a `http://localhost:8080/status`, el navegador envía una petición HTTP a nginx, que está escuchando en el puerto 80 de su contenedor, publicado como puerto 8080 en mi equipo. nginx recibe la petición y, gracias a la directiva proxy\_pass de nginx.conf, la reenvía a `http://backend:8080`. Docker permite localizar el servicio mediante el nombre backend dentro de su red interna. La petición llega a Kestrel, que recibe la solicitud y busca la ruta /status definida mediante MapGet en Program.cs. Al encontrarla, ejecuta la función C\#, genera el HTML con la fecha y hora actuales y devuelve la respuesta a nginx, que finalmente se la entrega al navegador.

   Si NovaHarvest utilizara CGI, el servidor web crearía un proceso externo nuevo por cada petición, ejecutaría el programa y recibiría el HTML generado mediante la salida estándar.

   Con un módulo embebido, el intérprete estaría integrado en el propio servidor web, evitando crear procesos nuevos.

4. **Reverse proxy.** Haz `curl -I` (o mira la pestaña Red del navegador) a `/status` con Docker y con `dotnet run`. ¿Qué cabecera `Server` aparece en cada caso y por qué? Para el backend (`docker compose stop backend`) y pide `/status` y `/css/site.css`: ¿qué código HTTP recibes y por qué? ¿Por qué `backend` no publica ningún puerto?

   En mi proyecto, al ejecutar `dotnet run`, Kestrel atiende directamente las peticiones y la cabecera es Server: Kestrel. Cuando utilizo Docker, la petición pasa primero por nginx, que actúa como reverse proxy, por lo que el navegador recibe Server: nginx/1.27.x. Esto ocurre porque nginx oculta por defecto la cabecera Server del servidor al que reenvía la petición y utiliza su propia identificación.

   Al ejecutar `docker compose stop backend` y solicitar `/status`  y `/css/estilos.css`, nginx devolverá un error 502 Bad Gateway, porque no podrá comunicarse con el servidor de aplicaciones. En mi proyecto el backend no publica ningún puerto porque solamente necesita recibir peticiones desde nginx a través de la red interna de Docker, evitando exponerlo directamente al exterior.

5. **nginx.** ¿Por qué nginx no sirve `site.css` ni `logo.png` por su cuenta? Cita dos funciones que nginx podría aportar si NovaHarvest se publicara en Internet.

   nginx no sirve directamente `estilos.css` ni `logo.png` porque en mi proyecto está configurado únicamente como reverse proxy. Los archivos estáticos se encuentran dentro de `backend/NovaHarvest.Web/wwwroot/`, y ASP.NET Core los sirve gracias a `app.MapStaticAssets()`, definido en Program.cs. Por tanto, nginx no necesita almacenar ni conocer los archivos de la aplicación.

   Si NovaHarvest se publicara en Internet, nginx podría encargarse de gestionar las conexiones HTTP.

6. **Tecnologías.** Si la cooperativa pidiera hacer el backend con otra de las tecnologías *server-side* vistas en clase, ¿qué ficheros de tu repositorio cambiarían y cuáles se quedarían igual?

   Si la cooperativa decidiera desarrollar NovaHarvest utilizando otra tecnología server-side, tendría que modificar principalmente los archivos del backend. Tendría que sustituir `Program.cs` y el archivo `.csproj` por los archivos correspondientes al lenguaje y framework elegido, además de adaptar el Dockerfile para instalar las dependencias y ejecutar esa tecnología.

   Sin embargo, podría mantener la estructura general del proyecto, el servicio `web`, su `Dockerfile` y la configuración de `nginx.conf`. El archivo `compose.yaml` también podría conservarse si se mantiene el nombre del servicio backend y el puerto interno 8080\.

7. **Marcado y código.** Copia de tu `Program.cs` el código de una de las páginas y señala **dos** problemas concretos de generar HTML a base de cadenas. ¿Qué parte se repite en las tres páginas?

 ```csharp
   app.MapGet("/status", () => Results.Content(
$"""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>NovaHarvest - Estado</title>
    <link rel="stylesheet" href="/css/estilos.css">
</head>
<body>
    <header>
        <img src="/img/logo.png" alt="Logotipo de NovaHarvest">
        <span>NovaHarvest</span>
        <nav>
            <a href="/">Inicio</a>
            <a href="/about">Acerca de</a>
            <a href="/status">Estado del servidor</a>
        </nav>
    </header>

    <main>
        <h1>Estado</h1>
        <p>Fecha y hora del servidor: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
        <p>Página generada por ASP.NET Core (Kestrel)</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · 2026
    </footer>
</body>
</html>
""",
"text/html"));
```

   El primero es que dificulta la lectura y el mantenimiento del código, porque el marcado HTML queda mezclado con el código C\# y las cadenas de texto. El segundo es que resulta fácil cometer errores al escribir etiquetas o comillas, y el editor no ofrece las mismas ayudas que cuando trabajamos directamente con archivos HTML.

   Además, en mis tres páginas se repite gran parte de la estructura: la cabecera con el logotipo, el menú de navegación, la referencia al CSS y el pie de página.

8. **Docker.** ¿Cuántas imágenes y cuántos contenedores tienes con el entorno levantado? Explica la diferencia usando tu proyecto. ¿Por qué la imagen final del backend no incluye el SDK? ¿Qué pasaría sin el `.dockerignore`?

   Con el entorno de NovaHarvest levantado tengo dos imágenes propias de los servicios y dos contenedores en ejecución. Una imagen es una plantilla que contiene la aplicación y sus dependencias, mientras que un contenedor es una instancia en ejecución creada a partir de esa imagen. En mi proyecto, una imagen corresponde a nginx y otra al backend ASP.NET Core; sus respectivos contenedores ejecutan ambos servicios y se comunican mediante la red interna de Docker.

   La imagen final del backend no incluye el SDK porque mi Dockerfile utiliza en la primera etapa `mcr.microsoft.com/dotnet/sdk:10.0` para compilar y publicar la aplicación, mientras que en la segunda se utiliza `mcr.microsoft.com/dotnet/aspnet:10.0`, que solamente contiene el entorno necesario para ejecutarla.

   Sin el archivo `.dockerignore`, Docker copiaría también las carpetas bin y obj.

9. **Herramientas.** Haz una tabla con las herramientas que has usado en el proyecto y el problema que resuelve cada una en él.

| Herramienta | Problema que resuelve en NovaHarvest |
| :---- | :---- |
| Visual Studio Code / IDE | Permite escribir y organizar el código C\# y los archivos de configuración. |
| C\# y ASP.NET Core | Permiten desarrollar la aplicación web y generar las páginas dinámicas. |
| .NET 10 SDK | Permite compilar, ejecutar y publicar el backend mediante `dotnet run` y `dotnet publish`. |
| Kestrel | Recibe las peticiones HTTP y ejecuta el código C\# de la aplicación. |
| Docker | Empaqueta la aplicación y sus dependencias en contenedores para disponer de un entorno reproducible. |
| Docker Compose | Permite levantar y gestionar conjuntamente los servicios web y backend. |
| nginx | Actúa como reverse proxy y proporciona un único punto de entrada a la aplicación. |
| Navegador / curl | Permiten comprobar las páginas y examinar las peticiones, respuestas y cabeceras HTTP. |
| Git | Permite controlar las versiones del proyecto y conservar el historial de modificaciones. |

10. **Observación.** Compara la hora de `/status` con `dotnet run` y con Docker. ¿Coinciden? Explica por qué.

   Hay diferencia de 1 hora entre ambos, con dotnet run, utiliza la zona horaria configurada en mi ordenador, mientras que con Docker utiliza la zona horaria del contenedor. En mi configuración no he establecido ninguna zona horaria específica para el backend, por lo que el contenedor Linux normalmente utiliza UTC. Por este motivo, la hora mostrada en Docker es hora menos que la obtenida mediante dotnet run.
