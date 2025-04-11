$folderPath = "..\MPR.RestApiTemplate.Domain\Entities"

Get-ChildItem -Path $folderPath -Filter *.cs -Recurse | ForEach-Object {
    $filePath = $_.FullName
    $lines = Get-Content $filePath

    # Convertir a lista de strings explícitamente
    $mutableLines = New-Object System.Collections.Generic.List[string]
    foreach ($line in $lines) {
        $mutableLines.Add([string]$line)
    }

    # Eliminar 'using Microsoft.EntityFrameworkCore;'
    $mutableLines = $mutableLines | Where-Object { $_ -notmatch '^\s*using\s+Microsoft\.EntityFrameworkCore\s*;' }

    # Buscar la línea con 'public partial class Nombre'
    $classMatch = $mutableLines | Select-String -Pattern '^\s*public\s+partial\s+class\s+\w+' | Select-Object -First 1

    if ($classMatch) {
        $classLineIndex = $classMatch.LineNumber - 1

        # Separar antes y después de la clase
        $before = $mutableLines[0..($classLineIndex - 1)]
        $after = $mutableLines[$classLineIndex..($mutableLines.Count - 1)]

        # Eliminar solo líneas de atributos [Algo]
        $cleanBefore = $before | Where-Object { $_ -notmatch '^\s*\[.*\]\s*$' }

        # Reunir contenido final
        $finalContent = $cleanBefore + $after

        # Guardar el archivo
        Set-Content -Path $filePath -Value $finalContent
        Write-Host "Limpio: $filePath"
    } else {
        Write-Host "No se encontró clase parcial en: $filePath"
    }
}