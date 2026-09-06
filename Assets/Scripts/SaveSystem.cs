using System;
using System.IO;
using UnityEngine;

namespace HxHGame
{
    public sealed class SaveSystem
    {
        private readonly string root;
        public SaveSystem(string overrideRoot = null) { root = overrideRoot ?? Path.Combine(Application.persistentDataPath, "Saves"); }
        public string PathFor(int slot) => Path.Combine(root, $"slot_{Mathf.Clamp(slot, 1, 6)}.json");
        public void Save(int slot, SaveData data)
        {
            Directory.CreateDirectory(root);
            string target = PathFor(slot), temp = target + ".tmp", backup = target + ".bak";
            File.WriteAllText(temp, JsonUtility.ToJson(data, true));
            if (File.Exists(target)) File.Copy(target, backup, true);
            if (File.Exists(target)) File.Delete(target);
            File.Move(temp, target);
        }
        public SaveData Load(int slot)
        {
            string path = PathFor(slot);
            if (!File.Exists(path)) return null;
            try { var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path)); return data != null && data.version == 1 ? data : null; }
            catch (Exception error) { Debug.LogWarning($"Save slot rejected: {error.Message}"); return null; }
        }
    }
}

