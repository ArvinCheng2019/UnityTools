using UnityEngine;
using UnityEditor;
using System.IO;

namespace Arvin
{
    public class ScriptableHelper
    {
        private static string basePath = Application.dataPath + "/Scriptables/";

        private static void checkFolder()
        {
            if (Directory.Exists(basePath))
            {
                return;
            }

            Directory.CreateDirectory(basePath);
        }

        public static TextureOptimization GetTextureOptimization()
        {
            checkFolder();
            string file =   "Assets/Scriptables/TextureOptimization.asset";
            var importInfo = AssetDatabase.LoadAssetAtPath<TextureOptimization>(file);
            if (importInfo == null)
            {
                importInfo = ScriptableObject.CreateInstance<TextureOptimization>();
                AssetDatabase.CreateAsset(importInfo, file);
                AssetDatabase.SaveAssets();
            }

            return importInfo;
        }

        public static SoundOptimization GetSoundOptimization()
        {
            checkFolder();
            string file = "Assets/Scriptables/SoundOptimization.asset";
            var importInfo = AssetDatabase.LoadAssetAtPath<SoundOptimization>(file);
            if (importInfo == null)
            {
                importInfo = ScriptableObject.CreateInstance<SoundOptimization>();
                AssetDatabase.CreateAsset(importInfo, file);
                AssetDatabase.SaveAssets();
            }

            return importInfo;
        }
    }
}