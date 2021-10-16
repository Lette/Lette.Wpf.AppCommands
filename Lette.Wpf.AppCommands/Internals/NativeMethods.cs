using System;

namespace Lette.Wpf.AppCommands.Internals
{
    internal static class NativeMethods
    {
        public const int WM_APPCOMMAND = 0x0319;
        public const int FAPPCOMMAND_MASK = 0xF000;

        public static int GetAppCommand(IntPtr lParam)
        {
            return ((short)(SignedHIWORD(IntPtrToInt32(lParam)) & ~FAPPCOMMAND_MASK));
        }

        public static int SignedHIWORD(int n)
        {
            return (short)((n >> 16) & 0xffff);
        }

        public static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }
    }
}
