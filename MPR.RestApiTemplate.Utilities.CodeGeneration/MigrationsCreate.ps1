$timestamp = Get-Date -Format "yyyyMMddHHmmss"
$contextsPath = "..\MPR.RestApiTemplate.Infrastructure\Context"
$startupProject = "..\MPR.RestApiTemplate.Api"
$project = "..\MPR.RestApiTemplate.Infrastructure"

Write-Host "Generating migrations..." -ForegroundColor Yellow

# Asegura que el CLI esté disponible
if (-not (Get-Command "dotnet-ef" -ErrorAction SilentlyContinue)) {
    Write-Error "dotnet-ef is not installed. Run 'dotnet tool install --global dotnet-ef'"
    exit 1
}

Get-ChildItem -Path $contextsPath -Recurse -Filter *.cs | ForEach-Object {
    $fileContent = Get-Content $_.FullName -Raw
    $regex = '(?s)class\s+(\w+Context)\b.*?:\s*[\w<>,\s]*Context\b'
    if ($fileContent -match $regex) {
        $contextName = $matches[1]
        $contextShortName = $contextName -replace "Context$", ""
        $outputDir = "Migrations\$contextShortName"
        $migrationName = "AutoMigration_${contextName}_$timestamp"

        Write-Host "Generating migration for $contextName into $outputDir..." -ForegroundColor Cyan

        & dotnet ef migrations add $migrationName `
            --project $project `
            --startup-project $startupProject `
            --context $contextName `
            --output-dir $outputDir

        if ($LASTEXITCODE -eq 0) {
            Write-Host "Migration generated successfully for $contextName" -ForegroundColor Green
        } else {
            Write-Host "Migration failed for $contextName" -ForegroundColor Red
        }

        Write-Host "--------------------------------------------"
    }
}