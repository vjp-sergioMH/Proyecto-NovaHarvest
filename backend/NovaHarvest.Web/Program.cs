var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapStaticAssets();

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>NovaHarvest - Inicio</title>
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
        <h1>Inicio</h1>
        <p>En el Valle del Jerte, una cooperativa agraria y sus productores tienen cada semana fruta, verdura y otros productos que no llegan a venderse: cereza que madura a la vez, tomate que no aguanta el transporte, calabacín que sobra de una cosecha. Parte acaba en la basura.</p>
        <p>Al otro lado hay compradores que podrían aprovecharlo: residencias, comedores sociales, restaurantes y particulares. Lo que falta es una herramienta que los conecte.</p>
        <p>Está dirigido a agricultores y profesionales del sector agrícola.</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · 2026
    </footer>
</body>
</html>
""", "text/html"));

app.MapGet("/about", () => Results.Content("""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>NovaHarvest - Acerca de</title>
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
        <h1>Acerca de</h1>
        <p>Proyecto: NovaHarvest</p>
        <p>Centro educativo: IES Valle del Jerte</p>
        <p>Autor: Sergio Molano Hisado</p>
        <p>Versión: 0.1.0</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · 2026
    </footer>
</body>
</html>
""", "text/html"));


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

app.Run();
