
Indicaciones para AppSettings.json siguiendo las buenas practicas de desarrollo seguro para proyectos:

Cuando se realice el comando git pull origin master, se encontrara con un archivo llamado appsettings.example.json, habra de ser necesario cambiar el nombre del mismo a solo: appsettings.json
---
---	y se debe agregar las credenciales que dispongas en los campos correspondientes:
---
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=TU_HOST; Database=TU_DB; Username=TU_USER; Password=TU_PASSWORD; SSL Mode=VerifyFull; Channel Binding=Require;"
  },
  "Jwt": {
    "Key": "TU_CLAVE_JWT_AQUI",
    "Issuer": "MiProyecto",
    "Audience": "MiProyectoUsers",
    "ExpirationHours": 8
  },
  "PayPal": {
    "ClientId": "TU_PAYPAL_CLIENT_ID",
    "ClientSecret": "TU_PAYPAL_CLIENT_SECRET",
    "ReturnUrl": "http://localhost:4200/donaciones/confirmacion",
    "CancelUrl": "http://localhost:4200/donaciones/cancelado"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

---
---	Después de configurar el appsettings.json, se debe correr en la consola de NuGet: dotnet ef database update
--- 	- Esto aplicará las nuevas tablas y cambios a su BD local.
---
------- Cada vez que se añada una credencial nueva, Además de agregarla en el appsettings.json local se debe:
---
---	-	Actualizar en este archivo para tener un conjunto de las mismas.
---	    Luego de modificar el appsettings.json y actualizar la BD local, es necesario agregar la modificación con campos vacíos al appsettings.example.json. Según la indicación:
---
---	Actualizar el archivo appsettings.example.json que ya existe en el repositorio de GitHub (NO crear un archivo nuevo, solo modificar el existente.), y agregar los nuevos campos con valores vacíos.
---
>>>>>> 		*Este contenido, es para seguir el patron de seguridad:*	**Environment-based configuration**.
