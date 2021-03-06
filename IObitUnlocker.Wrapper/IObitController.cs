﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace IObitUnlocker.Wrapper
{
    public static class IObitController
    {
        static bool init = false;

        static string SysPath = Path.Combine(Directory.GetCurrentDirectory(), "IObitUnlocker.sys");

        static string DLLPath = Path.Combine(Directory.GetCurrentDirectory(), "IObitUnlocker.dll");
        public static void Init()
        {
            File.WriteAllBytes(DLLPath, Properties.Resources.IObitUnlocker);
            File.WriteAllBytes(SysPath, Properties.Resources.IObitUnlockerSyS);
            init = true;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool IObitDriverBase();

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        delegate int IObitDriverUnlockFile(string file, ref uint unk1, FileOperation deleteFile, int unk2, out int unk3);


        public static bool DriverStart()
        {
            if (!init)
            {
                Init();
            }
            var dll = Native.LoadLibraryEx(DLLPath, IntPtr.Zero, LoadLibraryFlags.None);
            var addr = Native.GetProcAddress(dll, "DriverStart");
            var DriverStart = Marshal.GetDelegateForFunctionPointer<IObitDriverBase>(addr);
            return DriverStart();
        }

        public static bool DriverStop()
        {
            if (!init)
            {
                Init();
            }
            var dll = Native.LoadLibraryEx(DLLPath, IntPtr.Zero, LoadLibraryFlags.None);
            var addr = Native.GetProcAddress(dll, "DriverStop");
            var DriverStop = Marshal.GetDelegateForFunctionPointer<IObitDriverBase>(addr);
            return DriverStop();
        }

        public static int UnlockFile(string file, FileOperation operation)
        {
            if (!init)
            {
                Init();
            }
            var dll = Native.LoadLibraryEx(DLLPath, IntPtr.Zero, LoadLibraryFlags.None);
            var addr = Native.GetProcAddress(dll, "DriverUnlockFile");
            var DriverUnlockFile = Marshal.GetDelegateForFunctionPointer<IObitDriverUnlockFile>(addr);
            uint flag = 0xc08b0000;
            return DriverUnlockFile(file, ref flag, operation, 0, out int unk3);
        }
    }
}
