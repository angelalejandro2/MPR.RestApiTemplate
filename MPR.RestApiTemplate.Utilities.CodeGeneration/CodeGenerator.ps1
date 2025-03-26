# PowerShell script to generate solution code from added Entity classes

Write-Host "........................................"
Write-Host "Generate solution code from added Entity classes" -ForegroundColor Cyan
Write-Host "........................................"

# Prompt for confirmation
$confirmation = Read-Host "Are you sure you want to delete generated files (Y/[N])?"
if ($confirmation -ne "Y") {
    exit
}

# Select the VS version
# $tt = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\TextTransform.exe"
# $tt = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\TextTransform.exe"
# $tt = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\TextTransform.exe"
# $tt = "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\TextTransform.exe"
# $tt = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\TextTransform.exe"
$tt = "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\TextTransform.exe"

Write-Host "Delete previously generated cs code files"
Remove-Item -Force "..\MPR.RestApiTemplate.Api\Startup\DbContextRegistration.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Api\Startup\AutoMapperServiceRegistration.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Api\Startup\ApplicationServiceRegistration.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Api\Controllers\Controllers.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Application\Services\Services.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Application\Mappings\MappingProfiles.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Application\Models\Models.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Infrastructure\UnitOfWork.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Infrastructure\Repositories\RepositoryImplementation.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Domain\Interfaces\IUnitOfWork.generated.cs"
Remove-Item -Force "..\MPR.RestApiTemplate.Domain\Interfaces\Repositories\RepositoryInterfaces.generated.cs"

Write-Host "."
Write-Host "Run all T4s..."
Write-Host (Get-Location)

Write-Host "1. Generate Repository Interfaces"
& $tt ".\T4Templates\RepositoryInterfaces.tt" -out "..\MPR.RestApiTemplate.Domain\Interfaces\Repositories\RepositoryInterfaces.generated.cs"

Write-Host "2. Generate IUnitOfWork Interface"
& $tt ".\T4Templates\IUnitOfWork.tt" -out "..\MPR.RestApiTemplate.Domain\Interfaces\IUnitOfWork.generated.cs"

Write-Host "3. Generate Repository Implementations"
& $tt ".\T4Templates\RepositoryImplementation.tt" -out "..\MPR.RestApiTemplate.Infrastructure\Repositories\RepositoryImplementation.generated.cs"

Write-Host "4. Generate UnitOfWork Implementation"
& $tt ".\T4Templates\UnitOfWork.tt" -out "..\MPR.RestApiTemplate.Infrastructure\UnitOfWork.generated.cs"

Write-Host "5. Generate Application Models"
& $tt ".\T4Templates\Models.tt" -out "..\MPR.RestApiTemplate.Application\Models\Models.generated.cs"

Write-Host "6. Generate Entity-Model Mappings"
& $tt ".\T4Templates\MappingProfiles.tt" -out "..\MPR.RestApiTemplate.Application\Mappings\MappingProfiles.generated.cs"

Write-Host "7. Generate Application Services"
& $tt ".\T4Templates\Services.tt" -out "..\MPR.RestApiTemplate.Application\Services\Services.generated.cs"

Write-Host "8. Generate Controllers"
& $tt ".\T4Templates\Controllers.tt" -out "..\MPR.RestApiTemplate.Api\Controllers\Controllers.generated.cs"

Write-Host "9. Generate Startup Application Service Registration"
& $tt ".\T4Templates\ApplicationServiceRegistration.tt" -out "..\MPR.RestApiTemplate.Api\Startup\ApplicationServiceRegistration.generated.cs"

Write-Host "10. Generate Startup AutoMapper Service Registration"
& $tt ".\T4Templates\AutoMapperServiceRegistration.tt" -out "..\MPR.RestApiTemplate.Api\Startup\AutoMapperServiceRegistration.generated.cs"

Write-Host "11. Generate Startup Db Context Registration"
& $tt ".\T4Templates\DbContextRegistration.tt" -out "..\MPR.RestApiTemplate.Api\Startup\DbContextRegistration.generated.cs"

Write-Host "T4s completed."
