param(
    [Parameter(Mandatory = $false)]
    [string]$SourceName = "MPR.RestApiTemplate", # Ej: MPR.RestApiTemplate

    [Parameter(Mandatory = $false)]
    [string]$ShortName = "mpr-rest-api-template", # Ej: mprapi

    [Parameter(Mandatory = $false)]
    [string]$Identity = "MPR.RestApiTemplate", # Ej: TuEmpresa.MPR.RestApiTemplate

    [Parameter(Mandatory = $false)]
    [string]$OutputPath = "./output",

    [Parameter(Mandatory = $false)]
    [string[]]$Exclude = @("MPR.RestApiTemplate.Vsix", ".git", ".gitignore", ".vs", "PrepareTemplate.ps1", "CHANGELOG.md", "README.md", "LICENSE", "LICENSE.txt", "CONTRIBUTING.md", "CONTRIBUTORS.md", ".vscode", ".editorconfig", ".gitattributes", ".gitkeep", ".github", "template_tmp", "TemplateConfig", "template.json") # Exclude folders and files (wildcards are allowed)
)

$TempPath = "./template_tmp"

function Initialize-TempDirectory {
    Write-Host "Preparando carpeta temporal '$TempPath'..."

    if (Test-Path $TempPath) {
        Remove-Item -Recurse -Force $TempPath
    }
    New-Item -ItemType Directory -Path $TempPath | Out-Null

    $rootItems = Get-ChildItem -LiteralPath "." -Force |
    Where-Object {
        $excludeMatch = $false
        foreach ($pattern in $Exclude) {
            if ($_.FullName -like "*$pattern*") { $excludeMatch = $true; break }
        }
        return !$excludeMatch
    } 

    foreach ($item in $rootItems) {
        Copy-Item -Path $item.FullName -Destination $TempPath -Recurse -Force
    }
}

function Remove-DirectoryContent {
    Write-Host "Limpiando archivos innecesarios en '$TempPath'..."
    Get-ChildItem -Path $TempPath -Recurse -Include bin, obj, .vs, .vscode, TestResults | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    Get-ChildItem -Path $TempPath -Recurse -Include *.user, *.suo, launchSettings.json | Remove-Item -Force -ErrorAction SilentlyContinue
}

function Update-Names {
    Write-Host "Reemplazando '$SourceName' a '{{SolutionName}}' en archivos y nombres..."

    $allFiles = Get-ChildItem -Path $TempPath -Recurse -File -Include *.cs, *.csproj, *.sln, *.json, *.cshtml, *.config, *.yml

    foreach ($file in $allFiles) {
        (Get-Content $file.FullName) -replace $SourceName, "{{SolutionName}}" | Set-Content $file.FullName
    }

    # Renombrar carpetas
    Get-ChildItem -Path $TempPath -Recurse -Directory |
    Where-Object { $_.Name -like "*$SourceName*" } |
    ForEach-Object {
        $newName = $_.Name -replace [regex]::Escape($SourceName), "{{SolutionName}}"
        Rename-Item -Path $_.FullName -NewName $newName -Force
    }

    # Renombrar archivos
    Get-ChildItem -Path $TempPath -Recurse -File |
    Where-Object { $_.Name -like "*$SourceName*" } |
    ForEach-Object {
        $newName = $_.Name -replace [regex]::Escape($SourceName), "{{SolutionName}}"
        Rename-Item -Path $_.FullName -NewName $newName -Force
    }
}

function New-NugetPackage {
    $templateCsproj = Join-Path $TempPath "Template.csproj"
    $nugetOutput = Join-Path $OutputPath "nuget"

    Write-Host "Empaquetando plantilla como NuGet..."
    dotnet pack $templateCsproj -o $nugetOutput --nologo

    if (Test-Path $nugetOutput) {
        Write-Host "Paquete generado en: $nugetOutput"
    } else {
        Write-Warning "No se pudo generar el paquete. Revisa que dotnet esté disponible y que Template.csproj sea válido."
    }
}

function Remove-Temp {
    if (Test-Path $TempPath) {
        Write-Host "Eliminando carpeta temporal..."
        Remove-Item -Recurse -Force $TempPath
    }
}

