# Path to your JSON file
$jsonFilePath = "result.json"

# Import the JSON file
$jsonData = Get-Content -Path $jsonFilePath -Raw | ConvertFrom-Json

$jsonData.results.results | ForEach-Object {
    $rawObject = $_.prompt.raw | ConvertFrom-Json
    $question = $rawObject | Select-Object content -Last 1
    $answer = $_.response.output
    [PSCustomObject]@{
        "Question" = $question
        "Answer"   = $answer
    }
}
| Export-Excel result.xlsx
#Format-Table -AutoSize

