
# Rest Api Template

## Tabla de Contenido

1. [Introducción](#1-introducción)
2. [¿Qué problema resuelve Clean Architecture?](#2-qué-problema-resuelve-clean-architecture)
3. [Capas de Clean Architecture](#3-capas-de-clean-architecture)
    - 3.1 [Dominio (Domain)](#31-dominio-domain)
    - 3.2 [Aplicación (Application)](#32-aplicación-application)
    - 3.3 [Infraestructura (Infrastructure)](#33-infraestructura-infrastructure)
    - 3.4 [API / Presentación](#34-api--presentación)
4. [Beneficios de aplicar correctamente cada capa](#4-beneficios-de-aplicar-correctamente-cada-capa)
5. [Migración a la nube: cómo ayuda esta arquitectura](#5-migración-a-la-nube-cómo-ayuda-esta-arquitectura)
6. [Automatización con T4 Templates](#6-automatización-con-t4-templates)
    - 6.1 [Qué se genera](#61-qué-se-genera)
    - 6.2 [Generacion de codigo](#62-generacion-de-codigo)
    - 6.3 [Diseño de extensibilidad](#63-diseño-de-extensibilidad)
    - 6.4 [Extender un servicio en la capa application para aplicar logica de negocio](#64-extender-un-servicio-en-la-capa-application-para-aplicar-logica-de-negocio)
    - 6.5 [Sobrescribir un método virtual (override)](#65-sobrescribir-un-método-virtual-override)
    - 6.6 [Patrón de Repositorios y Unit of Work](#66-patrón-de-repositorios-y-unit-of-work)
7. [Orden de componentes: del controlador a la base de datos](#7-orden-de-componentes-del-controlador-a-la-base-de-datos)
    - 7.1 [Secuencia paso a paso usando entidad Policy como ejemplo](#71-secuencia-paso-a-paso-usando-entidad-policy-como-ejemplo)
    - 7.2 [Ejemplo de código](#72-ejemplo-de-código)
8. [Enfoque Code First y Migraciones usando EF Core](#8-enfoque-code-first-y-migraciones-usando-ef-core)

## 1. Introducción

Este documento describe cómo esta estructurado el Rest Api  Template y como tienen que estar estructurados los APIs en general utilizando los principios de **Clean Architecture**, ajustados a la realidad de nuestro equipo.  
Se explica cómo gestionamos la separación de capas, cómo automatizamos el código repetitivo con **T4 templates**, y cómo este enfoque facilita una futura migración a la nube.

---

## 2. ¿Qué problema resuelve Clean Architecture?

Las arquitecturas tradicionales tienden a mezclar la lógica de negocio, los detalles técnicos y la presentación en un mismo lugar, generando:

- Código difícil de mantener.
- Alta dependencia entre componentes.
- Problemas al escalar o cambiar tecnología.
- Reutilización limitada y pruebas complicadas.

Clean Architecture propone una forma clara y escalable de estructurar el sistema por capas bien definidas y desacopladas.

---

## 3. Capas de Clean Architecture

### 3.1 Dominio (Domain)
- Entidades con reglas de negocio puras.
- Sin dependencias externas.
- Totalmente testeables.

### 3.2 Aplicación (Application)
- Casos de uso representados por métodos dentro de servicios de aplicación.
- Se orquesta la lógica de negocio usando entidades y repositorios.
- Sin dependencias hacia Infraestructura ni hacia la web.

### 3.3 Infraestructura (Infrastructure)
- Implementación de bases de datos, correo, archivos, etc.
- Contiene los repositorios concretos que heredan del repositorio genérico.

### 3.4 API / Presentación
- Controladores que reciben peticiones HTTP.
- Validan entradas y delegan la ejecución a los servicios de aplicación.
- Devuelven respuestas HTTP.

---

## 4. Beneficios de aplicar correctamente cada capa

| Capa | Beneficios |
|------|------------|
| Dominio | Reglas claras, reusables y aisladas. Fácil de probar y migrar. |
| Aplicación | Casos de uso explícitos. Lógica de negocio estructurada. |
| Infraestructura | Se puede cambiar sin afectar el negocio. Ideal para nube. |
| API | Ligera, fácil de testear y mantener. |

---

## 5. Migración a la nube: cómo ayuda esta arquitectura

- **Separación por capas** permite migrar en fases (ej. primero base de datos, luego APIs).
- Infraestructura desacoplada permite usar servicios cloud (ej. Azure SQL, Blob Storage).
- Fácil de escalar horizontalmente sin romper otras capas.
- Compatible con arquitecturas modernas (microservicios, serverless).
- Mejora pruebas locales antes de subir a producción cloud.

---

## 6. Automatización con T4 Templates

Usamos T4 Templates para generar automáticamente código repetitivo a partir de las entidades y DbContexts. 

Todo el codigo que es auto generado va a estar identificado de alguna manera. Ya sea que contiene .generated. como parte del nombre del archivo o que se encuentre dentro de un directorio con nombre Generated. Estos archivos se recomienda no modificarlos para que no se pierda codigo cuando se ejecute el script de generar codigo.

El hecho de que el template tenga partes autogeneradas, no implica que no se pueden agregar objetos, clases y metodos nuevos. Esto sigue siendo una solucion como cualquier otra salvo que tiene una parte autogenerada. **Lo que no se permite es violentar la arquitectura y las referencias de proyectos establecida.** 

### 6.1 Qué se genera

- Interfaces de repositorios (`IPoliciesRepository`)
- Repositorios concretos (`PoliciesRepository`)
- Repositorio genérico (`GenericRepository<TEntity>`)
- Unit of Work (`IUnitOfWork`, `UnitOfWork`)
- DTOs (`PolicyDto`, `PolicyCreateDto`, etc.)
- Mapeos con AutoMapper
- Controladores REST (`PolicyController`)
- Servicios de Aplicación (`PolicyService`)
- Extensiones para injectar dependencias en Program.cs

### 6.2 Generacion de codigo

En el proyecto CodeGeneration se encuentra un powershell script con el nombre CodeGenerator.ps1. Este script va a ejecutar los T4 templates que generan codigo en el orden programado.

Los T4 templates van a generar los objetos definidos previamente a partir de las entidades y los dbContext. Por tanto, para poder tener una generacion de codigo exitosa, tiene que haber creado sus entidades y haberlas referenciado en su respectivo DbContext.

Para ejecutar el script:
1. Abra el Package Manager Console o el Developer PowerShell.
2. Cambie el directorio al proyecto que termina en code generation:
```ps1
cd {YourSolutionName}.CodeGeneration
```
3. Ejecute el siguiente comando
``` ps1
.\CodeGenerator.ps1
```

La ejecucion de powershell scripts en windows puede dar errores por defecto. Esto ocurre cuando un powershell script no esta digitally signed. Para poder ejecutar estos scripts se necesita ejecutar el siguiente powershell command. Esto es una sola vez.
```ps1
Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
```

**NOTA IMPORTANTE** Si usted baja codigo por el internet, lo copia de otra computadora o se lo envian por teams, es posible que windows le bloquee los archivos por seguridad. Esto va a causar que no pueda modificar los archivos de codigo o que no pueda ejecutar los scripts. Para remover el bloqueo necesita hacer lo siguiente:
- Hacer right click en el archivo que esta bloqueado y seleccionar properties
- Si el archivo esta bloqueado, en el tab General, a la parte abajo va a ver la siguiente seccion:
    - Security: This file came from another computer and might be blocked to help protect this computer
- Si el archivo no esta bloqueado no va a ver esta seccion.
- Para desbloquear haga check en el checkbox que dice Unblock y presione Apply

Se recomienda que todo lo relacionado a codigo se maneje a traves de los repositorios en github o svn (si fuera el caso).

### 6.3 Diseño de extensibilidad

Para que se pueda extender el codigo sin tener que tocar el codigo autogenerado se siguen estas practicas:

- Clases generadas como `partial`: permiten extender el código sin perder cambios al regenerar.
- Métodos `virtual`: permiten sobrescribir lógica si se requiere personalización o pruebas.

### 6.4 Extender un servicio en la capa application para aplicar logica de negocio

La logica de negocios va a estar ubicada en la carpeta Services de la capa Application. Los servicios que son autogenerados estan en el directorio Generated para mantenerlos separados del codigo que se genere manualmente.

La regla aqui es que vas a utilizar los repositorios y el unit of work para manejar todo el tema de extracion de datos y persistencia de datos. No se van a llamar objetos de la capa de infrastructura directamente, por ejemplo los DbContext.

**NOTA** Esta misma dinamica aplica para los controllers en la capa API, salvo que la carpeta es Controllers dentro de la capa API. 

Supón que el archivo generado contiene:

```csharp
// Archivo generado automáticamente
public partial class PolicyService
{
    // ...métodos generados...
}
```

Puedes crear un archivo separado para extenderlo:

```csharp
// Archivo creado por el usuario (no se sobrescribe al regenerar)
public partial class PolicyService
{
    public string GetCustomMessage()
    {
        return "Este método fue agregado como extensión parcial para implementar logica de negocios.";
    }
}
```

Así puedes agregar métodos o propiedades adicionales sin perder tus cambios al volver a generar el código.

### 6.5 Sobrescribir un método virtual (override)

Van a haber momento donde la clase generada tiene el metodo que queremos pero la implementacion no es la deseada. Para poder utilizar ese mismo metodo pero nosotros especificar la logica que queremos, lo que hacemos es sobrescribir el metodo.

Para sobrescribir un método virtual, debes crear una clase que herede del servicio generado:

Supón que el archivo generado contiene:

```csharp
// Archivo generado automáticamente
public class PolicyService
{
    public virtual string GetPolicyType(int policyId)
    {
        // Lógica por defecto
        return "General";
    }
}
```

Puedes crear una clase que herede y sobrescriba el método:

```csharp
// Archivo creado por el usuario
public class CustomPolicyService : PolicyService
{
    public override string GetPolicyType(int policyId)
    {
        // Lógica personalizada
        if (policyId == 1)
            return "Especial";
        return base.GetPolicyType(policyId);
    }
}
```

De esta forma puedes personalizar la lógica de los métodos virtuales mediante herencia.

## 6.6 Patrón de Repositorios y Unit of Work

En este template utilizamos el **patrón de repositorios** junto con el **Unit of Work** y un **repositorio genérico** para manejar el acceso a datos de manera estructurada y desacoplada.

- El **repositorio genérico** permite reutilizar la lógica común para operaciones CRUD sobre cualquier entidad.
- Los **repositorios concretos** heredan del repositorio genérico y pueden implementar lógica específica para cada entidad.
- El **Unit of Work** centraliza la gestión de los repositorios y coordina la persistencia de los cambios en la base de datos.

### Escenario con múltiples DbContext

En nuestro caso particular, el sistema puede trabajar con varios `DbContext` al mismo tiempo. Por eso, el **Unit of Work** es el encargado de centralizar todos los repositorios y asociarlos con su respectivo `DbContext`.  
De esta manera, la capa de aplicación no necesita preocuparse por cuál contexto utilizar ni cómo manejar múltiples fuentes de datos: simplemente utiliza el Unit of Work y los repositorios necesarios, manteniendo el código limpio y sencillo.

Este enfoque facilita la escalabilidad, el mantenimiento y la posibilidad de trabajar con diferentes bases de datos o esquemas dentro de la misma solución.

---

## 7. Orden de componentes: del controlador a la base de datos

```plaintext
[1] Controladores en capa API
        │
        ▼
[2] Servicios en capa Application (Aqui va toda la logica de negocios)
        │
        ▼
[3] El servicio se comunica con dominio:
    - Mapea los DTO a la entidad de dominio y viceversa
    - Ejecuta reglas de negocio
        │
        ▼
[4] UnitOfWork
        │
        ▼
[5] Repositorios heredando del repositorio generico
        │
        ▼
[6] DbContexts
        │
        ▼
[6] Bases De datos
```
**NOTA** Este orden no se puede alterar. Por ejemplo, no podemos tener un controlador llamando al UnitOfWork o un Servicio en application llamando a un DbContext.

### 7.1 Secuencia paso a paso usando entidad Policy como ejemplo

```plaintext
[1] PolicyController recibe Dto
        │
        ▼
[2] Llama al metodo AddAsync en PolicyService
        │
        ▼
[3] El servicio:
    - Mapea el DTO a la entidad de dominio
    - Ejecuta reglas de negocio
    - Persiste a través del repositorio
        │
        ▼
[4] El servicio llama al UnitOfWork
        │
        ▼
[5] UnitOfWork llama a PolicyRepository que implementa el repositorio generico
        │
        ▼
[6] Se persisten los datos en la base de datos
        │
        ▼
[7] Se devuelve respuesta

```

###  7.2 Ejemplo de código

Controller

``` csharp
[HttpPost]
public virtual async Task<IActionResult> Post(PolicyCreateDto model)
{
    var result = await _policyService.AddAsync(model);
    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
}
```

Application Service

``` csharp
public partial class PolicyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PolicyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public partial async Task<PolicyCreateDto> AddAsync(PolicyCreateDto model)
    {
        var policy = _mapper.Map<Policy>(model);
        await _unitOfWork.PoliciesRepository.AddAsync(policy);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PolicyCreateDto>(policy);
    }
}
```

## 8. Enfoque Code First y Migraciones usando EF Core

Para el manejo de las bases de datos en este template utilizamos un enfoque **Code First** con Entity Framework Core. Esto significa que el modelo de datos se define primero en el código (entidades y DbContext), y a partir de ahí se generan las tablas y relaciones en la base de datos.

Si el proyecto parte de una base de datos existente, se realiza un proceso de **scaffolding** para generar las entidades y el DbContext a partir de la estructura actual. A partir de ese punto, todo el desarrollo y los cambios en el modelo se gestionan en modo Code First.

```ps1
Scaffold-DbContext "Server=TU_SERVIDOR;Database=TU_BASEDEDATOS;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context TuDbContext -f
```
- Reemplaza `TU_SERVIDOR` y `TU_BASEDEDATOS` por los valores de tu entorno.
- `-OutputDir Models` indica la carpeta donde se generarán las entidades.
- `-Context TuDbContext` define el nombre de la clase DbContext generada.
- `-f` fuerza la sobreescritura de archivos existentes.
- Reemplaza `Microsoft.EntityFrameworkCore.SqlServer` por `Oracle.EntityFrameworkCore` si estas utilizando una base de datos Oracle. Es posible que tambien tengas que ajustar el connection string de acuerdo a lo que necesite Oracle.

Una vez que el modelo está listo o se realizan cambios, utilizamos **migraciones** para aplicar esos cambios a las bases de datos en los ambientes de desarrollo y pruebas. Las migraciones permiten mantener sincronizado el esquema de la base de datos con el modelo definido en el código, facilitando la evolución del sistema.

En el proyecto CodeGeneration hay un powershell script para generar las migraciones. Para utilizarlo:

1. Abra el Package Manager Console o el Developer PowerShell.
2. Cambie el directorio al proyecto que termina en code generation:
```ps1
cd {YourSolutionName}.CodeGeneration
```
3. Ejecute el siguiente comando
``` ps1
.\MigrationsCreate.ps1
```

Esto va a generar los archivos de migracion y los va a colocar en el directorio Migrations en la capa Infrastructure. 

Para aplicar las migraciones en ambientes no productivos, en el proyecto CodeGeneration hay un powershell script para generar las migraciones. Para utilizarlo:

1. Abra el Package Manager Console o el Developer PowerShell.
2. Cambie el directorio al proyecto que termina en code generation:
```ps1
cd {YourSolutionName}.CodeGeneration
```
3. Ejecute el siguiente comando
``` ps1
.\MigrationsApply.ps1
```

**NOTA** Esto va a ejecutar las migraciones para todos los dbcontext a la vez. Si no quiere aplicarlos todos o quiere hacerlo de otra manera, puede ejcutar manualmente el comando para aplicar las migraciones manualmente.

Para ambientes de producción, no se aplican las migraciones directamente desde el código. En su lugar, se generan **migration scripts** (scripts SQL) a partir de las migraciones, los cuales son revisados y ejecutados manualmente o mediante procesos controlados. Esto garantiza mayor seguridad y control sobre los cambios aplicados en producción.

```ps1
dotnet ef migrations script -o Migrations\Script.sql
```
- Este comando genera un archivo `Script.sql` en el directorio `Migrations` con todas las migraciones pendientes.
- Puedes especificar migraciones de inicio y fin con los parámetros `--from` y `--to` si solo quieres el script para ciertos cambios.
- Asegúrate de estar en el directorio del proyecto donde está el DbContext.

Este enfoque permite flexibilidad durante el desarrollo y control en los despliegues productivos, asegurando que la evolución del modelo de datos sea segura y predecible.
