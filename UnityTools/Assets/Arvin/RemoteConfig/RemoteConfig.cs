using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class RemoteConfig : OdinEditorWindow
{
    [MenuItem("Arvin/RemoteConfig")]
    private static void OpenWindow()
    {
        GetWindow<RemoteConfig>().Show();
    }

    [LabelText("服务器地址")] public string CDN;

    [HorizontalGroup("Split", 0.5f)]
    [Button("拉取"), GUIColor(0.12f, 1, 0)]
    public void PullData()
    {
    }

    [VerticalGroup("Split/right")]
    [Button("推送"), GUIColor(0.12f, 1, 0)]
    public void PostData()
    {
    }

    [Button("加一个新的元素"), GUIColor(1, 0, 0)]
    void AddTable()
    {
        Table.Add(new RemoteData());
    }

    [PropertyOrder(1)] [LabelText("元素列表"),] [TableList]
    public List<RemoteData> Table = new List<RemoteData>();
}

[Serializable]
public class RemoteData
{
    public string name;
    public string key;
    public Type type;
}