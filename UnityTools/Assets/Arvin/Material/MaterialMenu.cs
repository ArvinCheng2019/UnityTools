using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialMenu : Editor
{
    private static Dictionary<string, List<string>> ClearMatProperty = null;

    [MenuItem("Kunpo/优化工具/清理/材质球属性")]
    public static void Run()
    {
        ClearMaterialProperty();
    }

    public static void ClearMaterialProperty()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");
        int i = 0;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EditorUtility.DisplayProgressBar("处理中", path, (float) i / guids.Length);
            if (ClearMatProperty == null)
                ClearMatProperty = new Dictionary<string, List<string>>();
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat)
            {
                SerializedObject psSource = new SerializedObject(mat);
                SerializedProperty emissionProperty = psSource.FindProperty("m_SavedProperties");
                SerializedProperty texEnvs = emissionProperty.FindPropertyRelative("m_TexEnvs");
                List<string> result = CleanMaterialSerializedProperty(texEnvs, mat);
                psSource.ApplyModifiedProperties();
                if (!ClearMatProperty.ContainsKey(guid))
                {
                    ClearMatProperty.Add(guid, result);
                }

                EditorUtility.SetDirty(mat);
            }

            i++;
        }
    }

    private static List<string> CleanMaterialSerializedProperty(SerializedProperty property, Material mat)
    {
        List<string> results = new List<string>();
        for (int j = property.arraySize - 1; j >= 0; j--)
        {
            string propertyName = property.GetArrayElementAtIndex(j).FindPropertyRelative("first").stringValue;

            if (!mat.HasProperty(propertyName))
            {
                if (propertyName == "_MainTex") //_MainTex是自带属性，最好不要删除，否则UITexture等控件在获取mat.maintexture的时候会报错
                {
                    if (property.GetArrayElementAtIndex(j).FindPropertyRelative("second")
                        .FindPropertyRelative("m_Texture").objectReferenceValue != null)
                    {
                        property.GetArrayElementAtIndex(j).FindPropertyRelative("second")
                            .FindPropertyRelative("m_Texture").objectReferenceValue = null;
                        string msg = "Set _MainTex is null " + propertyName;
                        results.Add(msg);
                    }
                }
                else
                {
                    property.DeleteArrayElementAtIndex(j);
                    string msg = "Delete legacy property in serialized object : " + propertyName;
                    results.Add(msg);
                }
            }
        }

        return results;
    }
}