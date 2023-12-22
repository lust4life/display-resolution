# Change Display Settings
Changes display settings in *Windows* from Powershell or .Net tool.

# Usage
## Powershell

Install
``` powershell
Import-Module set-display-resolution.psm1
```

Usage:
``` powershell
Set-DisplayResolution(1024,768,CDSFlags.SetPrimary);

Set-DisplayResolution -Width 1024 -Height 768
Set-DisplayResolution -Width 1024 -Height 768 -RefreshRate 60
Set-DisplayResolution -Width 1024 -Height 768 -RefreshRate 60 -flag "t"

Get-DisplayResolution
```

## .Net Tool

Install:
```
dotnet tool install --global ChangeDisplaySettings
```

Usage:
```
ChangeDisplaySettings -w 1024 -h 768

...

ChangeDisplaySettings --help

...

  -w, --width     Required. Width of the screen

  -h, --height    Required. Height of the screen

  -r, --refreshrate Optional. Refresh rate of the screen

  -t, --test      (Default: false) Test resolution change or actually perform the change.

  --help          Display this help screen.

  --version       Display version information.
```

# Resources
- https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changedisplaysettingsa
- https://github.com/tpn/winsdk-10/blob/9b69fd26ac0c7d0b83d378dba01080e93349c2ed/Include/10.0.10240.0/um/WinUser.h#L12522
- https://github.com/tpn/winsdk-10/blob/9b69fd26ac0c7d0b83d378dba01080e93349c2ed/Include/10.0.10240.0/um/wingdi.h#L2315
- https://devblogs.microsoft.com/scripting/use-powershell-to-interact-with-the-windows-api-part-1/