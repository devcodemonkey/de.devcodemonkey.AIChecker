# Import the module
Import-Module ImportExcel

# Define the path to the Excel file and the sheet name
$excelFilePath = "C:\Users\d-hoe\source\repos\masterarbeit.wiki\20_00_00-Projektplanung\ModellvergleichVonTheBloke.xlsx"
$sheetName = "Sheet1"  # Change to your actual sheet name if different

# Define the output directory for the downloaded files
$outputDirectory = "D:\MyModels\Models"
if (-not (Test-Path -Path $outputDirectory)) {
    New-Item -ItemType Directory -Path $outputDirectory
}

# Define the log file path
$logFilePath = Join-Path -Path $outputDirectory -ChildPath "download_log.txt"

# Define column names
$rowUrl = 'URL'
$rowName = 'Name'


# Start logging
Add-Content -Path $logFilePath -Value "`n`n===== Download Started: $(Get-Date) ====="

# Import the data from the Excel file
$data = Import-Excel -Path $excelFilePath -WorksheetName $sheetName | Where-Object { $_.'Max RAM required as Number' -le 16 -and $_.'Quant method' -eq 'Q4_K_M' }

# Loop through each row and download the file from the URL in the first column
foreach ($row in $data) {
    $url = $row.$rowUrl
    $name = $row.$rowName
    
    # Edit url from this https://huggingface.co/TheBloke/leo-hessianai-7B-chat-bilingual-GGUF
    # to this format     https://huggingface.co/TheBloke/leo-hessianai-7B-chat-bilingual-GGUF/blob/main/leo-hessianai-7b-chat-bilingual.Q4_K_M.gguf    
    $url = "$url/resolve/main/$name"

    $fileName = [System.IO.Path]::GetFileName($url)
    $outputFilePath = Join-Path -Path $outputDirectory -ChildPath $fileName

    # Download the file using Start-BitsTransfer
    try {
        Write-Host "Downloading $url..."
        Start-BitsTransfer -Source $url -Destination $outputFilePath -Priority Foreground -ErrorAction Stop
        Write-Host "Downloaded $url to $outputFilePath" -ForegroundColor Green

        # Log the successful download
        Add-Content -Path $logFilePath -Value "SUCCESS: Downloaded $url to $outputFilePath"
    }
    catch {
        Write-Host "Failed to download $url" -ForegroundColor Red

        # Log the failure
        Add-Content -Path $logFilePath -Value "ERROR: Failed to download $url - $_"
    }
}

Write-Host "All downloads complete!"