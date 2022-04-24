using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class FindAlphaOneTexture : Editor
{
    private static List<string> NeedDelRes;
    
    [MenuItem("Assets/Arvin/常用工具/获取透明图片")]
    public static void GetAlphaOneTextures()
    {
        Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
        NeedDelRes = new List<string>();
        int max = objects.Length;
        int cur = 0;
        foreach (var text in objects)
        {
            float val = cur / max;
            EditorUtility.DisplayProgressBar("设置图片中...", $"请稍等({cur}/{max}) ", val);
            float prop = 0;
            Texture2D texture = text as Texture2D;
            string path = AssetDatabase.GetAssetPath(texture);
            ReImportAndSetRW(path, true);
            float allNum = texture.height * texture.width;
            Color[] colors = texture.GetPixels();
            int aa = 0;
            for (int i = 0; i < allNum; i++)
            {
                if (colors[i] != null && colors[i].a == 0)
                {
                    aa++;
                }
            }

            prop = aa / allNum;
            if (prop > 0.99f)
            {
                NeedDelRes.Add(path);
            }

            cur++;
        }

        foreach (var text in objects)
        {
            string path = AssetDatabase.GetAssetPath(text);
            ReImportAndSetRW(path, false);
        }

        foreach (var path in NeedDelRes)
        {
            bool result = AssetDatabase.DeleteAsset(path);
            Debug.LogError($" 检测到 {path}   这张贴图 透明像素 占比到 99% 以上，清理掉");
        }

        EditorUtility.ClearProgressBar();
    }

    //  重新倒入，来更新 read / write
    public static void ReImportAndSetRW(string path, bool isReadable)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        importer.isReadable = isReadable;
        importer.SaveAndReimport();
    }
}