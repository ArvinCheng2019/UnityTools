using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Text;


public class SplitTexture : Editor
{
    private static List<FrameRanage> framsRange;

    [MenuItem("Tools/分割图片")]
    static void RunSplit()
    {
        framsRange = new List<FrameRanage>();
        string texturePath = "Assets/unitytext.png";
        string textPath = "Assets/unitytext.txt";
        TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(textPath);
        Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

        JSONNode node = SimpleJSON.JSON.Parse(ta.text);
        JSONNode framesNode = node["frames"];
        foreach (KeyValuePair<string, JSONNode> keyValuePair in framesNode.Linq)
        {
            FrameData fd = new FrameData();
            fd.name = keyValuePair.Key;
            fd.frame = keyValuePair.Value["frame"].ToString();
            fd.rotated = keyValuePair.Value["rotated"].ToString();
            fd.trimmed = keyValuePair.Value["trimmed"].ToString();
            fd.spriteSourceSize = keyValuePair.Value["spriteSourceSize"].ToString();
            fd.sourceSize = keyValuePair.Value["sourceSize"].ToString();
            var range = getFrameRange(fd.frame);
            range.name = keyValuePair.Key;
            framsRange.Add(range);
        }

        int index = 0;
        int height = 913;

        foreach (var frame in framsRange)
        {
            Texture2D newTexture2D = new Texture2D(frame.w, frame.h);
            //for (int j = height - frame.y; j >= height - frame.y - frame.h; j--)
            for (int j =frame.y;  j< frame.y + frame.h - 2; j++)
            {
                for (int i = frame.x; i < frame.x + frame.w; i++)
                {
                    int x = i;
                    int y = j;
                    newTexture2D.SetPixel(x, y, texture2D.GetPixel(i, j));
                }
            }

            newTexture2D.Apply();
            byte[] bytes = newTexture2D.EncodeToPNG();
            FileStream fs = new FileStream(Application.dataPath + "/" + frame.name, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        AssetDatabase.Refresh();
    }

    private static FrameRanage getFrameRange(string frame)
    {
        JSONNode node = JSONNode.Parse(frame);
        int x = tryGetInt(node, "x");
        int y = tryGetInt(node, "y");
        int w = tryGetInt(node, "w");
        int h = tryGetInt(node, "h");
        return new FrameRanage()
        {
            x = x,
            y = y,
            w = w,
            h = h
        };
    }

    private static int tryGetInt(JSONNode node, string key)
    {
        int value = 0;
        int.TryParse(node[key].ToString(), out value);
        return value;
    }
}

class FrameRanage
{
    public string name;
    public int x;
    public int y;
    public int w;
    public int h;
}

class FrameData
{
    public string name;
    public string frame;
    public string rotated;
    public string trimmed;
    public string spriteSourceSize;
    public string sourceSize;
    public FrameRanage range;
}