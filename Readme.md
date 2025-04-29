# Instrucciones para modificar el template
1. Hacer los cambios que se necesiten hacer en los proyectos. 
1. Modificar el archivo Template.csproj y subirle la version de acuerdo al cambio que se esta haciendo.
1. Ejecutar el script PrepareTemplate.ps1 para que se generen los archivos de configuración y se eliminen los archivos innecesarios.
1. El script va a generar un nuget package en el directorio output/nuget

# Docker image para oracle
- Bajar imagen y ejecutarla
```bash
docker run -d --name oracle-xe -p 1521:1521 -p 8080:8080 -e ORACLE_PASSWORD=MySecurePassword gvenzl/oracle-xe:21-slim
```

- Probar
```bash
docker exec -it oracle-xe sqlplus system/MySecurePassword@//localhost:1521/XEPDB1
```

-INstalar esquemas de ejemplo
```bash
./oracle-install-sample-schemas.sh