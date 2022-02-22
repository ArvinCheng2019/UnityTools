using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AlignmentTools
{
    [MenuItem("GameObject/对齐工具/上对齐")]
    // Start is called before the first frame update
    static void Run()
    {
        Debug.Log("Run" + Time.time);
        var files = Selection.objects;
        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            Debug.Log(file.name);
        }
    }
}