$folderPath = "Entities"

# Buscar todos los archivos .cs en la carpeta (recursivamente si lo deseas)
Get-ChildItem -Path $folderPath -Filter *.cs -Recurse | ForEach-Object {
    $filePath = $_.FullName
    $lines = Get-Content $filePath

    # Eliminar línea que contenga using Microsoft.EntityFrameworkCore;
    $lines = $lines | Where-Object { $_ -notmatch '^\s*using\s+Microsoft\.EntityFrameworkCore\s*;' }

    # Buscar la línea que contiene 'public partial class'
    $classIndex = $lines | Select-String -Pattern 'public\s+partial\s+class' | Select-Object -First 1

    if ($classIndex) {
        $index = $classIndex.LineNumber - 1

        # Recorre desde el principio hasta la línea anterior al class
        for ($i = $index - 1; $i -ge 0; $i--) {
            if ($lines[$i] -match '^\s*\[.*\]\s*$') {
                $lines.RemoveAt($i)
            }
        }

        # Sobrescribe el archivo con el nuevo contenido
        Set-Content -Path $filePath -Value $lines
        Write-Host "Limpio: $filePath"
    } else {
        Write-Host "Clase no encontrada en: $filePath"
    }
}