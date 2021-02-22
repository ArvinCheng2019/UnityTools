using System.Collections;
using System.Collections.Generic;
using System.IO;
using Arvin;
using UnityEditor;
using UnityEngine;

public class SplitAnimator : Editor
{
    [MenuItem("Assets/Kunpo/常用工具/从模型里分离AnimClip")]
    public static void Run()
    {
        var setting = ScriptableHelper.GetOptimizastionSetting();
        string tarPath = setting.Anim_ExportClipPath;
        if (string.IsNullOrEmpty(tarPath))
        {
            EditorUtility.DisplayDialog("请设置AnimationClip的导出路径", " AnimClip的导出路径为空，请设置", "OK");
            return;
        }

        string path = Application.dataPath + tarPath.Replace("Assets", "");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        UnityEngine.Object[]
            objects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Unfiltered); //获取所有选中的物体
        foreach (UnityEngine.Object o in objects) //遍历选择的物体
        {
            string fbxPath = AssetDatabase.GetAssetPath(o);
            string[] fbxName = o.name.Split('@');
            string animName = string.Empty;
            if (fbxName.Length == 2)
            {
                animName = fbxName[1];
            }
            else if (fbxName.Length < 2)
            {
                animName = fbxName[0];
            }

            AnimationClip fbxClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(fbxPath); //获取FBX上的animationClip
            if (fbxClip == null)
            {
                Debug.Log("当前选择的文件不是带有AnimationClip的FBX文件");
            }
            else
            {
                var assetPath = tarPath + animName + ".anim";
                var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
                var existFlag = clip != null;
                Debug.LogError(existFlag);
                if (!existFlag)
                {
                    clip = new AnimationClip(); //new一个AnimationClip存放生成的AnimationClip
                }
                EditorUtility.CopySerialized(fbxClip, clip); //复制
                if (!existFlag)
                    AssetDatabase.CreateAsset(clip, assetPath); //生成文件
            }
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}