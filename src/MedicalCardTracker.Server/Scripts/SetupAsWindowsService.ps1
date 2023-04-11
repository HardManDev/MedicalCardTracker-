Set-Location ..

# Global variables
$serviceName = "MedicalCardTracker.Server"
$serviceDisplayName = "Medical Card Tracker Server"
$serviceDescription = 'Service for "Medical Card Tracker Server" (Software for tracking the transfer of medical cards within healthcare organizations). More info: https://github.com/HardManDev/MedicalCardTracker'
$serviceBinaryPathName = "$( $PWD.Path )\MedicalCardTracker.Server.exe --workingDirectory=$( $PWD.Path )"


function Write-Host-Header()
{
    [CmdletBinding()]
    param (
        [switch]$Clear
    )
    if ($Clear)
    {
        Clear-Host
    }

    Write-Host "Medical Card Tracker Server v0.0.1 - Windows service setup wizzard" -ForegroundColor Magenta
    Write-Host "Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>. All right reserved." -ForegroundColor Magenta
    Write-Host ""
}

Write-Host-Header -Clear

if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue)
{
    Write-Host "$serviceName service already exists." -ForegroundColor Yellow

    if ((Get-Service -Name $serviceName).Status -eq "Running")
    {
        Write-Host "$serviceName service running. An attempt is being made to stop..." -ForegroundColor Gray
        sc.exe stop $serviceName
        Write-Host "$serviceName was successfully stopped." -ForegroundColor Green
    }

    Write-Host "$serviceName service uninstall in progress..." -ForegroundColor Gray
    sc.exe delete $serviceName
    Write-Host "$serviceName was successfully uninstall." -ForegroundColor Green
}

Write-Host "$serviceName service install in progress..." -ForegroundColor Gray
New-Service -Name $serviceName `
            -DisplayName $serviceDisplayName `
            -Description $serviceDescription `
            -BinaryPathName $serviceBinaryPathName `
            -StartupType Automatic
Write-Host "$serviceName was successfully install." -ForegroundColor Green
Pause
