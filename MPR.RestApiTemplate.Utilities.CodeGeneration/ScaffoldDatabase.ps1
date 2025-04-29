param(
    [Parameter(Mandatory = $true)]
    [string]$ConnectionString,

    [Parameter(Mandatory = $true)]
    [ValidateSet("SqlServer", "Oracle")]
    [string]$DbProvider,

    [string]$DomainProjectPath = "../*.Domain",        # Ruta al .csproj de las entidades
    [string]$InfrastructureProjectPath = "../*.Infrastructure",# Ruta al .csproj donde irá el DbContext
    [string]$OutputModelsDir = "Entities",
    [string]$OutputContextDir = "Context"
)

function Resolve-ProjectPath {
    param(
        [string]$PathWildcard
    )

    $projects = Get-ChildItem -Path $PathWildcard -Recurse -Filter "*.csproj"
    if ($projects.Count -eq 0) {
        throw "No .csproj found for path: $PathWildcard"
    } elseif ($projects.Count -gt 1) {
        Write-Warning "Multiple matches found for $PathWildcard. Using first one: $($projects[0].FullName)"
    }
    return $projects[0].FullName
}

$ResolveDomainProjectPath = Resolve-ProjectPath -PathWildcard $DomainProjectPath
$ResolveInfrastructureProjectPath = Resolve-ProjectPath -PathWildcard $InfrastructureProjectPath

# Determinar el proveedor y sus opciones
switch ($DbProvider) {
    "SqlServer" {
        $Provider = "Microsoft.EntityFrameworkCore.SqlServer"
    }
    "Oracle" {
        $Provider = "Oracle.EntityFrameworkCore"
    }
}

# Comando de scaffolding
$Command = @(
    "dotnet ef dbcontext scaffold",
    "`"$ConnectionString`"",
    "$Provider",
    "--output-dir $OutputModelsDir",               # Va al proyecto de modelos
    "--context-dir $OutputContextDir",             # Va al proyecto del contexto
    "--project $ResolveDomainProjectPath",        # Proyecto donde se ejecuta EF
    "--startup-project $ResolveInfrastructureProjectPath",# Proyecto de inicio para ejecutar EF
    "--force",
    "--no-onconfiguring",
    "--use-database-names",
    "--data-annotations"
) -join " "

# Cambiar al directorio del proyecto de las entidades para que genere los modelos allí
Push-Location (Split-Path -Path $ResolveDomainProjectPath)

Write-Host "Ejecutando scaffolding EF Core con los siguientes parámetros:" -ForegroundColor Cyan
Write-Host "DbContext:     $ContextName" -ForegroundColor Yellow
Write-Host "Connection:    $ConnectionString"
Write-Host "DbProvider:    $DbProvider"
Write-Host "Model output:  $OutputModelsDir"
Write-Host "Context output:$OutputContextDir"
Write-Host "Project EF:    $InfrastructureProjectPath"
Write-Host "Project Domain:$DomainProjectPath"
Write-Host ""
Write-Host $Command -ForegroundColor Green

# Ejecutar el comando
Invoke-Expression $Command

# Volver al directorio anterior
Pop-Location