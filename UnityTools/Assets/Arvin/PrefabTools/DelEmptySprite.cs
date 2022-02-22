using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DelEmptySprite
{
    [MenuItem("GameObject/场景工具/ 清理Miss 图片")]
    static void Run()
    {
        var games = Selection.gameObjects;
        List<GameObject> needDel = new List<GameObject>();
        foreach (var item in games)
        {
            var renders = item.GetComponentsInChildren<SpriteRenderer>();
            if (renders == null || renders.Length == 0)
            {
                continue;
            }

            for (int i = 0; i < renders.Length; i++)
            {
                var subRender = renders[i];
                if (subRender.sprite == null)
                {
                    needDel.Add(subRender.gameObject);
                }
            }
        }

        for (int i = 0; i < needDel.Count; i++)
        {
            var game = needDel[i];
            GameObject.DestroyImmediate(game);
        }
    }
}