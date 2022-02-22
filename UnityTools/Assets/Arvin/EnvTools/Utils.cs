using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arvin.AlignTools
{
    internal static class Utils
    {
        internal static string editorPath = "Assets/Editor/EnvTools";

        internal static Texture LoadTexture(string textureName)
        {
            string path = string.Format("Assets/Editor/Icons/{1}.png", editorPath, textureName);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        internal static Transform[] GetTransforms()
        {
            return Selection.transforms;
        }

        internal static List<RectTransform> GetRectTransforms()
        {
            var arr = Selection.transforms;
            var list = new List<RectTransform>();
            foreach (var trans in arr)
            {
                var rt = trans as RectTransform;
                if (!rt) continue;
                list.Add(rt);
            }

            return list;
        }

        internal static List<Transform> GetWorldTransforms()
        {
            var arr = Selection.transforms;
            var list = new List<Transform>();
            foreach (var trans in arr)
            {
                var rt = trans as RectTransform;
                if (rt) continue;
                list.Add(trans);
            }

            return list;
        }

        internal static void WorldCorners(this RectTransform rt, Vector3[] corners)
        {
            rt.GetWorldCorners(corners);
            //var p1 = corners[0];
            if (corners[0].x > corners[3].x)
                Swap(ref corners[0], ref corners[3]);
            if (corners[1].x > corners[2].x)
                Swap(ref corners[1], ref corners[2]);
            if (corners[0].y > corners[1].y)
                Swap(ref corners[0], ref corners[1]);
            if (corners[3].y > corners[2].y)
                Swap(ref corners[3], ref corners[2]);
        }

        private static void Swap(ref Vector3 v1, ref Vector3 v2)
        {
            var tx = v1;
            v1 = v2;
            v2 = tx;
        }
    }
}