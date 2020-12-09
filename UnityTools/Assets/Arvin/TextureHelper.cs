// using System.Collections.Generic;
// using System.IO;
// using System.Reflection;
// using UnityEditor;
// using UnityEngine;
//
// public class TextureHelper
// {
//     public static void CompressAndroid(string guid)
//     {
//         string path = AssetDatabase.GUIDToAssetPath(guid);
//         string suffix = path.ToLower();
//         if (suffix.EndsWith(".png") || suffix.EndsWith(".tga") || suffix.EndsWith(".jpg") ||
//             suffix.EndsWith(".psd"))
//         {
//             TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
//             importer.mipmapEnabled = false;
//             importer.filterMode = FilterMode.Bilinear;
//
//             var androidSetting = new TextureImporterPlatformSettings();
//             importer.GetDefaultPlatformTextureSettings().CopyTo(androidSetting);
//             androidSetting.overridden = true;
//
//             Texture tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
//             if (tex)
//             {
//                 androidSetting.format = TextureImporterFormat.ASTC_RGBA_6x6;
//                 // //判断纹理尺寸是否符合四的倍数
//                 // var size = GetOriginalSize(importer);
//                 //
//                 // int width = size[0];
//                 // int height = size[1];
//                 // if ((width % 4 == 0) && (height % 4 == 0))
//                 // {
//                 //     androidSetting.format = TextureImporterFormat.ETC2_RGBA8Crunched;
//                 // }
//                 // else
//                 // {
//                 //     androidSetting.format = TextureImporterFormat.ASTC_RGBA_6x6;
//                 // }
//             }
//
//             androidSetting.name = "android";
//             importer.SetPlatformTextureSettings(androidSetting);
//             importer.SaveAndReimport();
//         }
//     }
//
//     static bool IsNOTexture(int size)
//     {
//         int[] numbers = new[] { 128, 256, 512, 1024, 2048 };
//         return numbers.Equals(size);
//     }
//
//     static int[] GetOriginalSize(TextureImporter importer)
//     {
//         object[] args = { 0, 0 };
//         MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
//         mi.Invoke(importer, args);
//         return new int[] {
//             (int)args[0],
//             (int)args[1]
//         };
//     }
//     
//     public static void CompressIOS(string guid)
//     {
//         string path = AssetDatabase.GUIDToAssetPath(guid);
//         string suffix = path.ToLower();
//         if (suffix.EndsWith(".png") || suffix.EndsWith(".tga") || suffix.EndsWith(".jpg") ||
//             suffix.EndsWith(".psd"))
//         {
//             TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
//             var iosSetting = new TextureImporterPlatformSettings();
//             importer.mipmapEnabled = false;
//             importer.filterMode = FilterMode.Bilinear;
//
//             importer.GetDefaultPlatformTextureSettings().CopyTo(iosSetting);
//             iosSetting.overridden = true;
//             Texture tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
//             if (tex)
//             {
//                 iosSetting.format = TextureImporterFormat.ASTC_RGBA_6x6;
//                 // //判断纹理尺寸是否符合四的倍数
//                 // var size = GetOriginalSize(importer);
//                 //
//                 // int width = size[0];
//                 // int height = size[1];
//                 // if ((width % 4 == 0) && (height % 4 == 0))
//                 // {
//                 //     iosSetting.format = TextureImporterFormat.ETC2_RGBA8Crunched;
//                 // }
//                 // else
//                 // {
//                 //     iosSetting.format = TextureImporterFormat.ASTC_RGBA_6x6;
//                 // }
//             }
//
//             importer.mipmapEnabled = false;
//             iosSetting.name = "iPhone";
//
//             importer.SetPlatformTextureSettings(iosSetting);
//             importer.SaveAndReimport();
//         }
//     }
//
//     public static void CompressAndroidAndIOS(string guid)
//     {
//         CompressAndroid(guid);
//         CompressIOS(guid);
//     }
//
//     public static void JenkinsComporssTexture()
//     {
//         string[] guids = AssetDatabase.FindAssets("t:Texture");
//         foreach (string guid in guids)
//         {
//             CompressAndroidAndIOS(guid);
//         }
//     }
//
//     public static void JenkinsChangeMaxSize()
//     {
//         var rule = ScriptObjectHelper.GetChangedMaxRules();
//         SetTextureMaxSize(rule.Max256, 256);
//         SetTextureMaxSize(rule.Max512, 512);
//         SetTextureMaxSize(rule.Max1024, 1024);
//         SetTextureMaxSize(rule.Max2048, 2048);
//     }
//
//     public static void SetTextureFolder()
//     {
//         var rule = ScriptObjectHelper.GetSetTextureRules();
//
//         foreach (var settings in rule.foldRules)
//         {
//             var tip = $"检查中({(rule.foldRules.IndexOf(settings) + 1)}/{rule.foldRules.Count})";
//             EditorUtility.DisplayProgressBar(tip, "", 0);
//             int count = 0;
//             var files = Directory.GetFiles(settings.path, "*", SearchOption.AllDirectories);
//             foreach (var file in files)
//             {
//                 count++;
//                 EditorUtility.DisplayProgressBar(tip + ":" + settings.path, file, count * 1f / files.Length);
//
//                 var ext = Path.GetExtension(file).ToLower();
//                 if (ext == ".png" || ext == ".jpg" || ext == ".psd")
//                 {
//                     var importer = (TextureImporter)AssetImporter.GetAtPath(file);
//                     bool change = false;
//
//                     if (settings.setTextureType && importer.textureType != settings.textureType)
//                     {
//                         change = true;
//                         importer.textureType = settings.textureType;
//                     }
//
//                     if (settings.setMaxSize && importer.maxTextureSize != settings.maxSize)
//                     {
//                         change = true;
//                         importer.maxTextureSize = settings.maxSize;
//                     }
//
//                     if (settings.setSpritePackingTag && importer.spritePackingTag != settings.spritePackingTag)
//                     {
//                         change = true;
//                         importer.spritePackingTag = settings.spritePackingTag;
//                     }
//
//                     if (settings.overrideForAndroid)
//                     {
//                         var c = importer.GetPlatformTextureSettings("Android", out int maxTextureSize, out TextureImporterFormat format, out int compressionQuality);
//
//                         if(!c || maxTextureSize != settings.androidMaxSize || format != settings.fromat || compressionQuality != settings.compressorQuality)
//                         {
//                             change = true;
//                             var importSettings = new TextureImporterPlatformSettings
//                             {
//                                 name = "Android",
//                                 overridden = true,
//                                 maxTextureSize = settings.androidMaxSize,
//                                 format = settings.fromat,
//                                 compressionQuality = settings.compressorQuality
//                             };
//                             importer.SetPlatformTextureSettings(importSettings);
//                         }
//
//                     }
//
//                     if (change)
//                     {
//                         Debug.Log("Change:" + file);
//                         importer.SaveAndReimport();
//                     }
//                 }
//             }
//         }
//         EditorUtility.ClearProgressBar();
//     }
//
//     private static void SetTextureMaxSize(List<string> paths, int maxSize)
//     {
//         if (paths == null || paths.Count == 0)
//         {
//             return;
//         }
//
//         string[] guids = AssetDatabase.FindAssets("t:Texture", paths.ToArray());
//         foreach (string guid in guids)
//         {
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//             string suffix = path.ToLower();
//             if (suffix.EndsWith(".png") || suffix.EndsWith(".tga") || suffix.EndsWith(".jpg") ||
//                 suffix.EndsWith(".psd"))
//             {
//                 Debug.Log("修改图片最大尺寸：" + path);
//                 TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
//                 var androidSetting = new TextureImporterPlatformSettings();
//                 androidSetting.overridden = true;
//                 androidSetting.name = "android";
//                 androidSetting.maxTextureSize = maxSize;
//                 importer.SetPlatformTextureSettings(androidSetting);
//
//                 var ios = new TextureImporterPlatformSettings();
//                 ios.overridden = true;
//                 ios.name = "iPhone";
//                 ios.maxTextureSize = maxSize;
//                 importer.SetPlatformTextureSettings(ios);
//                 importer.SaveAndReimport();
//             }
//         }
//     }
//
//     public static void CheckTextureSize(List<string> paths)
//     {
//         //写入csv日志
//         StreamWriter sw = new StreamWriter(Application.dataPath + "/../指定文件夹图片的检查结果.csv", false,
//             System.Text.Encoding.UTF8);
//         sw.WriteLine("指定文件夹图片的检查结果");
//         int max = 0;
//         string[] guids = AssetDatabase.FindAssets("t:Texture", paths.ToArray());
//         max = guids.Length;
//         int index = 0;
//         foreach (string guid in guids)
//         {
//             index++;
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//             string msg = string.Format("正在检查第{0} 张图片", index);
//             EditorUtility.DisplayProgressBar("正在检查图片", msg, (float)index / (float)max);
//
//             if (path.StartsWith("Assets/"))
//             {
//                 if (path.EndsWith(".png") || path.EndsWith(".tga") || path.EndsWith(".jpg") ||
//                     path.EndsWith(".psd"))
//                 {
//                     Texture tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture)) as Texture;
//                     if (tex)
//                     {
//                         //判断纹理尺寸是否符合四的倍数
//                         if (((tex.width % 4) != 0) || ((tex.height % 4) != 0))
//                         {
//                             sw.WriteLine(
//                                 string.Format("纹理尺寸不符合四的倍数  W/H size,{0},{1},{2}", path, tex.width, tex.height));
//                         }
//                     }
//                 }
//             }
//         }
//
//         EditorUtility.ClearProgressBar();
//         sw.Flush();
//         sw.Close();
//     }
// }