// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Text.RegularExpressions;
// using UnityEditor;
// using UnityEngine;
//
// public class CommandCheckerHelper
// {
//     private static Dictionary<string, List<string>> ClearMatProperty = null;
//     private static List<EffectCheckResult> CheckPrefabMissMat = null;
//     private static List<string> CheckFailureDir = null;
//
//     //android 和ios 通用的检测
//     public static void CommandChecker()
//     {
//         InitDictionary();
//         // 清理材质球上的无用属性
//         ClearMaterialProperty();
//         // 检查 Prefab  里有miss 的资源
//       //  CheckFailureDir = CheckFailureScript._CheckFailureDir();
//         // 检查 Prefab 是否有丢失的材质球
//         CheckPrefabMissMat = CheckPrefabMatMiss();
//         TextureHelper.JenkinsComporssTexture();
//         TextureHelper.JenkinsChangeMaxSize();
//         ModelAnimtorHelper.JenkinsModelRule();
//         ModelAnimtorHelper.JenkinsAnimRule();
//         // 保存检查结果
//         SaveAsCsv();
//         SaveCheckPrefabMissRes();
//         SaveCheckPrefabMissMat();
//         SaveTextureCsv();
//     }
//
//
//     #region 清理材质球上，无用的属性
//
//     public static void ClearMaterialProperty(bool isCommand = true)
//     {
//         string[] guids = AssetDatabase.FindAssets("t:Material");
//         int i = 0;
//         foreach (string guid in guids)
//         {
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//
//             if (!isCommand)
//             {
//                 EditorUtility.DisplayProgressBar("处理中", path, (float) i / guids.Length);
//             }
//
//             if (ClearMatProperty == null)
//                 ClearMatProperty = new Dictionary<string, List<string>>();
//             Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
//             if (mat)
//             {
//                 SerializedObject psSource = new SerializedObject(mat);
//                 SerializedProperty emissionProperty = psSource.FindProperty("m_SavedProperties");
//                 SerializedProperty texEnvs = emissionProperty.FindPropertyRelative("m_TexEnvs");
//                 List<string> result = CleanMaterialSerializedProperty(texEnvs, mat);
//                 psSource.ApplyModifiedProperties();
//                 if (!ClearMatProperty.ContainsKey(guid))
//                 {
//                     ClearMatProperty.Add(guid, result);
//                 }
//
//                 EditorUtility.SetDirty(mat);
//             }
//
//             i++;
//         }
//
//         AssetDatabase.SaveAssets();
//         if (!isCommand)
//             EditorUtility.ClearProgressBar();
//     }
//
//     public static void ReplaceShader(Shader from, Shader to)
//     {
//         string[] guids = AssetDatabase.FindAssets("t:Material");
//         int i = 0;
//         foreach (string guid in guids)
//         {
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//
//             EditorUtility.DisplayProgressBar("处理中", path, (float) i / guids.Length);
//
//             if (ClearMatProperty == null)
//                 ClearMatProperty = new Dictionary<string, List<string>>();
//             Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
//             if (mat)
//             {
//                 if (mat.shader == from)
//                 {
//                     mat.shader = to;
//                 }
//
//                 EditorUtility.SetDirty(mat);
//             }
//
//             i++;
//         }
//
//         AssetDatabase.SaveAssets();
//         EditorUtility.ClearProgressBar();
//     }
//
//     private static List<string> CleanMaterialSerializedProperty(SerializedProperty property, Material mat)
//     {
//         List<string> results = new List<string>();
//         for (int j = property.arraySize - 1; j >= 0; j--)
//         {
//             string propertyName = property.GetArrayElementAtIndex(j).FindPropertyRelative("first").stringValue;
//
//             if (!mat.HasProperty(propertyName))
//             {
//                 if (propertyName == "_MainTex") //_MainTex是自带属性，最好不要删除，否则UITexture等控件在获取mat.maintexture的时候会报错
//                 {
//                     if (property.GetArrayElementAtIndex(j).FindPropertyRelative("second")
//                         .FindPropertyRelative("m_Texture").objectReferenceValue != null)
//                     {
//                         property.GetArrayElementAtIndex(j).FindPropertyRelative("second")
//                             .FindPropertyRelative("m_Texture").objectReferenceValue = null;
//                         string msg = "Set _MainTex is null " + propertyName;
//                         results.Add(msg);
//                     }
//                 }
//                 else
//                 {
//                     property.DeleteArrayElementAtIndex(j);
//                     string msg = "Delete legacy property in serialized object : " + propertyName;
//                     results.Add(msg);
//                 }
//             }
//         }
//
//         return results;
//     }
//
//     #endregion
//
//     #region 检查Prefab 上是否有丢失材质球的情况
//
//     public static List<EffectCheckResult> CheckPrefabMatMiss(string[] paths = null)
//     {
//         string[] guids;
//         List<EffectCheckResult> results = new List<EffectCheckResult>();
//         if (paths == null)
//         {
//             guids = AssetDatabase.FindAssets("t:Prefab");
//         }
//         else
//         {
//             guids = AssetDatabase.FindAssets("t:Prefab", paths.ToArray());
//         }
//
//         foreach (string guid in guids)
//         {
//             string path = AssetDatabase.GUIDToAssetPath(guid);
//             GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
//             ParticleSystem[] sys = go.GetComponentsInChildren<ParticleSystem>();
//             foreach (var ps in sys)
//             {
//                 Renderer render = ps.GetComponent<Renderer>();
//                 if (render)
//                 {
//                     if (render.sharedMaterial == null)
//                     {
//                         EffectCheckResult ecr = new EffectCheckResult();
//                         ecr.Prefab = go;
//                         ecr.Path = path;
//                         ecr.Descript = string.Format("{0}缺少材质球，提示可能不全，请逐一检查", ps.gameObject.name);
//                         results.Add(ecr);
//                     }
//                 }
//             }
//         }
//
//         return results;
//     }
//
//     #endregion
//
//     #region 导出检查信息 保存为csv
//
//     private static void InitDictionary()
//     {
//         ClearMatProperty = new Dictionary<string, List<string>>();
//         CheckPrefabMissMat = new List<EffectCheckResult>();
//         CheckFailureDir = new List<string>();
//     }
//
//     private static void SaveCheckPrefabMissMat()
//     {
//         StreamWriter sw = new StreamWriter(Application.dataPath + "/../Jenkins自动检查结果_Prefab上有miss的材质球.csv", false,
//             System.Text.Encoding.UTF8);
//
//         sw.WriteLine(" Prefab 上有材质球丢失了 ");
//         sw.WriteLine(" ");
//         if (CheckPrefabMissMat.Count == 0)
//         {
//             sw.WriteLine(" 没有检查到 Prefab 有  miss的 材质球");
//         }
//
//         foreach (EffectCheckResult data in CheckPrefabMissMat)
//         {
//             string msg =
//                 string.Format("PrefabName：" + data.Prefab.name + "  路径 ：" + data.Path + " 描述：" + data.Descript);
//             sw.WriteLine(msg);
//         }
//
//         sw.Flush();
//         sw.Close();
//     }
//
//     private static void SaveCheckPrefabMissRes()
//     {
//         StreamWriter sw = new StreamWriter(Application.dataPath + "/../Jenkins自动检查结果_Prefab上有miss的资源.csv", false,
//             System.Text.Encoding.UTF8);
//
//         sw.WriteLine(" Prefab 里有引用 Miss 的资源 ");
//         sw.WriteLine("  ");
//
//         if (CheckFailureDir.Count == 0)
//         {
//             sw.WriteLine(" 没有检查到 Prefab上有miss的资源");
//         }
//
//         foreach (string msg in CheckFailureDir)
//         {
//             sw.WriteLine(msg);
//         }
//
//         sw.Flush();
//         sw.Close();
//     }
//
//
//     private static void SaveAsCsv()
//     {
//         StreamWriter sw = new StreamWriter(Application.dataPath + "/../Jenkins自动检查结果_清理材质球上无用属性.csv", false,
//             System.Text.Encoding.UTF8);
//
//         sw.WriteLine(" 清理了材质球上无用的属性 ");
//
//         foreach (List<string> res in ClearMatProperty.Values)
//         {
//             foreach (string msg in res)
//             {
//                 sw.WriteLine(msg);
//             }
//         }
//
//         sw.Flush();
//         sw.Close();
//     }
//
//     static void SaveTextureCsv()
//     {
//         //写入csv日志
//         StreamWriter sw = new StreamWriter(Application.dataPath + "/../Jenkins自动检查结果_异常图片.csv", false,
//             System.Text.Encoding.UTF8);
//         sw.WriteLine("图片 命名 和 尺寸异常");
//
//         string[] allAssets = AssetDatabase.GetAllAssetPaths();
//         foreach (string s in allAssets)
//         {
//             if (s.StartsWith("Assets/"))
//             {
//                 Texture tex = AssetDatabase.LoadAssetAtPath(s, typeof(Texture)) as Texture;
//
//                 if (tex)
//                 {
//                     //检测纹理资源命名是否合法
//                     if (!Regex.IsMatch(s, @"^[a-zA-Z][a-zA-Z0-9_/.]*$"))
//                     {
//                         sw.WriteLine(string.Format("不符合命名规范的图片:,{0}", s));
//                     }
//
//                     //判断纹理尺寸是否符合四的倍数
//                     if (((tex.width % 4) != 0) || ((tex.height % 4) != 0))
//                     {
//                         sw.WriteLine(string.Format("纹理尺寸不符合四的倍数  W/H size,{0},{1},{2}", s, tex.width, tex.height));
//                     }
//                 }
//             }
//         }
//
//         sw.Flush();
//         sw.Close();
//     }
//
//    
//     #endregion
// }