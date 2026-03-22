using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Engine.Config
{
    /// <summary>
    /// Windows レジストリへのファイル関連付けを管理する静的クラス。
    /// HKCU\Software\Classes を使用するため管理者権限不要。
    /// システム全体への登録が必要な場合は管理者として再起動する。
    /// </summary>
    public static class FileAssociationManager
    {
        private const string ProgIdBase      = "MediaPlayerXArk";
        private const string AppFriendlyName = "MediaPlayer X Ark";

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private const uint SHCNE_ASSOCCHANGED = 0x08000000;
        private const uint SHCNF_IDLIST       = 0x0000;

        // ─────────────────────────────────────────
        //  公開 API
        // ─────────────────────────────────────────

        /// <summary>指定した拡張子が本アプリに関連付けられているか確認する。</summary>
        public static bool IsRegistered(string ext)
        {
            ext = NormalizeExt(ext);
            string progId = MakeProgId(ext);
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey($@"Software\Classes\{ext}");
                return key != null && (key.GetValue(null) as string) == progId;
            }
            catch { return false; }
        }

        /// <summary>
        /// 拡張子を本アプリに関連付ける（HKCU）。
        /// </summary>
        public static void RegisterExtension(string ext, string exePath)
        {
            ext = NormalizeExt(ext);
            string progId = MakeProgId(ext);
            string quoted  = $"\"{exePath}\"";

            // ProgID キーを登録
            using (var progKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}"))
            {
                progKey.SetValue(null, $"{AppFriendlyName} {ext.ToUpper().TrimStart('.')} File");

                using (var iconKey = progKey.CreateSubKey("DefaultIcon"))
                    iconKey.SetValue(null, $"{quoted},0");

                using (var openCmd = progKey.CreateSubKey(@"shell\open\command"))
                    openCmd.SetValue(null, $"{quoted} \"%1\"");

                using (var openKey = progKey.OpenSubKey("shell\\open", writable: true))
                    openKey?.SetValue("FriendlyAppName", AppFriendlyName);

                // プレイリストへ追加コンテキストメニュー
                using (var enqKey = progKey.CreateSubKey("shell\\enqueue"))
                    enqKey.SetValue(null, "プレイリストに追加 (&A)");

                using (var enqCmd = progKey.CreateSubKey(@"shell\enqueue\command"))
                    enqCmd.SetValue(null, $"{quoted} /enqueue \"%1\"");
            }

            // 拡張子キーにデフォルト ProgID をセット
            using (var extKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{ext}"))
            {
                extKey.SetValue(null, progId);
                using (var owKey = extKey.CreateSubKey("OpenWithProgids"))
                    owKey.SetValue(progId, Array.Empty<byte>(), RegistryValueKind.Binary);
            }

            // Capabilities（RegisteredApplications 向け）
            string capPath = $@"Software\Clients\Media\{ProgIdBase}\Capabilities";
            using (var capKey = Registry.CurrentUser.CreateSubKey(capPath))
            {
                capKey.SetValue("ApplicationName",        AppFriendlyName);
                capKey.SetValue("ApplicationDescription", $"{AppFriendlyName} Media Player");
                using (var faKey = capKey.CreateSubKey("FileAssociations"))
                    faKey.SetValue(ext, progId);
            }

            using (var regApp = Registry.CurrentUser.CreateSubKey(@"Software\RegisteredApplications"))
                regApp.SetValue(ProgIdBase, capPath);
        }

        /// <summary>
        /// 拡張子の関連付けを解除する。他のアプリの設定は消去しない。
        /// </summary>
        public static void UnregisterExtension(string ext)
        {
            ext = NormalizeExt(ext);
            string progId = MakeProgId(ext);

            try { Registry.CurrentUser.DeleteSubKeyTree($@"Software\Classes\{progId}", throwOnMissingSubKey: false); }
            catch { }

            try
            {
                using var extKey = Registry.CurrentUser.OpenSubKey($@"Software\Classes\{ext}", writable: true);
                if (extKey != null)
                {
                    if ((extKey.GetValue(null) as string) == progId)
                        extKey.DeleteValue("", throwOnMissingValue: false);

                    using var owKey = extKey.OpenSubKey("OpenWithProgids", writable: true);
                    owKey?.DeleteValue(progId, throwOnMissingValue: false);
                }
            }
            catch { }

            try
            {
                using var faKey = Registry.CurrentUser.OpenSubKey(
                    $@"Software\Clients\Media\{ProgIdBase}\Capabilities\FileAssociations", writable: true);
                faKey?.DeleteValue(ext, throwOnMissingValue: false);
            }
            catch { }
        }

        /// <summary>複数の拡張子を一括登録し、Shell に通知する。</summary>
        public static void RegisterExtensions(IEnumerable<string> extensions, string exePath)
        {
            foreach (var ext in extensions)
                RegisterExtension(ext, exePath);
            NotifyShell();
        }

        /// <summary>複数の拡張子を一括解除し、Shell に通知する。</summary>
        public static void UnregisterExtensions(IEnumerable<string> extensions)
        {
            foreach (var ext in extensions)
                UnregisterExtension(ext);
            NotifyShell();
        }

        /// <summary>本アプリが登録した全ての拡張子の関連付けを解除する。</summary>
        public static void UnregisterAll()
        {
            try
            {
                using var faKey = Registry.CurrentUser.OpenSubKey(
                    $@"Software\Clients\Media\{ProgIdBase}\Capabilities\FileAssociations");
                if (faKey != null)
                    UnregisterExtensions(faKey.GetValueNames());
            }
            catch { }
            NotifyShell();
        }

        /// <summary>Shell（エクスプローラー）に関連付け変更を通知する。</summary>
        public static void NotifyShell()
        {
            try { SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero); }
            catch { }
        }

        /// <summary>現在のプロセスが管理者権限で実行されているか確認する。</summary>
        public static bool IsRunningAsAdmin()
        {
            try
            {
                using var identity = WindowsIdentity.GetCurrent();
                return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch { return false; }
        }

        /// <summary>管理者権限で自身を再起動する。</summary>
        /// <returns>再起動を試みた場合 true（ユーザーキャンセル時は false）</returns>
        public static bool RestartAsAdmin()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName        = Application.ExecutablePath,
                    UseShellExecute = true,
                    Verb            = "runas",
                    Arguments       = "/admin"
                });
                return true;
            }
            catch (System.ComponentModel.Win32Exception) { return false; }
        }

        // ─────────────────────────────────────────
        //  内部ユーティリティ
        // ─────────────────────────────────────────

        /// <summary>".mp3" or "mp3" → ".mp3"</summary>
        private static string NormalizeExt(string ext)
        {
            ext = ext.Trim().ToLower();
            return ext.StartsWith(".") ? ext : "." + ext;
        }

        /// <summary>".mp3" → "MediaPlayerXArk_mp3"</summary>
        private static string MakeProgId(string ext)
            => $"{ProgIdBase}_{ext.TrimStart('.')}";
    }
}
