using System;
using System.Runtime.InteropServices;


namespace OSharp.Wpf.Utilities
{
    /// <summary>
    ///     User32.dll 本地方法调用
    /// </summary>
    public static class User32
    {
        /// <summary>
        ///     The flash status stored in <see cref="T:PInvoke.User32.FLASHWINFO" /> and used in
        ///     <see cref="M:PInvoke.User32.FlashWindowEx(PInvoke.User32.FLASHWINFO@)" />.
        /// </summary>
        public enum FlashWindowFlags
        {
            /// <summary>Stop flashing. The system restores the window to its original state.</summary>
            FLASHW_STOP = 0,
            /// <summary>Flash the window caption.</summary>
            FLASHW_CAPTION = 1,
            /// <summary>Flash the taskbar button.</summary>
            FLASHW_TRAY = 2,
            /// <summary>
            ///     Flash both the window caption and taskbar button. This is equivalent to setting the FLASHW_CAPTION |
            ///     FLASHW_TRAY flags.
            /// </summary>
            FLASHW_ALL = 3,
            /// <summary>Flash continuously, until the <see cref="F:PInvoke.User32.FlashWindowFlags.FLASHW_STOP" /> flag is set.</summary>
            FLASHW_TIMER = 4,
            /// <summary>Flash continuously until the window comes to the foreground.</summary>
            FLASHW_TIMERNOFG = 12 // 0x0000000C
        }


        /// <summary>Flashes the specified window. It does not change the active state of the window.</summary>
        /// <param name="pwfi">A pointer to a <see cref="T:PInvoke.User32.FLASHWINFO" /> structure.</param>
        /// <returns>
        ///     The return value specifies the window's state before the call to the FlashWindowEx function. If the window
        ///     caption was drawn as active before the call, the return value is nonzero. Otherwise, the return value is zero.
        /// </returns>
        [DllImport("User32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);


        /// <summary>
        ///     Contains the flash status for a window and the number of times the system should flash the window. Used in
        ///     <see cref="M:PInvoke.User32.FlashWindowEx(PInvoke.User32.FLASHWINFO@)" />.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FLASHWINFO
        {
            /// <summary>The size of the structure, in bytes.</summary>
            public int cbSize;
            /// <summary>A handle to the window to be flashed. The window can be either opened or minimized.</summary>
            public IntPtr hwnd;
            /// <summary>The flash status</summary>
            public FlashWindowFlags dwFlags;
            /// <summary>The number of times to flash the window.</summary>
            public int uCount;
            /// <summary>
            ///     The rate at which the window is to be flashed, in milliseconds. If
            ///     <see cref="F:PInvoke.User32.FLASHWINFO.dwTimeout" /> is zero, the
            ///     function uses the default cursor blink rate.
            /// </summary>
            public int dwTimeout;

            /// <summary>
            ///     Create a new instance of <see cref="T:PInvoke.User32.FLASHWINFO" /> with
            ///     <see cref="F:PInvoke.User32.FLASHWINFO.cbSize" /> set to the correct value.
            /// </summary>
            /// <returns>
            ///     A new instance of <see cref="T:PInvoke.User32.FLASHWINFO" /> with
            ///     <see cref="F:PInvoke.User32.FLASHWINFO.cbSize" /> set to the correct value.
            /// </returns>
            public static FLASHWINFO Create()
            {
                return new FLASHWINFO()
                {
                    cbSize = Marshal.SizeOf(typeof(FLASHWINFO))
                };
            }
        }
    }
}