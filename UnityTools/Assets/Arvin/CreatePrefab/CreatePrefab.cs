using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CreatePrefab : EditorWindow
{

    [MenuItem("Arvin/工具/CreatePrefab")]
    private static void ShowWindow()
    {
        var window = GetWindow<CreatePrefab>();
        window.titleContent = new GUIContent("创建Prefab");
        window.Show();
    }

    private void OnGUI()
    {

    }
}