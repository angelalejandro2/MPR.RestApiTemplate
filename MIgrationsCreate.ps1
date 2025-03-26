$timestamp = Get-Date -Format "yyyyMMddHHmmss"
$contextsPath = ".\Infrastructure\Context"
$startupProject = ".\MPR.RestApiTemplate.Api"
$project = ".\Infrastructure"

# Requiere Entity Framework CLI
# dotnet tool install --global dotnet-ef

Get-ChildItem -Path $contextsPath -Filter *.cs | ForEach-Object {
    $fileContent = Get-Content $_.FullName -Raw
    if ($fileContent -match "class\s+(\w+DbContext)\s*:\s*DbContext") {
        $contextName = $matches[1]
        $contextShortName = $contextName -replace "DbContext$", ""
        $outputDir = "Migrations\$contextShortName"
        $migrationName = "AutoMigration_${contextName}_$timestamp"

        Write-Host "Generating migration for $contextName into $outputDir..."

        dotnet ef migrations add $migrationName `
            --project $project `
            --startup-project $startupProject `
            --context $contextName `
            --output-dir $outputDir
    }
}