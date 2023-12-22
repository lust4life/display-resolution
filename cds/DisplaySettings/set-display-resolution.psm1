[Flags()] enum CDSFlags {
    Dynamically = 0
    UpdateRegistry = 0x01
    Test = 0x02
    FullScreen = 0x04
    Global = 0x08
    SetPrimary = 0x10
    Reset = 0x40000000
    NoReset = 0x10000000
}

<#
.LINK
https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changedisplaysettingsa#parameters
#>
function Set-DisplayResolution {
    param (
        [Parameter(Mandatory = $true, Position = 0)] 
        [int] 
        $Width, 

        [Parameter(Mandatory = $true, Position = 1)] 
        [int] 
        $Height,

        [Parameter(Mandatory = $false, Position = 2)] 
        [int] 
        $RefreshRate=0,

        [CDSFlags]
        $Flag = [CDSFlags]::Dynamically
    )
    
    [cds.Helper]::ChangeDisplaySettings($width, $height, $refreshrate, $flag)
}

function Get-DisplayResolution {
    [cds.Helper]::GetDisplaySettings()
}

$cds = Get-Content $PSScriptRoot/CDS.cs -Raw
Add-Type -TypeDefinition $cds
