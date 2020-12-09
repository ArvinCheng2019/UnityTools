using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arvin
{
    public class TextureOptimization : ScriptableObject
    {
        public List<TextureFolderData> TextureOptimizations = new List<TextureFolderData>();

        public void AddTexturePath(string path)
        {
           var pathed = TextureOptimizations.Find(item => { return item.Path.Equals(path); });
           if (pathed == null)
           {
               TextureOptimizations.Add( new TextureFolderData()
               {
                   Compression =  TextureImporterFormat.ASTC_RGBA_6x6,
                   Path =  path,
                   Platform = TextureFolderData.OptimizationPlatform.Android_iOS
               });
           }
        }

        public void RemoveTextures( string path )
        {
            var pathed = TextureOptimizations.Find(item => { return item.Path.Equals(path); });
            if (pathed == null)
            {
                return;
            }

            TextureOptimizations.Remove(pathed);
        }

       public void Run()
        {
            foreach (var data in TextureOptimizations)
            {
                EditorUtility.DisplayProgressBar("正在压缩图片", $"正在压缩{data.Path} 下的图片", 0);
                string[] guids = AssetDatabase.FindAssets("t:Texture", new[] {data.Path});
                int index = 0, max = guids.Length;
                foreach (var guid in guids)
                {
                    EditorUtility.DisplayProgressBar("正在压缩图片", $"正在压缩{data.Path} 下的图片,压缩格式为{data.Compression}",
                        (float) index / (float) max);
                    compresssTexture(guid, data.Platform, data.Compression);
                    index++;
                }
            }

            EditorUtility.ClearProgressBar();
        }

         void compresssTexture(string guid, TextureFolderData.OptimizationPlatform platform,
            TextureImporterFormat format)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string suffix = path.ToLower();
            if (suffix.EndsWith(".png") || suffix.EndsWith(".tga") || suffix.EndsWith(".jpg") ||
                suffix.EndsWith(".psd"))
            {
                TextureImporter importer = (TextureImporter) AssetImporter.GetAtPath(path);
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Bilinear;

                if (platform == TextureFolderData.OptimizationPlatform.Android ||
                    platform == TextureFolderData.OptimizationPlatform.Android_iOS)
                {
                    var androidSetting = new TextureImporterPlatformSettings();
                    importer.GetDefaultPlatformTextureSettings().CopyTo(androidSetting);
                    androidSetting.format = format;
                    androidSetting.overridden = true;
                    androidSetting.name = "android";
                    importer.SetPlatformTextureSettings(androidSetting);
                }

                if (platform == TextureFolderData.OptimizationPlatform.iOS ||
                    platform == TextureFolderData.OptimizationPlatform.Android_iOS)
                {
                    var ios = new TextureImporterPlatformSettings();
                    importer.GetDefaultPlatformTextureSettings().CopyTo(ios);
                    ios.format = format;
                    ios.overridden = true;
                    ios.name = "ios";
                    importer.SetPlatformTextureSettings(ios);
                }

                importer.SaveAndReimport();
            }
        }
    }

    [System.Serializable]
    public class TextureFolderData
    {
        public enum OptimizationPlatform
        {
            None,
            Android,
            iOS,
            Android_iOS
        }

        public string Path;

        //[LabelText("优化平台")]
        public OptimizationPlatform Platform = OptimizationPlatform.Android_iOS;

        //[LabelText("压缩格式")] 
        public TextureImporterFormat Compression = TextureImporterFormat.ASTC_RGBA_6x6;
    }
}