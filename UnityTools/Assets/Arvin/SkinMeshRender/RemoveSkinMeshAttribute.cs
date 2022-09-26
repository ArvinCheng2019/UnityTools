using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RemoveSkinMeshAttribute : Editor
{
    [MenuItem("Assets/Arvin/常用工具/删除SkinnedMesh的不常用属性")]
    static void Run()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

        foreach (var obj in objects)
        {
            GameObject go = (GameObject)obj;
            SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.skinnedMotionVectors = false;
                renderer.allowOcclusionWhenDynamic = false;
                renderer.receiveShadows = false;
            }
        }
    }
}