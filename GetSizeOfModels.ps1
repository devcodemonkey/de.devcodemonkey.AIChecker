# Define the URL to fetch
$url = "https://huggingface.co/models?library=gguf&language=de&sort=likes&search=thebloke"

# Send a web request to the URL
$response = Invoke-WebRequest -Uri $url

# Extract all the links from the content
$links = $response.Links | Where-Object { $_.href -like "*/TheBloke*" }

# Display the links
foreach ($link in $links) {
    Write-Output $link.href
    $url = "https://huggingface.co$($link.href)"
    $response = Invoke-WebRequest -Uri $url
    # Extract all the tables from the content
    $tables = $response.ParsedHtml.getElementsByTagName("table")

    # Create a list to store the table objects
    $tableObjects = @()
    # Loop through each table
    foreach ($table in $tables) {
        $rows = $table.getElementsByTagName("tr")

        # Get the headers from the first row
        $headers = $rows[0].getElementsByTagName("th") | ForEach-Object { $_.innerText }
        
        $tables = $response.ParsedHtml.getElementsByTagName("table")

        # Create a list to store the table objects
        $tableObjects = @()
        # Loop through each table
        foreach ($table in $tables) {
            $rows = $table.getElementsByTagName("tr")

            # Get the headers from the first row
            $headers = $rows[0].getElementsByTagName("th") | ForEach-Object { $_.innerText }

            # Loop through each row (skipping the header row)
            for ($i = 1; $i -lt $rows.length; $i++) {
                $row = $rows[$i]
                $cells = $row.getElementsByTagName("td")

                # Create a hashtable to store the cell data
                $rowData = @{}
                for ($j = 0; $j -lt $cells.length; $j++) {
                    $rowData[$headers[$j]] = $cells[$j].innerText
                }        

                # Add the URL to the hashtable
                $rowData["URL"] = $url

                # Convert the hashtable to a custom object and add it to the list
                $tableObjects += New-Object PSObject -Property $rowData
            }
        }

        # Output the table objects
        $tableObjects | Format-Table -AutoSize

        # Define the path for the CSV file
        $csvPath = "output.xlsx"

        # Export the table objects to the CSV file
        $tableObjects | Export-Excel -Path $csvPath -Append
    }
}