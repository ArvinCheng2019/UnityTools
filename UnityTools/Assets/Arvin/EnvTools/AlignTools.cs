using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arvin.AlignTools
{
    public class AlignTools
    {
        delegate void CalcValueOne(int axis, Vector3[] corners, bool isFirst, ref float v);

        delegate float CalcValueTwo(int axis, Vector3[] corners, bool isFirst, ref float minV, ref float maxV);

        delegate float CalcSize(int axis, Vector3[] corners, out float minV, out float maxV);

        delegate Vector3 ApplyValue(int axis, RectTransform rt, float v);

        public static void AlignToMin(int axis)
        {
            AlignUI(axis, CalcValueMin, ApplyValueMin);
        }

        public static void AlignToMax(int axis)
        {
            AlignUI(axis, CalcValueMax, ApplyValueMax);
        }

        public static void AlignToCenter(int axis)
        {
            AlignCenterUI(axis, CalcValueCenter, ApplyValueCenter);
        }

        public static void Distribution(int axis)
        {
            DistributionUI(axis, CalcValueCenter, ApplyValueCenter);
        }

        public static void DistributionGap(int axis)
        {
            DistributionGapUI(axis, CalcUISize, ApplyValueCenter);
        }

        public static void Expand(int axis)
        {
            ExpandUI(axis, CalcUISize, ApplyValueSize);
        }

        public static void Shrink(int axis)
        {
            SharkUI(axis, CalcUISize, ApplyValueSize);
        }

        public static void Tiled(int axis)
        {
            var list = Selection.objects;
            List<GameObject> listGO = new List<GameObject>();

            bool isAllNameHasSize = true;
            foreach (var item in list)
            {
                string[] names = item.name.Split('%');
                // if (names.Length < 2)
                // {
                //     EditorUtility.DisplayDialog("提示", $"这个Prefab [{item.name}] 没有尺寸信息，正常为name%number%number，请检查选择的内容",
                //         "OK");
                //     isAllNameHasSize = false;
                //     break;
                // }

                listGO.Add(item as GameObject);
            }

            if (!isAllNameHasSize) return;

            if (list.Length <= 1)
            {
                EditorUtility.DisplayDialog("提示", "请选择至少2个物体", "OK");
                return;
            }

            var baseOne = listGO[0].transform; //  选中的第一个是基础
            // //var render = baseOne.GetComponent<SpriteRenderer>();
            //
            // if (render == null)
            // {
            //     EditorUtility.DisplayDialog("提示", " 目前平铺工具 不支持 多图的情况", "OK");
            //     return;
            // }

            float lastPivotX = 0;
            float lastPivotY = 0;

            float lastX = baseOne.localPosition.x;
            float lastY = baseOne.localPosition.y;

            for (int i = 0; i < listGO.Count; i++)
            {
                var next = listGO[i];
                var nextRender = next.GetComponent<SpriteRenderer>();
                float pivotX = nextRender.sprite.pivot.x;
                float pivotY = nextRender.sprite.pivot.y;

                // float pivotX = 0;
                // float pivotY = 0;
                // getPivot(next.name, ref pivotX, ref pivotY);
                float x = 0;
                float y = 0;
                float scaleX = next.transform.localScale.x;
                float scaleY = next.transform.localScale.y;
                switch (axis)
                {
                    case 0: //AXIS_LEFT:
                        x = (lastPivotX + pivotX) * 0.01f * scaleX + lastX;
                        y = baseOne.localPosition.y;
                        break;
                    case 1: // AXIS_RIGHT:
                        if (next.transform.localPosition.x < 0)
                        {
                            x = lastX - (lastPivotX + pivotX) * scaleX * 0.01f;
                        }
                        else
                        {
                            x = lastX - (lastPivotX + pivotX) * scaleX * 0.01f;
                        }

                        y = baseOne.localPosition.y;
                        break;
                    case 2: // AXIS_TOP:
                        x = baseOne.localPosition.x;
                        y = (lastPivotY + pivotY) * 0.01f * scaleY + lastY;
                        break;
                    case 3: //AXIS_DOWN:
                        y = lastY - (lastPivotY + pivotY) * scaleY * 0.01f;
                        x = baseOne.localPosition.x;
                        break;
                }


                if (i == 0)
                {
                    x = lastX;
                    y = lastY;
                }

                lastX = x;
                lastY = y;
                next.transform.localPosition = new Vector3(x, y, baseOne.localPosition.z);
                lastPivotX = pivotX;
                lastPivotY = pivotY;
            }
        }

        static void getPivot(string name, ref float pivotX, ref float pivotY)
        {
            string[] names = name.Split("%");
            string x = names[1];
            string y = names[2];

            string[] ends = y.Split("_");
            string realY = ends[0];

            pivotX = float.Parse(x) * 0.5f;
            pivotY = float.Parse(realY) * 0.5f;
        }

        #region logic

        private static void AlignUI(int axis, CalcValueOne calcValue, ApplyValue applyValue)
        {
            var list = Utils.GetRectTransforms();
            if (list.Count < 1) return;

            float v = 0f;
            Vector3[] corners = new Vector3[4];
            if (list.Count == 1)
            {
                var parent = list[0].parent as RectTransform;
                if (parent == null) return;
                parent.GetWorldCorners(corners);
                calcValue(axis, corners, true, ref v);
            }
            else
            {
                for (var i = 0; i < list.Count; i++)
                {
                    list[i].GetWorldCorners(corners);
                    calcValue(axis, corners, 0 == i, ref v);
                }
            }

            foreach (var rt in list)
            {
                var pos = applyValue(axis, rt, v);
                Undo.RecordObject(rt, "Align UI");
                rt.anchoredPosition3D = pos;
            }
        }

        private static void AlignCenterUI(int axis, CalcValueTwo calcValue, ApplyValue applyValue)
        {
            var list = Utils.GetRectTransforms();
            if (list.Count < 1) return;

            float minV = 0f, maxV = 0f;
            Vector3[] corners = new Vector3[4];
            if (list.Count == 1)
            {
                var parent = list[0].parent as RectTransform;
                if (parent == null) return;
                parent.GetWorldCorners(corners);
                calcValue(axis, corners, true, ref minV, ref maxV);
            }
            else
            {
                for (var i = 0; i < list.Count; i++)
                {
                    list[i].GetWorldCorners(corners);
                    calcValue(axis, corners, 0 == i, ref minV, ref maxV);
                }
            }

            float v = (minV + maxV) * 0.5f;
            foreach (var rt in list)
            {
                var pos = applyValue(axis, rt, v);
                Undo.RecordObject(rt, "Align Center UI");
                rt.anchoredPosition3D = pos;
            }
        }

        struct Value
        {
            public RectTransform rt;
            public float v;
            public float size;
        }

        private static void DistributionUI(int axis, CalcValueTwo calcValue, ApplyValue applyValue)
        {
            var list = Utils.GetRectTransforms();
            if (list.Count < 3) return;

            var vlist = new List<Value>(list.Count);

            float minV = 0f, maxV = 0f;
            Vector3[] corners = new Vector3[4];
            for (var i = 0; i < list.Count; i++)
            {
                list[i].GetWorldCorners(corners);
                vlist.Add(new Value
                {
                    rt = list[i],
                    v = calcValue(axis, corners, 0 == i, ref minV, ref maxV)
                });
            }

            ;


            switch (Settings.DistributionOrder)
            {
                case DistributionOrder.Position:
                    vlist.Sort(SortByPosition);
                    break;
                case DistributionOrder.Hierarchy:
                    vlist.Sort(SortByHierarchy);
                    break;
                case DistributionOrder.HierarchyFlipY:
                    vlist.Sort(SortByHierarchy);
                    vlist.Reverse();
                    break;
            }

            float gap = (maxV - minV) / (list.Count - 1);
            for (var i = 1; i < vlist.Count - 1; i++)
            {
                var rt = vlist[i].rt;
                var pos = applyValue(axis, rt, minV + gap * i);
                Undo.RecordObject(rt, "Distribution UI");
                rt.anchoredPosition3D = pos;
            }
        }

        private static void DistributionGapUI(int axis, CalcSize calcSize, ApplyValue applyValue)
        {
            var list = Utils.GetRectTransforms();
            if (list.Count < 3) return;

            var vlist = new List<Value>(list.Count);

            float minV = 0f, maxV = 0f;
            float sumSize = 0f;
            Vector3[] corners = new Vector3[4];
            for (var i = 0; i < list.Count; i++)
            {
                list[i].GetWorldCorners(corners);
                float _minV, _maxV;
                float size = calcSize(axis, corners, out _minV, out _maxV);
                minV = 0 == i ? _minV : Mathf.Min(_minV, minV);
                maxV = 0 == i ? _maxV : Mathf.Max(_maxV, maxV);
                sumSize += size;
                vlist.Add(new Value
                {
                    rt = list[i],
                    v = (_minV + _maxV) / 2,
                    size = size,
                });
            }

            ;

            switch (Settings.DistributionOrder)
            {
                case DistributionOrder.Position:
                    vlist.Sort(SortByPosition);
                    break;
                case DistributionOrder.Hierarchy:
                    vlist.Sort(SortByHierarchy);
                    break;
                case DistributionOrder.HierarchyFlipY:
                    vlist.Sort(SortByHierarchy);
                    vlist.Reverse();
                    break;
            }

            float gap = (maxV - minV - sumSize) / (list.Count - 1);
            float curV = minV;
            for (var i = 0; i < vlist.Count; i++)
            {
                var rt = vlist[i].rt;
                var pos = applyValue(axis, rt, curV + vlist[i].size / 2);
                curV += vlist[i].size + gap;
                Undo.RecordObject(rt, "Distribution UI By Gap");
                rt.anchoredPosition3D = pos;
            }
        }

        private static void ExpandUI(int axis, CalcSize calcSize, ApplyValue applyValue)
        {
            var list = Utils.GetRectTransforms();
            if (list.Count < 2) return;

            float size = 0f;
            Vector3[] corners = new Vector3[4];
            for (var i = 0; i < list.Count; i++)
            {
                list[i].GetWorldCorners(corners);
                float _minV, _maxV;
                size = Mathf.Max(size, calcSize(axis, corners, out _minV, out _maxV));
            }

            foreach (var rt in list)
            {
                var v = applyValue(axis, rt, size);
                Undo.RecordObject(rt, "Expand or Shark UI");
                rt.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, v.x);
            }
        }

        private static void SharkUI(int axis, CalcSize calcSize, ApplyValue applyValue)
        {
            var list = Utils.GetRectTransforms();
            if (list.Count < 2) return;

            float size = 10000000f;
            Vector3[] corners = new Vector3[4];
            for (var i = 0; i < list.Count; i++)
            {
                list[i].GetWorldCorners(corners);
                float _minV, _maxV;
                size = Mathf.Min(size, calcSize(axis, corners, out _minV, out _maxV));
            }

            foreach (var rt in list)
            {
                var v = applyValue(axis, rt, size);
                Undo.RecordObject(rt, "Expand or Shark UI");
                rt.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, v.x);
            }
        }

        #endregion


        #region calc value

        private static void CalcValueMin(int axis, Vector3[] corners, bool isFirst, ref float v)
        {
            if (isFirst)
                v = corners[0][axis];
            else
                v = Mathf.Min(v, corners[0][axis]);
        }

        private static void CalcValueMax(int axis, Vector3[] corners, bool isFirst, ref float v)
        {
            if (isFirst)
                v = corners[2][axis];
            else
                v = Mathf.Max(v, corners[2][axis]);
        }

        private static float CalcValueCenter(int axis, Vector3[] corners, bool isFirst, ref float minV, ref float maxV)
        {
            var v = (corners[0][axis] + corners[2][axis]) * 0.5f;
            if (isFirst)
                minV = maxV = v;
            else
            {
                minV = Mathf.Min(minV, v);
                maxV = Mathf.Max(maxV, v);
            }

            return v;
        }

        #endregion

        #region calc size min and max

        private static float CalcUISize(int axis, Vector3[] corners, out float minV, out float maxV)
        {
            minV = Mathf.Min(corners[0][axis], corners[2][axis]);
            maxV = Mathf.Max(corners[0][axis], corners[2][axis]);
            return maxV - minV;
        }

        #endregion

        #region applay value

        private static Vector3 ApplyValueMin(int axis, RectTransform rt, float v)
        {
            var vpos = Vector3.zero;
            vpos[axis] = v;
            var interPos = rt.InverseTransformPoint(vpos);
            var pos = rt.anchoredPosition3D;
            pos[axis] += interPos[axis] + rt.pivot[axis] * rt.rect.size[axis];
            return pos;
        }

        private static Vector3 ApplyValueMax(int axis, RectTransform rt, float v)
        {
            var vpos = Vector3.zero;
            vpos[axis] = v;
            var interPos = rt.InverseTransformPoint(vpos);
            var pos = rt.anchoredPosition3D;
            pos[axis] += interPos[axis] - (1 - rt.pivot[axis]) * rt.rect.size[axis];
            return pos;
        }

        private static Vector3 ApplyValueCenter(int axis, RectTransform rt, float v)
        {
            var vpos = Vector3.zero;
            vpos[axis] = v;
            var interPos = rt.InverseTransformPoint(vpos);
            var pos = rt.anchoredPosition3D;
            pos[axis] += interPos[axis] + (rt.pivot[axis] - 0.5f) * rt.rect.size[axis];
            return pos;
        }

        private static Vector3 ApplyValueSize(int axis, RectTransform rt, float v)
        {
            var vpos = Vector3.zero;
            vpos[axis] = v;
            var interV = rt.InverseTransformVector(vpos);
            v = interV[axis] * rt.localScale[axis];
            return new Vector3(v, v, v);
        }

        #endregion


        private static int SortByPosition(Value a, Value b)
        {
            if (Mathf.Approximately(a.v, b.v)) return 0;
            if (a.v < b.v) return -1;
            else if (a.v > b.v) return 1;
            return 0;
        }

        private static Dictionary<Transform, int> s_Sets = new Dictionary<Transform, int>();

        private static int SortByHierarchy(Value a, Value b)
        {
            // 是否兄弟节点
            if (a.rt.parent == b.rt.parent)
                return a.rt.GetSiblingIndex() - b.rt.GetSiblingIndex();

            // 是否非跟节点
            var rootA = a.rt.root;
            var rootB = b.rt.root;
            if (rootA != rootB)
                return rootA.GetSiblingIndex() - rootB.GetSiblingIndex();

            s_Sets.Clear();
            int siblingIndx = -1;
            Transform transA = a.rt;
            do
            {
                s_Sets.Add(transA, siblingIndx);
                siblingIndx = transA.GetSiblingIndex();
                transA = transA.parent;
            } while (transA != null);


            Transform transB = b.rt;
            while (transB != null)
            {
                if (s_Sets.TryGetValue(transB.parent, out siblingIndx))
                {
                    s_Sets.Clear();
                    return siblingIndx - transB.GetSiblingIndex();
                }

                transB = transB.parent;
            }

            s_Sets.Clear();
            return 0;
        }
    }
}