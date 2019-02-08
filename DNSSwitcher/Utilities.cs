using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DNSSwitcher
{
    /// <summary>
    /// Class with utility functions.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Application name.
        /// </summary>
        private const string AppName = "DnsSwitcher";

        /// <summary>
        /// Path to the registry key where it should be register as start up.
        /// </summary>
        private const string RegistryPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        /// <summary>
        /// First part of the startup script.
        /// </summary>
        private const string FirstStartUpScriptPart =
            "Set WshShell = CreateObject(\"WScript.Shell\" )\nWshShell.Run \"\"\"";

        /// <summary>
        /// Second part of the startup script.
        /// </summary>
        private const string SecondStartUpScriptPart = "\"\"\", 0\nSet WshShell = Nothing";

        /// <summary>
        /// Is the app registered as a start up program?
        /// </summary>
        public static bool IsStartUpProgram =>
            Registry.CurrentUser.OpenSubKey(RegistryPath, true)?.GetValue(AppName) != null;

        /// <summary>
        /// Path to the start up launcher.
        /// </summary>
        private static string LauncherPath =>
            "\"" + Directory.GetParent(Application.ExecutablePath).FullName + "\\StartUp.vbs\"";

        /// <summary>
        /// Creates the startup script.
        /// This script is needed as a workaround for running a program on startup with admin permissions.
        /// Refer to: https://superuser.com/questions/929225/how-to-run-a-program-as-an-administrator-at-startup-on-windows-10
        /// </summary>
        private static void CreateStartUpScript() =>
            File.WriteAllText("StartUp.vbs",
                FirstStartUpScriptPart + Application.ExecutablePath + SecondStartUpScriptPart);

        /// <summary>
        /// Register as a start up program on Windows or unregister if it's not needed.
        /// </summary>
        public static void SwitchStartUpProgram()
        {
            CreateStartUpScript();
            Console.WriteLine(RegistryPath);
            using (var rk = Registry.CurrentUser.OpenSubKey(RegistryPath, true))
            {
                if (!IsStartUpProgram)
                    rk?.SetValue(AppName, LauncherPath, RegistryValueKind.String);
                else
                    rk?.DeleteValue(AppName, false);
                Console.WriteLine(rk?.ValueCount);
            }
        }
    }
}