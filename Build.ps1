<#
  .SYNOPSIS
  Performs a command-line build of the source code.

  .DESCRIPTION
  The Build.ps1 script updates the registry with new data generated
  during the past month and generates a report.

  .PARAMETER BuildAction
  Specifies the build action to be performed. Permissable input is Clean, Build, Test, or Package. Default is Build

  .PARAMETER TargetConfiguration
  Specifies the target configuration for the Build. Permissable input is Debug or Release. Default is Debug

  .PARAMETER TargetPlatform
  Specifies the target platform for the Build. Permissable input is AnyCPU, x86, or x64. Default is AnyCPU

  .INPUTS
  None. You cannot pipe objects to Build.ps1.

  .OUTPUTS
  Output from the "dotnet" build tool.

  .EXAMPLE
  PS> .\Build.ps1

  .EXAMPLE
  PS> .\Build.ps1 Build Debug

  .EXAMPLE
  PS> .\Build.ps1 -BuildAction Clean -TargetConfiguration Release
#>

param ([Parameter(Position=0)]
       [ValidateSet('Clean','Build','Test','Package')]
       [string]$BuildAction="Build", 
       
       [Parameter(Position=1)]
       [ValidateSet('Debug','Release')]
       [string]$TargetConfiguration="Debug", 
              
       [Parameter(,Position=2)]
       [ValidateSet('AnyCPU','x86','x64')]
       [string]$TargetPlatform="AnyCPU")

function OutputErrorMessage
{
    Write-Host "===============================" -ForegroundColor Red
    Write-Host "|  B u i l d   F a i l e d !  |" -ForegroundColor Red
    Write-Host "===============================" -ForegroundColor Red

    exit 1
}

$CurrentDate = Get-Date
$fileName = [string]::Format(".\out\logs\build-{0}-{1}.log", $CurrentDate.ToString("yyyyMMdd"), $CurrentDate.ToString("HHmmss"))

$SolutionFiles = Get-ChildItem -Path . -Filter *.sln* -Recurse

foreach ($FileToBuild in $SolutionFiles)
{
    switch ($BuildAction) {
        "Clean" 
        {  
            dotnet clean $FileToBuild -c $TargetConfiguration -o ./out /clp:verbosity=detailed /flp:Logfile=""$fileName""
            if ($lastexitcode -eq 1) {
                OutputErrorMessage
            }
        }

        "Build" 
        {  
            dotnet build $FileToBuild -c $TargetConfiguration -o ./out/build  /clp:verbosity=detailed /flp:Logfile=""$fileName""
            if ($lastexitcode -eq 1) {
                OutputErrorMessage
            }
        }

        "Test" 
        {
            dotnet test $FileToBuild --filter TestCategory=Unit --output ./out/build --logger trx
            if ($lastexitcode -eq 1) {
                OutputErrorMessage
            }
        }

        "Package" 
        {
            $BuildSettings = Get-Content .\BuildSettings.json | ConvertFrom-Json -AsHashtable

            #First remove the conainer images
            docker compose -f $BuildSettings.dockerComposeFile down

            docker compose -f $BuildSettings.dockerComposeFile up --build
            if ($lastexitcode -eq 1) {
                OutputErrorMessage
            }
        }
    }
}