function Clear-SpecifiedPaths {
    param (
        [Parameter(Mandatory = $false)]
        [string[]]$DirectoriesToClean = @(),

        [Parameter(Mandatory = $false)]
        [string[]]$FilesToDelete = @()
    )

    foreach ($dir in $DirectoriesToClean) {
        $cleanPath = Join-Path $TempPath $dir

        if (Test-Path $cleanPath) {
            Write-Host "Limpiando contenido de '$cleanPath'..."
            Get-ChildItem -Path $cleanPath -Recurse -Force |
            Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
        }
        else {
            Write-Host "Creando carpeta limpia '$cleanPath'..."
            New-Item -ItemType Directory -Path $cleanPath -Force | Out-Null
        }
    }

    foreach ($file in $FilesToDelete) {
        $targetFile = Join-Path $TempPath $file

        if (Test-Path $targetFile) {
            Write-Host "Eliminando archivo '$targetFile'..."
            Remove-Item -Path $targetFile -Force -ErrorAction SilentlyContinue
        }
    }
}

function Copy-TemplateConfigFiles {
    $templateSourceDir = "./TemplateConfig"
    $templateTargetConfigDir = Join-Path $TempPath "template.config"
    $templateCsprojTargetPath = Join-Path $TempPath "Template.csproj"

    if (!(Test-Path $templateSourceDir)) {
        Write-Warning "La carpeta 'TemplateConfig' no existe. Se omite la copia de archivos de configuración."
        return
    }

    if (!(Test-Path $templateTargetConfigDir)) {
        New-Item -ItemType Directory -Path $templateTargetConfigDir -Force | Out-Null
    }

    $templateJsonSourcePath = Join-Path $templateSourceDir "template.json"
    $templateCsprojSourcePath = Join-Path $templateSourceDir "Template.csproj"

    if (Test-Path $templateJsonSourcePath) {
        Copy-Item $templateJsonSourcePath -Destination (Join-Path $templateTargetConfigDir "template.json") -Force
        Write-Host "template.json copiado a $templateTargetConfigDir"
    } else {
        Write-Warning "No se encontró 'template.json' en TemplateConfig/"
    }

    if (Test-Path $templateCsprojSourcePath) {
        Copy-Item $templateCsprojSourcePath -Destination $templateCsprojTargetPath -Force
        Write-Host "Template.csproj copiado a $templateCsprojTargetPath"
    } else {
        Write-Warning "No se encontró 'Template.csproj' en TemplateConfig/"
    }
}



function Move-NugetPackage {
    $NugetSourcePath = Join-Path $OutputPath "nuget"
    $VsixProjectPath = "./MPR.RestApiTemplate.Vsix/ProjectTemplates"

    if (!(Test-Path $NugetSourcePath)) {
        Write-Warning "No se encontró la carpeta de paquetes NuGet en '$NugetSourcePath'"
        return
    }

    $nupkg = Get-ChildItem -Path $NugetSourcePath -Filter *.nupkg | Select-Object -Last 1

    if ($null -eq $nupkg) {
        Write-Warning "No se encontró ningún archivo .nupkg en '$NugetSourcePath'"
        return
    }

    if (!(Test-Path $VsixProjectPath)) {
        Write-Host "Creando carpeta de destino: $VsixProjectPath"
        New-Item -ItemType Directory -Path $VsixProjectPath -Force | Out-Null
    }

    Copy-Item -Path $nupkg.FullName -Destination $VsixProjectPath -Force

    Write-Host "Paquete '$($nupkg.Name)' copiado a '$VsixProjectPath'"
}

Move-NugetPackage

# === EJECUCIÓN ===

$ErrorActionPreference = "Stop"

Remove-Temp
Initialize-TempDirectory
Remove-DirectoryContent
Clear-SpecifiedPaths `
    -DirectoriesToClean @(
    "MPR.RestApiTemplate.Api\\Controllers\\Generated",
    "MPR.RestApiTemplate.Api\\Startup\\Generated", 
    "MPR.RestApiTemplate.Application\\Dtos\\Generated",
    "MPR.RestApiTemplate.Application\\Mappings\\Generated",
    "MPR.RestApiTemplate.Application\\Services\\Generated",
    "MPR.RestApiTemplate.Domain\\Entities",
    "MPR.RestApiTemplate.Domain\\Interfaces\\Repositories\\Generated",
    "MPR.RestApiTemplate.Infrastructure\\Context",
    "MPR.RestApiTemplate.Infrastructure\\Migrations",
    "MPR.RestApiTemplate.Infrastructure\\Repositories\\Generated"
) `
    -FilesToDelete @(
    "*.user", 
    "*.suo", 
    "MPR.RestApiTemplate.Domain\\Interfaces\\IUnitOfWork.generated.cs",
    "MPR.RestApiTemplate.Infrastructure\\UnitOfWork.generated.cs"
)
Update-Names
Copy-TemplateConfigFiles
New-NugetPackage
Remove-Temp

Write-Host "`nPlantilla lista en '$OutputPath'"
Write-Host "Puedes probarla con:"
Write-Host "   dotnet new $ShortName -n MiProyecto"