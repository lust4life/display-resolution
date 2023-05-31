namespace cds
{
    using System;
    using System.Runtime.InteropServices;


    /// <summary>
    /// See: https://learn.microsoft.com/en-us/windows/win32/api/wingdi/ns-wingdi-devmodea
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct DEVMODE
    {
        /// <summary>The number of pixels per logical inch. Printer drivers do not use this member.</summary>
        [FieldOffset(102)]
        public short dmLogPixels;

        /// <summary>Bits per pixel</summary>
        /// <remarks>Specifies the color resolution, in bits per pixel, of the display device (for example: 4 bits for 16 colors, 8 bits for 256 colors, or 16 bits for 65,536 colors).</remarks>
        [FieldOffset(104)]
        public int dmBitsPerPel;

        /// <summary>Pixel height</summary>
        [FieldOffset(108)]
        public int dmPelsWidth;

        /// <summary>Pixel width</summary>
        [FieldOffset(112)]
        public int dmPelsHeight;

        /// <summary>Mode flags</summary>
        [FieldOffset(116)]
        public int dmDisplayFlags;

        /// <summary>Mode frequency</summary>
        [FieldOffset(120)]
        public int dmDisplayFrequency;
    }



    /// <summary>
    /// See: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-changedisplaysettingsa
    /// </summary>
    public class Helper
    {

        /// <summary>
        /// Gets the display settings.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">can't get resolution from win api</exception>
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

        /// <summary>
        /// Changes the display settings.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static string ChangeDisplaySettings(int width, int height, Flags flags)
        {
            return ChangeDisplaySettings(width, height, flags, out var _);
        }

        /// <summary>
        /// Changes the display settings.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="success">if set to <c>true</c> the change was successful.</param>
        /// <returns></returns>
        public static string ChangeDisplaySettings(int width, int height, Flags flags, out bool success)
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

            var res = ChangeDisplaySettings(ref devMode, (int)flags);
            success = false;
            switch (res)
            {
                case 0:
                    success = true;
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
        private static extern int EnumDisplaySettings(string? deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport("user32.dll")]
        private static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

        /// <summary>
        /// Resolution chgange flags
        /// </summary>
        [Flags]
        public enum Flags:int
        {
            // #define CDS_UPDATEREGISTRY           0x00000001
            // #define CDS_TEST                     0x00000002
            // #define CDS_FULLSCREEN               0x00000004
            // #define CDS_GLOBAL                   0x00000008
            // #define CDS_SET_PRIMARY              0x00000010
            // #define CDS_RESET                    0x40000000
            // #define CDS_NORESET                  0x10000000

            /// <summary/>
            CDS_UPDATEREGISTRY = 0x01,
            /// <summary/>
            CDS_TEST = 0x02,
            /// <summary/>
            CDS_FULLSCREEN = 0x04,
            /// <summary/>
            CDS_GLOBAL = 0x08,
            /// <summary/>
            CDS_SET_PRIMARY = 0x10,
            /// <summary/>
            CDS_RESET = 0x40000000,
            /// <summary/>
            CDS_NORESET = 0x10000000
        }
    }
}