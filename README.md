# reto_Guardians
Web application for Guardians of the Globe's data managment using technologies such as ASP.NET Core and SQL Server.
Aplicación web para manejar los datos de Guardians of The Globe, implementa tecnologías como ASP.NET Core y SQL Server.
## Configuración inicial:
  - .NET SDK instalado
  - SQL Server instalado
## Pasos de configuración:
1. Clonar repositorio:
   ```bash
   git clone https://github.com/Derjai/reto_Guardians
   cd reto_Guardians
    ```
2. Configurar la base de datos con el script de creación que encontrarás en
3. Actualizar la cadena de conexión de `appsetings.json` con la información de tu instancia de SQL Server:
   ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Initial Catalog=NombreDeTuBaseDeDatos;*demás configuraciones*"
    }
   ```
## Ejecución del proyecto:
1. Navegar al directorio del proyecto y ejecutar la aplicación:
```bash
cd src/reto_Guardians
dotnet run
```
La aplicación estará disponible en `https://localhost:7270`
### Utilización de las peticiones:
Debido a que la aplicación no cuenta con vistas, para probar y ejecutar los métodos se recomienda utilizar swagger al que será redirigido una vez ejecutar el programa de caso contrario basta con escribir `/swagger/index.html` al final del enlace. Una vez en swagger se pueden realizar la ejecución de los métodos.
#### Get y Delete

Los métodos get, y delete, pueden utilizarse desde los enlaces de cada uno, por ejemplo el método para conseguir todas las agendas de un heroe en particular sería el siguiente: `https://localhost:7270/api/Agendas/BuscarAgendaAlias/{alias}` donde este ultimo campo sería reemplazado por el alias de un heroe existente. 
#### Post y Put
Los métodos post y put, deben ser ejecutados desde una herramienta para probar apis, ya que estos requieren de un cuerpo que contenga la información del nuevo objeto o la información a editar de este mismo.
