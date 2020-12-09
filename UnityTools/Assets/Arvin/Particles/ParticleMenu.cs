using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParticleMenu : Editor
{
    [MenuItem("Arvin/特效/最大粒子数")]
    public static void RunMaxCount()
    {
        int index = 0;
        int max = 0;
        EditorUtility.DisplayProgressBar("修改特效文件", "正在处理特效资源", (float) index / (float) max);
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
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
                    }
                    else
                    {
                        if (ps.main.maxParticles > 50)
                            main.maxParticles = 50;

                        if (ps.main.maxParticles == 0)
                            render.enabled = false;
                    }
                }
            }
            EditorUtility.DisplayProgressBar("修改特效文件", "正在处理特效资源", (float) index / (float) max);
        }
        EditorUtility.ClearProgressBar();
    }
}