using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class createRemote : MonoBehaviour
{
    // Start is called before the first frame update
    private List<FrameRanage> framsRange;
    public Sprite[] sprites;
    private Action GetFrames;
    void Awake()
    {
        framsRange = new List<FrameRanage>();
    }


    public void GetRemoteAd( Action callback )
    {
        if ( callback != null)
        {
            this.GetFrames = callback;
        }
        
        StartCoroutine(getRemoteAtlas("http://gamescdn.lanfeitech.com/kunpopark/Wx/anim/"));
    }
    
    IEnumerator getRemoteAtlas(string url)
    {
        framsRange.Clear();
        string json = url + "unitytext.txt";
        UnityWebRequest request = UnityWebRequest.Get(json);
        yield return request.SendWebRequest();
        if (request.error != String.Empty)
        {
            Debug.Log(request.error);
            yield return null;
        }

        readJson(request.downloadHandler.text);
        string texture = url + "unitytext.png";
        request = UnityWebRequest.Get(texture);
        DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
        request.downloadHandler = downloadHandlerTexture;
        yield return request.SendWebRequest();
        if (request.error != String.Empty)
        {
            Debug.Log(request.error);
            yield return null;
        }

        Texture2D t = downloadHandlerTexture.texture;
        splitAtlas(t);
    }

    private void readJson(string msg)
    {
        JSONNode node = SimpleJSON.JSON.Parse(msg);
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

    }

    private void splitAtlas(  Texture2D atlase)
    {
        int index = 0;
        sprites = new Sprite[framsRange.Count];
        int height = 912;
        foreach (var frame in framsRange)
        {
            Texture2D texture2D = new Texture2D(frame.w,frame.h);
            for (int j = height - frame.y; j >= height - frame.y - frame.h; j--)
            {
                for (int i = frame.x; i < frame.x + frame.w; i++)
                {
                    int x = i;
                    int y = height - j;
                    texture2D.SetPixel(x, y, atlase.GetPixel(i, j));
                }
            }
            
            texture2D.Apply();
            Sprite sprite = Sprite.Create(texture2D,new Rect(0,0,frame.w,frame.h), Vector2.zero );
            
            sprites[index] = sprite;
            index++;
        }

        if (GetFrames != null)
        {
            GetFrames();
        }
    }

    private FrameRanage getFrameRange( string frame )
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

    private int tryGetInt( JSONNode node, string key)
    {
        int value = 0;
        int.TryParse(node[key].ToString(),out value);
        return value;
    }
}

class  FrameRanage
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