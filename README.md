# reto_Guardians
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
2. Configurar la base de datos con los scripts que encontrarás en la carpeta `Scripts`
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
##### Put
Se requerirá el id del objeto desde la ruta por ejemplo al querer modificar una agenda se haría lo siguiente: `api/Agendas/ModificarAgenda/5`. Y en el cuerpo del request deberán ir los campos a modificar, el resto de los campos deberán quedar vacíos, el método solo tomará aquellos valores que sean diferentes de null, para actualizarlos dentro de la base de datos.
##### Post
Las solicitudes post dependen exclusivamente del body sin pedir información adicional, los campos vacíos tomarán valores por defecto asignados dentro del código.

# reto_Guardians
Web application for Guardians of the Globe's data managment using technologies such as ASP.NET Core and SQL Server.
## Initial Configuration:
  - .NET SDK installed
  - SQL Server installed
## Configuration steps:
1. Cloning repository:
    ```bash
   git clone https://github.com/Derjai/reto_Guardians
   cd reto_Guardians
    ```
2. Create and configurate the database with the scripts provided on the `Scripts` folder
3. Update the connection string in `appsetings.json` with the information about your SQL Server instance:
   ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Initial Catalog=yourdatabasename;*otherconfigurations*"
    }
   ```
## Running the proyect:
1. Go to the proyect directory and run de application:
```bash
cd src/reto_Guardians
dotnet run
```
The application will be available on `https://localhost:7270`
### Requests usage:
Since the app doesn't implement views, to try and run methods you should use swagger, to which you'll be redirected as soon as the program starts running otherwise just type `/swagger/index.html` at the end of the link.
#### Get and Delete
Get and Delete methods, can be used from the links available for each one, for example a request to get all the agendas of an existing hero would be the following: 
`https://localhost:7270/api/Agendas/BuscarAgendaAlias/{alias}` in which the last field {alias} must be replace for the alias of an existing hero
#### Post and Put
Post and Put methods, must be executed from a api testing tool, since they depend on a request body that contains the information about the new object or the information that is going to change about it.
##### Put
The object id would be requested from the route for example, to edit an existing event in the agenda: `api/Agendas/ModificarAgenda/5`. And in the request body the you should edit the fields to modify, the others must be empty, since the method would only take in consideration every value that differs from null
##### Post
Post request depend exclusively on the request body, empty fields will be replaced by default values previously stated in the code.
   
