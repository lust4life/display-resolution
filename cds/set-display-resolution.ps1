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

function Set-DisplayResolution {
    param (
        [Parameter(Mandatory=$true, Position = 0)] 
        [int] 
        $Width, 

        [Parameter(Mandatory=$true, Position = 1)] 
        [int] 
        $Height,

        [CDSFlags]
        $Flag = [CDSFlags]::Dynamically
    )
    
    [cds.Helper]::ChangeDisplaySettings($width, $height, $flag)
}

$cds = @"
    namespace cds
    {
        using System;
        using System.Runtime.InteropServices;
    
        [StructLayout(LayoutKind.Explicit)]
        public struct DEVMODE
        {
            [FieldOffset(102)]
            public short dmLogPixels;
            [FieldOffset(104)]
            public int dmBitsPerPel;
            [FieldOffset(108)]
            public int dmPelsWidth;
            [FieldOffset(112)]
            public int dmPelsHeight;
            [FieldOffset(116)]
            public int dmDisplayFlags;
            [FieldOffset(120)]
            public int dmDisplayFrequency;
        }
    
    
        // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changedisplaysettingsa
        public class Helper
        {
            public static DEVMODE GetDisplaySettings()
            {
                // #define ENUM_CURRENT_SETTINGS       ((DWORD)-1)
                // #define ENUM_REGISTRY_SETTINGS      ((DWORD)-2)
                var devMode = new DEVMODE();
    
                // A NULL value specifies the current display device on the computer on which the calling thread is running.
                var retCode = EnumDisplaySettings(null, -1, ref devMode);
                if (retCode == 0)
                {
                    throw new Exception("can't get resolution from win api");
                }
    
                return devMode;
            }
    
            public static string ChangeDisplaySettings(int width, int height, int flags)
            {
                // /* Return values for ChangeDisplaySettings */
                // #define DISP_CHANGE_SUCCESSFUL       0
                // #define DISP_CHANGE_RESTART          1
                // #define DISP_CHANGE_FAILED          -1
                // #define DISP_CHANGE_BADMODE         -2
                // #define DISP_CHANGE_NOTUPDATED      -3
                // #define DISP_CHANGE_BADFLAGS        -4
                // #define DISP_CHANGE_BADPARAM        -5
                // #if(_WIN32_WINNT >= 0x0501)
                // #define DISP_CHANGE_BADDUALVIEW     -6
                // #endif /* _WIN32_WINNT >= 0x0501 */
    
                var devMode = GetDisplaySettings();
    
                devMode.dmPelsWidth = width;
                devMode.dmPelsHeight = height;
    
                var res = ChangeDisplaySettings(ref devMode, flags);
                switch (res)
                {
                    case 0:
                        return "The settings change was successful.";
                    case 1:
                        return "The computer must be restarted for the graphics mode to work.";
                    case -1:
                        return "The display driver failed the specified graphics mode.";
                    case -2:
                        return "The graphics mode is not supported.";
                    case -3:
                        return "Unable to write settings to the registry.";
                    case -4:
                        return "An invalid set of flags was passed in.";
                    case -5:
                        return "An invalid parameter was passed in. This can include an invalid flag or combination of flags.";
                    case -6:
                        return "The settings change was unsuccessful because the system is DualView capable.";
                    default:
                        return "unknow return code: " + res;
                }
            }
    
            [DllImport("user32.dll")]
            static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
    
            [DllImport("user32.dll")]
            public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);
    
            public class Flags
            {
                // #define CDS_UPDATEREGISTRY           0x00000001
                // #define CDS_TEST                     0x00000002
                // #define CDS_FULLSCREEN               0x00000004
                // #define CDS_GLOBAL                   0x00000008
                // #define CDS_SET_PRIMARY              0x00000010
                // #define CDS_RESET                    0x40000000
                // #define CDS_NORESET                  0x10000000
    
                public const int CDS_UPDATEREGISTRY = 0x01;
                public const int CDS_TEST = 0x02;
                public const int CDS_FULLSCREEN = 0x04;
                public const int CDS_GLOBAL = 0x08;
                public const int CDS_SET_PRIMARY = 0x10;
                public const int CDS_RESET = 0x40000000;
                public const int CDS_NORESET = 0x10000000;
            }
        }
    }    
"@
    
Add-Type -TypeDefinition $cds