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
    
    [MenuItem("Kunpo/优化工具/清理/特效")]
    public static void RunMaxCount()
    {
        int index = 0;
        int max = 0;
        EditorUtility.DisplayProgressBar("修改特效文件", "正在处理特效资源", (float) index / (float) max);
        var item = ScriptableHelper.GetGameObjectOptimizastion();
        string[] paths = item.GetParticlePaths();
        string[] guids = AssetDatabase.FindAssets("t:Prefab",paths);
        max = guids.Length;
        foreach (var guid in guids)
        {
            index++;
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            ParticleSystem[] sys = go.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in sys)
            {
                Renderer render = ps.GetComponent<Renderer>();
                ParticleSystem.MainModule main = ps.main;
                if (render != null)
                {
                    if (!render.enabled)
                    {
                        main.maxParticles = 0;
                        EditorUtility.DisplayProgressBar("修改特效文件", " 粒子的 render.enable = false, 将粒子数归零",
                            (float) index / (float) max);
                    }
                    else
                    {
                        if (ps.main.maxParticles > 50)
                        {
                            main.maxParticles = 50;
                            EditorUtility.DisplayProgressBar("修改特效文件", "特效数大于 50，修改成50", (float) index / (float) max);
                        }

                        if (ps.main.maxParticles == 0)
                            render.enabled = false;
                    }
                }
            }

            PrefabUtility.SavePrefabAsset(go);
        }

        EditorUtility.ClearProgressBar();
    }
}