# Define the application name (e.g., "chrome" for Google Chrome)
$applicationName = "ollama_llama_server"

# Get the process ID of the application
$process = Get-Process -Name $applicationName -ErrorAction SilentlyContinue
while ($true) {
    if ($process) {
        $processId = $process.Id

        # Retrieve GPU usage for the specific process
        $gpuCounters = Get-Counter -Counter "\GPU Engine(*)\Utilization Percentage" | Select-Object -ExpandProperty CounterSamples | Where-Object { $_.Path -like "*pid_$processId*" }

        if ($gpuCounters) {
            $gpuCounters | ForEach-Object {
                [PSCustomObject]@{
                    "Process ID"             = $processId
                    "Application Name"       = $applicationName
                    "Engine ID"              = $_.InstanceName
                    "Utilization Percentage" = $_.CookedValue                    
                }
            } | Export-Csv C:\Temp\gpu.csv -Append -NoTypeInformation #Format-Table -AutoSize
        }
        else {
            Write-Host "No GPU usage data found for process ID $processId."
        }
    }
    else {
        Write-Host "Process $applicationName not found."
    }
    Start-Sleep -Seconds 1
}