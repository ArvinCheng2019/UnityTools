using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace com.bt.editor
{
    [ExecuteInEditMode]
    public class SplitAtlas : MonoBehaviour
    {

        [MenuItem("Arvin/Tools/SplitAtlas")]

        public static void Split()
        {
            Sprite[] sprites = Selection.GetFiltered<Sprite>(SelectionMode.Deep);

            Texture2D oldTexture = null;
            int width = 0;
            int height = 0;
            Color[] pixels = null;
            string textureName = "";
            TextureFormat oldFormat = 0;

            string parentPath = Application.dataPath + "/SpriteOut/";
            if (!Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }
            for (int i = 0; i < sprites.Length; i++)
            {

                Texture2D oldTex = sprites[i].texture;
                if (oldTexture == null || oldTexture != oldTex)
                {
                    oldTexture = oldTex;
                    width = oldTexture.width;
                    height = oldTexture.height;
                    pixels = oldTexture.GetPixels();
                    textureName = oldTexture.name;
                    oldFormat = oldTexture.format;
                }
                Rect rect = sprites[i].rect;
                string name = textureName + "_" + sprites[i].name;

                Debug.Log(name + ":" + rect);
                int spritew = Mathf.FloorToInt(rect.width);
                int spriteh = Mathf.FloorToInt(rect.height);
                int left = Mathf.FloorToInt(rect.x);
                int up = Mathf.FloorToInt(rect.y);
                Texture2D newTex = new Texture2D(spritew, spriteh, oldFormat, false, false);
                Color[] newColors = new Color[spritew * spriteh];
                for (int x = 0; x < spritew; x++)
                {
                    for (int y = 0; y < spriteh; y++)
                    {

                        newColors[y * spritew + x] = pixels[(up + y) * width + left + x];
                    }
                }
                newTex.SetPixels(newColors);
                byte[] data = newTex.EncodeToPNG();
                string filePath = Application.dataPath + "/SpriteOut/" + name + ".png";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                FileStream file = File.Create(filePath);
                file.Write(data, 0, data.Length);
                file.Close();

            }
        }
    }

}

