using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimatorMenu : Editor
{
    [MenuItem("Arvin/清理/动画精度")]
    public static void RunAnim()
    {
        ModelAnimtorHelper.CheckAnimation();
    }

    [MenuItem("Arvin/清理/模型属性")]
    public static void RunModel()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model");
        foreach (string guid in guids)
        {
            ModelAnimtorHelper.ClearMesh(guid);
        }
    }
}
