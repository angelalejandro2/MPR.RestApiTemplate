$contextsPath = "..\MPR.RestApiTemplate.Infrastructure\Context"
$startupProject = "..\MPR.RestApiTemplate.Api"
$project = "..\MPR.RestApiTemplate.Infrastructure"

# Verifica que dotnet ef esté instalado
if (-not (Get-Command "dotnet-ef" -ErrorAction SilentlyContinue)) {
    Write-Error "dotnet-ef is not installed. Run 'dotnet tool install --global dotnet-ef'"
    exit 1
}

Get-ChildItem -Path $contextsPath -Recurse -Filter *.cs | ForEach-Object {
    $fileContent = Get-Content $_.FullName -Raw
    $regex = '(?s)class\s+(\w+DbContext)\b.*?:\s*[\w<>,\s]*DbContext\b'
    if ($fileContent -match $regex) {
        $contextName = $matches[1]

        Write-Host "`nApplying migrations for $contextName..." -ForegroundColor Cyan

        & dotnet ef database update `
            --project $project `
            --startup-project $startupProject `
            --context $contextName

        if ($LASTEXITCODE -eq 0) {
            Write-Host "Migration applied successfully for $contextName" -ForegroundColor Green
        } else {
            Write-Host "Migration failed for $contextName" -ForegroundColor Red
        }

        Write-Host "--------------------------------------------"
    }
}