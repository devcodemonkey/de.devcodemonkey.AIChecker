function New-SetupFile {
    param (
        $version
    )        
    $innoPath = "C:\Program Files (x86)\Inno Setup 6"

    $secureFtpPassword = Read-Host -AsSecureString "Enter ftp password"

    Set-Location $PSScriptRoot
    # change version in the vs project file
    Set-Location .\AIChecker\
    [xml]$vsProjectFile = Get-Content .\AIChecker.csproj
    $vsProjectFile.Project.PropertyGroup.Version = $version
    $vsProjectFile.Save(".\AIChecker\AIChecker.csproj")

    # change version in the installer script
    Set-location ..           

    $fileName = "Setup_$($version -replace '\.','_')"
    (Get-Content .\InstallerInno.iss) -replace "#define MyAppVersion ""##testing##""", "#define MyAppVersion ""$version""" -replace "OutputBaseFilename=Setup", "OutputBaseFilename=$fileName" | Set-Content .\InstallerInno.iss
    
    # build the project
    Set-Location .\AIChecker\
    . dotnet publish -c Release -o .\bin\publish
    Set-Location ..
    ."$innoPath\ISCC.exe" InstallerInno.iss        

    # create revision number
    $revisionNumber = (Get-Date).ToString("yyyyMMddHHmmss")
    Set-Location Output
    New-Item -ItemType Directory -Path $revisionNumber    
    Move-Item "$fileName.exe" $revisionNumber   

    # create a json revision file

    $jsonFileName = "revision.json"
    $jsonContent = @()

    if (Test-Path $jsonFileName) {
        $jsonContent = Get-Content -Path $jsonFileName | ConvertFrom-Json
        if ($jsonContent -isnot [array]) {
            $jsonContent = @($jsonContent)  # Convert to an array if it's not already one
        }
    }    

    $jsonObject = @(
        @{            
            revision = "$revisionNumber";
            file     = "$fileName.exe";            
            version  = "$version";            
        }
    )

    $jsonContent += $jsonObject
    $jsonString = $jsonContent | ConvertTo-Json -Depth 3
    Set-Content -Path $jsonFileName -Value $jsonString

    # Upload to FTP
    # Script from https://gist.github.com/TheUltimateC0der/dd59fa051c3f797c5a79a38df2e1bdc1

    # FTP Server Variables
    $FTPHost = 'ftp://202.61.232.190/'
    $FTPUser = 'aichecker'
    $FTPPass = [Runtime.InteropServices.Marshal]::PtrToStringAuto(
        [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureFtpPassword)
    )
  
    #Directory where to find pictures to upload
    $UploadFolder = (Get-Item -Path ".\").FullName    
   
    $webclient = New-Object System.Net.WebClient 
    $webclient.Credentials = New-Object System.Net.NetworkCredential($FTPUser, $FTPPass)  
  
    $SrcEntries = Get-ChildItem $UploadFolder -Recurse
    $Srcfolders = $SrcEntries | Where-Object { $_.PSIsContainer }
    $SrcFiles = $SrcEntries | Where-Object { !$_.PSIsContainer }
  
    # Create FTP Directory/SubDirectory If Needed - Start
    foreach ($folder in $Srcfolders) {    
        $SrcFolderPath = $UploadFolder -replace "\\", "\\" -replace "\:", "\:"  
        $DesFolder = $folder.Fullname -replace $SrcFolderPath, $FTPHost
        $DesFolder = $DesFolder -replace "\\", "/"
        # Write-Output $DesFolder
  
        try {
            $makeDirectory = [System.Net.WebRequest]::Create($DesFolder);
            $makeDirectory.Credentials = New-Object System.Net.NetworkCredential($FTPUser, $FTPPass);
            $makeDirectory.Method = [System.Net.WebRequestMethods+FTP]::MakeDirectory;
            $makeDirectory.GetResponse();
            #folder created successfully
        }
        catch [Net.WebException] {
            try {
                #if there was an error returned, check if folder already existed on server
                $checkDirectory = [System.Net.WebRequest]::Create($DesFolder);
                $checkDirectory.Credentials = New-Object System.Net.NetworkCredential($FTPUser, $FTPPass);
                $checkDirectory.Method = [System.Net.WebRequestMethods+FTP]::PrintWorkingDirectory;
                $response = $checkDirectory.GetResponse();
                #folder already exists!
            }
            catch [Net.WebException] {
                #if the folder didn't exist
            }
        }
    }
    # Create FTP Directory/SubDirectory If Needed - Stop
  
    # Upload Files - Start
    foreach ($entry in $SrcFiles) {
        $SrcFullname = $entry.fullname
        $SrcName = $entry.Name
        $SrcFilePath = $UploadFolder -replace "\\", "\\" -replace "\:", "\:"
        $DesFile = $SrcFullname -replace $SrcFilePath, $FTPHost
        $DesFile = $DesFile -replace "\\", "/"
        # Write-Output $DesFile
  
        $uri = New-Object System.Uri($DesFile) 
        $webclient.UploadFile($uri, $SrcFullname)
    }
    # Upload Files - Stop
}

New-SetupFile -version 0.0.1-alpha