﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arvin
{
    public class TextureOptimization : ScriptableObject
    {
        public List<TextureFolderData> TextureOptimizations = new List<TextureFolderData>();
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
                    Compression = setting.DefaultFormat,
                    Path = path,
                    Platform = TextureFolderData.OptimizationPlatform.Android_iOS
                });
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
                importer.mipmapEnabled = !setting.CloseMipMap;

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
    }
}