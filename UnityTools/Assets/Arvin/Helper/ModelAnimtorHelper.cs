﻿using System.Collections.Generic;
using System.IO;
using Arvin;
using UnityEditor;
using UnityEngine;

public class ModelAnimtorHelper
{
    public static void ClearMesh(string guid)
    {
        var setting = ScriptableHelper.GetOptimizastionSetting();
        string path = AssetDatabase.GUIDToAssetPath(guid);
        ModelImporter import = (ModelImporter) AssetImporter.GetAtPath(path);
        import.importCameras = setting.Model_ImportCameras;
        import.importLights = setting.Model_ImportLights;
        import.optimizeMesh = setting.Model_OptimizeMesh;
        import.importMaterials = setting.Model_ImportMaterials;
        import.meshCompression = setting.Model_MeshCompression;
        import.animationCompression = setting.Model_AnimCompression; // ModelImporterAnimationCompression.Optimal;
        import.importTangents = setting.Model_ImportTangents; //ModelImporterTangents.None;
        import.importNormals = setting.Model_ImportNormals; //ModelImporterNormals.Import;
        import.isReadable = setting.Model_Readable;
        import.generateSecondaryUV = setting.Model_GenerateSecondaryUV;
        import.SaveAndReimport();
    }

    public static void CheckAnimation()
    {
        try
        {
            var files = new List<string>();
            files.AddRange(Directory.GetFiles("Assets/", "*.fbx", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles("Assets/", "*.anim", SearchOption.AllDirectories));

            int count = 0;
            int sumCount = files.Count;
            foreach (var file in files)
            {
                count++;

                var ext = Path.GetExtension(file).ToLower();
                if (ext == ".fbx")
                {
                    var obj = AssetDatabase.LoadAssetAtPath<GameObject>(file);
                    RemoveAnimationCurve(obj);
                }
                else if (ext == ".anim")
                {
                    var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(file);
                    CompressAnimationClip(clip);
                }

                if (ext == ".fbx" || ext == ".anim" || count % 20 == 0)
                    EditorUtility.DisplayProgressBar($"检测中({count}/{sumCount})", file, count * 1f / sumCount);
            }

            AssetDatabase.SaveAssets();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    //移除scale
    public static void RemoveAnimationCurve(GameObject _obj)
    {
        var setting = ScriptableHelper.GetOptimizastionSetting();
        List<AnimationClip> tAnimationClipList = new List<AnimationClip>(AnimationUtility.GetAnimationClips(_obj));
        if (tAnimationClipList.Count == 0)
        {
            AnimationClip[] tObjectList =
                UnityEngine.Object.FindObjectsOfType(typeof(AnimationClip)) as AnimationClip[];
            tAnimationClipList.AddRange(tObjectList);
        }

        if (setting.Anim_RemoveAnimScale)
        {
            foreach (AnimationClip animClip in tAnimationClipList)
            {
                RemoveScale(animClip);
            }
        }

        foreach (AnimationClip animClip in tAnimationClipList)
        {
            CompressAnimationClip(animClip);
        }
    }

    static void RemoveScale(AnimationClip theAnimation)
    {
        try
        {
            //去除scale曲线
            foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(theAnimation))
            {
                string name = theCurveBinding.propertyName.ToLower();
                if (name.Contains("scale"))
                {
                    AnimationUtility.SetEditorCurve(theAnimation, theCurveBinding, null);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("CompressAnimationClip Failed !!! error: {0}", e));
        }
    }

    //压缩精度移除scale
    static void CompressAnimationClip(AnimationClip theAnimation)
    {
        try
        {
            //浮点数精度压缩到f3
            AnimationClipCurveData[] curves = null;
#pragma warning disable CS0618 // 类型或成员已过时
            curves = AnimationUtility.GetAllCurves(theAnimation);
#pragma warning restore CS0618 // 类型或成员已过时
            Keyframe key;
            Keyframe[] keyFrames;
            for (int ii = 0; ii < curves.Length; ++ii)
            {
                AnimationClipCurveData curveDate = curves[ii];
                if (curveDate.curve == null || curveDate.curve.keys == null)
                {
                    //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
                    continue;
                }

                keyFrames = curveDate.curve.keys;
                for (int i = 0; i < keyFrames.Length; i++)
                {
                    key = keyFrames[i];
                    key.value = float.Parse(key.value.ToString("f3"));
                    key.inTangent = float.Parse(key.inTangent.ToString("f3"));
                    key.outTangent = float.Parse(key.outTangent.ToString("f3"));
                    keyFrames[i] = key;
                }

                curveDate.curve.keys = keyFrames;
                theAnimation.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(string.Format("CompressAnimationClip Failed !!! error: {0}", e));
        }
    }
}