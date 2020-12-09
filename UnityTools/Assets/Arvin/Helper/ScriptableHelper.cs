using UnityEngine;
using UnityEditor;
using System.IO;

namespace Arvin
{
    public class ScriptableHelper
    {
        private static string basePath = Application.dataPath + "/Scriptables/";

        private static void existsDirecttory()
        {
            if (Directory.Exists(basePath))
            {
                return;
            }

            Directory.CreateDirectory(basePath);
        }

        private static string getFilePath(string fileName)
        {
            existsDirecttory();
            return $"Assets/Scriptables/{fileName}";
        }

        public static TextureOptimization GetTextureOptimization()
        {
            string file = getFilePath("TextureOptimization.asset");
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
            string file = getFilePath("SoundOptimization.asset");
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