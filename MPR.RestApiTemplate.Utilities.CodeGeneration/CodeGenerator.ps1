# PowerShell script to generate solution code from added Entity classes

Write-Host "........................................"
Write-Host "Generate solution code from added Entity classes" -ForegroundColor Cyan
Write-Host "........................................"

# Select the VS version
# $tt = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\TextTransform.exe"
# $tt = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\TextTransform.exe"
# $tt = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\TextTransform.exe"
# $tt = "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\TextTransform.exe"
# $tt = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\TextTransform.exe"
$tt = "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\TextTransform.exe"

# Función para ejecutar TextTransform.exe suprimiendo warnings y mostrando solo errores
function Invoke-TextTransform {
    param (
        [string]$TemplateFile,
        [string]$OutputFile
    )
    
    Write-Host "  Processing: $TemplateFile -> $OutputFile" -ForegroundColor DarkGray
    
    # Ejecutar TextTransform y capturar toda la salida
    $output = & $tt $TemplateFile -out $OutputFile 2>&1
    
    # Filtrar solo los errores (ignorar warnings)
    $errors = $output | Where-Object { $_ -match "error " -and $_ -notmatch "warning " }
    
    # Si hay errores, mostrarlos
    if ($errors) {
        Write-Host "  ERRORES ENCONTRADOS:" -ForegroundColor Red
        $errors | ForEach-Object { 
            Write-Host "    $_" -ForegroundColor Red
        }
        return $false
    }
    
    return $true
}

Write-Host "Delete previously generated cs code files" -ForegroundColor Cyan

Remove-Item -Force "..\MPR.RestApiTemplate.Tests.Integration\IntegrationTests.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force "..\MPR.RestApiTemplate.Api\Startup\Generated\AutoMapperServiceRegistration.cs" -ErrorAction SilentlyContinue
Remove-Item -Force "..\MPR.RestApiTemplate.Api\Startup\Generated\ApplicationServiceRegistration.cs" -ErrorAction SilentlyContinue
Remove-Item -Force "..\MPR.RestApiTemplate.Infrastructure\UnitOfWork.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force "..\MPR.RestApiTemplate.Domain\Interfaces\IUnitOfWork.generated.cs" -ErrorAction SilentlyContinue

Write-Host "Running T4 Templates..." -ForegroundColor Cyan
Write-Host (Get-Location)

$totalTemplates = 13
$completedTemplates = 0
$failedTemplates = 0

Write-Host "1. Generate Repository Interfaces"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\RepositoryInterfaces.tt" -OutputFile ".\T4Templates\RepositoryInterfaces.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "2. Generate Repository Interfaces SP"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\RepositoryInterfacesSp.tt" -OutputFile ".\T4Templates\RepositoryInterfacesSp.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "3. Generate IUnitOfWork Interface"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\IUnitOfWork.tt" -OutputFile "..\MPR.RestApiTemplate.Domain\Interfaces\IUnitOfWork.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "4. Generate Repository Implementations"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\RepositoryImplementation.tt" -OutputFile ".\T4Templates\RepositoryImplementation.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "5. Generate UnitOfWork Implementation"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\UnitOfWork.tt" -OutputFile "..\MPR.RestApiTemplate.Infrastructure\UnitOfWork.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "6. Generate Application Dtos"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\Dtos.tt" -OutputFile ".\T4Templates\Dtos.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "7. Generate Entity-Model Mappings"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\MappingProfiles.tt" -OutputFile ".\T4Templates\MappingProfiles.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "8. Generate Application Services"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\Services.tt" -OutputFile ".\T4Templates\Services.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "9. Generate Controllers"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\Controllers.tt" -OutputFile ".\T4Templates\Controllers.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "10. Generate Startup Application Service Registration"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\ApplicationServiceRegistration.tt" -OutputFile "..\MPR.RestApiTemplate.Api\Startup\Generated\ApplicationServiceRegistration.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "11. Generate Startup AutoMapper Service Registration"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\AutoMapperServiceRegistration.tt" -OutputFile "..\MPR.RestApiTemplate.Api\Startup\Generated\AutoMapperServiceRegistration.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "12. Generate Startup DbContext Registration"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\DbContextRegistration.tt" -OutputFile ".\T4Templates\DbContextRegistration.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

Write-Host "13. Generate Integration Tests"
if (Invoke-TextTransform -TemplateFile ".\T4Templates\IntegrationTests.tt" -OutputFile "..\MPR.RestApiTemplate.Tests.Integration\IntegrationTests.generated.cs") {
    $completedTemplates++
} else {
    $failedTemplates++
}

# Cleanup de archivos temporales
Remove-Item -Force ".\T4Templates\DbContextRegistration.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force ".\T4Templates\Controllers.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force ".\T4Templates\Services.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force ".\T4Templates\MappingProfiles.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force ".\T4Templates\Dtos.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force ".\T4Templates\RepositoryImplementation.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force ".\T4Templates\RepositoryInterfaces.generated.cs" -ErrorAction SilentlyContinue
Remove-Item -Force ".\T4Templates\RepositoryInterfacesSp.generated.cs" -ErrorAction SilentlyContinue

# Mostrar resumen
Write-Host "........................................"
if ($failedTemplates -eq 0) {
    Write-Host "¡GENERACIÓN COMPLETADA EXITOSAMENTE!" -ForegroundColor Green
    Write-Host "Templates procesados: $completedTemplates de $totalTemplates" -ForegroundColor Green
} else {
    Write-Host "GENERACIÓN COMPLETADA CON ERRORES" -ForegroundColor Yellow
    Write-Host "Templates exitosos: $completedTemplates de $totalTemplates" -ForegroundColor Green
    Write-Host "Templates con errores: $failedTemplates" -ForegroundColor Red
}
Write-Host "........................................"
