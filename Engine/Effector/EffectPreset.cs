using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayer_X_Ark.Engine.Effector
{
    public abstract class EffectPreset
    {
        // プリセット名
        public string Name { get; set; }

        // プリセットフォルダのルート
        public static readonly string PresetRoot =
            Path.Combine(Application.StartupPath, "Presets");

        // エフェクト名（サブクラスで定義）
        public abstract string EffectName { get; }

        // 保存先パス
        public string FilePath =>
            Path.Combine(PresetRoot, EffectName, Name + ".json");
        public static T Load<T>(string effectName, string name)
    where T : EffectPreset
        {
            var path = Path.Combine(PresetRoot, effectName, name + ".json");
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path, System.Text.Encoding.UTF8);
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }
        // 保存
        public void Save()
        {
            var dir = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var json = System.Text.Json.JsonSerializer.Serialize(
                this,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json, System.Text.Encoding.UTF8);
        }

        // 削除
        public void Delete()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        // 一覧取得
        public static List<string> GetPresetNames(string effectName)
        {
            var dir = Path.Combine(PresetRoot, effectName);
            if (!Directory.Exists(dir)) return new List<string>();
            return Directory.GetFiles(dir, "*.json")
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .OrderBy(n => n)
                .ToList();
        }
    }
}
