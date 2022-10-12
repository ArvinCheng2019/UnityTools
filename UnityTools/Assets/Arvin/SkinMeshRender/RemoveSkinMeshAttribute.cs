using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class RemoveSkinMeshAttribute
{
    [MenuItem("Assets/Arvin/常用工具/删除SkinnedMesh的不常用属性")]
    static void Run()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        EditorUtility.DisplayProgressBar("正在处理模型...", $"请稍等({0}/{objects.Length}) ", 0);
        foreach (var obj in objects)
        {
            GameObject go = (GameObject)obj;
            ClearSkinMesh(go);
            ClearMeshRender(go);
        }


        EditorUtility.DisplayProgressBar("正在处理模型...", $"请稍等({objects.Length}/{objects.Length}) ", 1);
        EditorUtility.ClearProgressBar();
    }

    static void ClearSkinMesh(GameObject go)
    {
        SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.skinnedMotionVectors = false;
            renderer.allowOcclusionWhenDynamic = false;
            renderer.receiveShadows = false;
            renderer.staticShadowCaster = false;
            renderer.lightProbeUsage = LightProbeUsage.Off;
            renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
        }
    }

    static void ClearMeshRender(GameObject go)
    {
        MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.allowOcclusionWhenDynamic = false;
            renderer.receiveShadows = false;
            renderer.staticShadowCaster = false;
            renderer.lightProbeUsage = LightProbeUsage.Off;
            renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
        }
    }
}