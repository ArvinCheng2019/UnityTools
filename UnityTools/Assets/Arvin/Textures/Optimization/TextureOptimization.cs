using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arvin
{
    public class TextureOptimization : ScriptableObject
    {
        // 因为 字典 不能被序列化到 面板上，所以拆成2个list
        public List<TextureFolderData> TextureOptimizations = new List<TextureFolderData>();
        public List<TextureFolderData> UIOptimizations = new List<TextureFolderData>();
        private SelfRuleRes selfRuleRes;
        private OptimizastionSetting setting;

        public void OnEnable()
        {
            selfRuleRes = ScriptableHelper.GetSelfRuleRes();
            setting = ScriptableHelper.GetOptimizastionSetting();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// 添加到统一处理列表里，这里传的是文件夹，不是具体文件
        /// </summary>
        /// <param name="path"></param>
        public void AddTexturePath(string path)
        {
            var pathed = TextureOptimizations.Find(item => { return item.Path.Equals(path); });
            if (pathed == null)
            {
                TextureOptimizations.Add(new TextureFolderData()
                {
                    Compression = setting.Texture_DefaultFormat,
                    Path = path,
                    Platform = TextureFolderData.OptimizationPlatform.Android_iOS
                });
            }
        }

        public void UpdateTextureSetting(TextureImporterFormat format,
            TextureFolderData.OptimizationPlatform platform = TextureFolderData.OptimizationPlatform.Android_iOS)
        {
            foreach (var data in TextureOptimizations)
            {
                data.Platform = platform;
                data.Compression = format;
            }
        }

        /// <summary>
        ///  从统一处理列表里删除，这里穿的是文件夹，不是具体文件
        /// </summary>
        /// <param name="path"></param>
        public void RemoveTextures(string path)
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
                if (data.SkinOptimization)
                {
                    continue;
                }

                string[] guids = AssetDatabase.FindAssets("t:Texture", new[] {data.Path});
                int index = 0, max = guids.Length;
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (selfRuleRes.IsResInSelfRule(path))
                    {
                        continue;
                    }

                    EditorUtility.DisplayProgressBar("正在压缩图片", $"正在压缩{data.Path} 下的图片,压缩格式为{data.Compression}",
                        (float) index / (float) max);
                    compresssTexture(guid, data.Platform, data.Compression);
                    index++;
                }
            }

            foreach (var data in UIOptimizations)
            {
                EditorUtility.DisplayProgressBar("正在压缩UI图片", $"正在压缩{data.Path} 下的图片", 0);
                if (data.SkinOptimization)
                {
                    continue;
                }

                string[] guids = AssetDatabase.FindAssets("t:Texture", new[] {data.Path});
                int index = 0, max = guids.Length;
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (selfRuleRes.IsResInSelfRule(path))
                    {
                        continue;
                    }

                    EditorUtility.DisplayProgressBar("正在压缩UI图片", $"正在压缩{data.Path} 下的图片,压缩格式为{data.Compression}",
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
                suffix.EndsWith(".psd") || suffix.EndsWith(".jpeg"))
            {
                TextureImporter importer = (TextureImporter) AssetImporter.GetAtPath(path);
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Bilinear;
                importer.mipmapEnabled = !setting.Texture_CloseMipMap;

                switch (platform)
                {
                    case TextureFolderData.OptimizationPlatform.Android:
                        runAndroid(importer, format);
                        break;
                    case TextureFolderData.OptimizationPlatform.iOS:
                        runiOS(importer, format);
                        break;
                    case TextureFolderData.OptimizationPlatform.Android_iOS:
                        runAndroid(importer, format);
                        runiOS(importer, format);
                        break;
                }

                importer.SaveAndReimport();
            }
        }

        void runAndroid(TextureImporter importer, TextureImporterFormat format)
        {
            var androidSetting = new TextureImporterPlatformSettings();
            importer.GetDefaultPlatformTextureSettings().CopyTo(androidSetting);
            androidSetting.format = format;
            androidSetting.overridden = true;
            androidSetting.name = "android";
            importer.SetPlatformTextureSettings(androidSetting);
        }

        void runiOS(TextureImporter importer, TextureImporterFormat format)
        {
            var ios = new TextureImporterPlatformSettings();
            importer.GetDefaultPlatformTextureSettings().CopyTo(ios);
            ios.format = format;
            ios.overridden = true;
            ios.name = "ios";
            importer.SetPlatformTextureSettings(ios);
        }

        /// <summary>
        /// 处理放在UI上的图片
        /// </summary>
        /// <param name="path"></param>
        public void AddUITexturePath(string path)
        {
            var pathed = UIOptimizations.Find(item => { return item.Path.Equals(path); });
            if (pathed == null)
            {
                UIOptimizations.Add(new TextureFolderData()
                {
                    Compression = setting.UI_DefaultFormat,
                    Path = path,
                    Platform = TextureFolderData.OptimizationPlatform.Android_iOS
                });
            }
        }

        /// <summary>
        /// 处理放在UI上的图片
        /// </summary>
        /// <param name="format"></param>
        /// <param name="platform"></param>
        public void UpdateUITextureSetting(TextureImporterFormat format,
            TextureFolderData.OptimizationPlatform platform = TextureFolderData.OptimizationPlatform.Android_iOS)
        {
            foreach (var data in UIOptimizations)
            {
                data.Platform = platform;
                data.Compression = format;
            }
        }

        /// <summary>
        ///  处理放在UI上的图片
        /// </summary>
        /// <param name="path"></param>
        public void RemoveUITextures(string path)
        {
            var pathed = UIOptimizations.Find(item => { return item.Path.Equals(path); });
            if (pathed == null)
            {
                return;
            }

            UIOptimizations.Remove(pathed);
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
        public OptimizationPlatform Platform = OptimizationPlatform.Android_iOS;
        public TextureImporterFormat Compression = TextureImporterFormat.ASTC_RGBA_6x6;
        public bool SkinOptimization = false;
    }
}