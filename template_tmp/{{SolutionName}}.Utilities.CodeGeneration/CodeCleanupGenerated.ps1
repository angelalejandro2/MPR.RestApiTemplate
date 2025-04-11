# PowerShell script to generate solution code from added Entity classes

Write-Host "........................................"
Write-Host "Generate solution code from added Entity classes" -ForegroundColor Cyan
Write-Host "........................................"

# Prompt for confirmation
$confirmation = Read-Host "Are you sure you want to delete generated files (Y/[N])?"
if ($confirmation -ne "Y") {
    exit
}

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
Write-Host "T4s completed." -ForegroundColor Green