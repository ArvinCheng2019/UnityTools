using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimatorMenu : Editor
{
    [MenuItem("Arvin/优化工具/清理/动画精度（全部动画）", false, 9)]
    public static void RunAnim()
    {
        ModelAnimtorHelper.CheckAnimation();
    }

    [MenuItem("Arvin/优化工具/清理/动画精度（选中）", false, 9)]
    public static void RunSelected()
    {
        try
        {
            var files = Selection.objects;
            int count = 0;
            int sumCount = files.Length;

            foreach (var file in files)
            {
                count++;
                string filePath = AssetDatabase.GetAssetPath(file);
                var ext = Path.GetExtension(filePath).ToLower();

                if (ext == ".fbx")
                {
                    var obj = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                    ModelAnimtorHelper.RemoveAnimationCurve(obj);
                }
                else if (ext == ".anim")
                {
                    var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(filePath);
                    ModelAnimtorHelper.CompressAnimationClip(clip);
                }

                if (ext == ".fbx" || ext == ".anim" || count % 20 == 0)
                    EditorUtility.DisplayProgressBar($"检测中({count}/{sumCount})", filePath, count * 1f / sumCount);
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

    [MenuItem("Arvin/优化工具/清理/模型属性", false, 10)]
    public static void RunModel()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model");
        foreach (string guid in guids)
        {
            ModelAnimtorHelper.ClearMesh(guid);
        }
    }
}