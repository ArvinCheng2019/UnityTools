using System;
using System.Collections;
using System.Collections.Generic;
using Arvin;
using UnityEditor;
using UnityEngine;

public class ParticleMenu : Editor
{
    [MenuItem("Assets/Kunpo/优化工具/特效/添加列表")]
    public static void AddToList()
    {
        var item = ScriptableHelper.GetGameObjectOptimizastion();
        var objs = Selection.objects;
        foreach (var obj in objs)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            item.AddToParticleList(path);
        }

        EditorUtility.SetDirty(item);
    }

    [MenuItem("Assets/Kunpo/优化工具/特效/从列表移除")]
    public static void RemoveToList()
    {
        var item = ScriptableHelper.GetGameObjectOptimizastion();
        var objs = Selection.objects;
        foreach (var obj in objs)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            item.RemoveParticleList(path);
        }

        EditorUtility.SetDirty(item);
    }

    [MenuItem("Kunpo/优化工具/优化特效", false, 9)]
    public static void RunMaxCount()
    {
        try
        {
            int index = 0;
            int max = 0;
            EditorUtility.DisplayProgressBar("修改特效文件", "正在处理特效资源", (float)index / (float)max);
            var item = ScriptableHelper.GetGameObjectOptimizastion();
            var setting = ScriptableHelper.GetOptimizastionSetting();
            string[] paths = item.GetParticlePaths();
            string[] guids = AssetDatabase.FindAssets("t:Prefab", paths);
            max = paths.Length;
            foreach (var guid in guids)
            {
                index++;
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go == null)
                    continue;
                ParticleSystem[] sys = go.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem ps in sys)
                {
                    Renderer render = ps.GetComponent<Renderer>();
                    ParticleSystem.MainModule main = ps.main;
                    var emit = ps.emission;
                    if (render != null && emit.enabled)
                    {
                        if (main.maxParticles > setting.Effect_MaxCount)
                        {
                            if (!render.enabled)
                            {
                                main.maxParticles = setting.Effect_RenderDisableMaxCount;
                                EditorUtility.DisplayProgressBar("修改特效文件",
                                    $" 粒子的 render.enable = false, 将粒子数改成{setting.Effect_RenderDisableMaxCount}",
                                    (float)index / (float)max);
                            }
                            else
                            {
                                main.maxParticles = setting.Effect_MaxCount;
                                EditorUtility.DisplayProgressBar("修改特效文件",
                                    $"特效数大于{setting.Effect_MaxCount}，修改成{setting.Effect_MaxCount}",
                                    (float)index / (float)max);

                                if (ps.main.maxParticles == 0)
                                    render.enabled = false;
                            }
                        }
                    }
                }

                PrefabUtility.SavePrefabAsset(go);
            }

            EditorUtility.ClearProgressBar();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            EditorUtility.ClearProgressBar();
        }

        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
}