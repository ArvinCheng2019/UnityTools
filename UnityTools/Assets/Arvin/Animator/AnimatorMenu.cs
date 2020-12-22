using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimatorMenu : Editor
{
    [MenuItem("Kunpo/优化工具/清理/动画精度",false,9)]
    public static void RunAnim()
    {
        ModelAnimtorHelper.CheckAnimation();
    }

    [MenuItem("Kunpo/优化工具/清理/模型属性",false,10)]
    public static void RunModel()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model");
        foreach (string guid in guids)
        {
            ModelAnimtorHelper.ClearMesh(guid);
        }
    }
}
