﻿#region

using System;
using System.IO;
using LeagueSharp.CommonEx.Core.Utils;
using SharpDX.Direct3D9;

#endregion

namespace LeagueSharp.CommonEx.Core
{
    /// <summary>
    ///     Constant values of LeagueSharp.CommonEx
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     LeagueSharp Font.
        /// </summary>
        public static Font LeagueSharpFont = new Font(
            Drawing.Direct3DDevice, 14, 0, FontWeight.DoNotCare, 0, false, FontCharacterSet.Default,
            FontPrecision.Default, FontQuality.Default, FontPitchAndFamily.DontCare | FontPitchAndFamily.Decorative,
            "Tahoma");

        /// <summary>
        ///     The directory where logs will be created.
        /// </summary>
        public static string LogDirectory
        {
            get
            {
                return
                    Cache.Instance.AddOrGetExisting(
                        "LogDirectory", () => Path.Combine(LeagueSharpAppData, "Logs", "CommonEx")).ToString();
            }
        }

        /// <summary>
        ///     The current filename that the logger will write to.
        /// </summary>
        public static string LogFileName
        {
            get
            {
                return
                    Cache.Instance.AddOrGetExisting(
                        "LogFileName", () => DateTime.Now.ToString("d").Replace('/', '-') + ".log").ToString();
            }
        }

        /// <summary>
        ///     Gets the LeagueSharp AppData directory.
        /// </summary>
        public static string LeagueSharpAppData
        {
            get
            {
                return
                    Cache.Instance.AddOrGetExisting(
                        "LeagueSharpDir",
                        () =>
                            Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "LS" + Environment.UserName.GetHashCode().ToString("X"))).ToString();
            }
        }

        /// <summary>
        /// Returns the Key that is used to show the menu.
        /// </summary>
        public static byte ShowMenuPressKey
        {                
            get
            {
                var key = (byte)Sandbox.SandboxConfig.MenuKey;
                key = key == 0 ? (byte)16 : key;
                key = VirtualKey.FixVirtualKey(key);
                return key;
            }
        }

        /// <summary>
        /// Returns the Key that is used to show the menu (toggle).
        /// </summary>
        public static byte ShowMenuToggleKey
        {
            get
            {
                var key = (byte)Sandbox.SandboxConfig.MenuToggleKey;
                key = key == 0 ? (byte)120 : key;
                key = VirtualKey.FixVirtualKey(key);
                return key;
            }
        }
    }
}