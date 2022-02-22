using UnityEditor;
using UnityEngine;

namespace Arvin.AlignTools
{
    public class AlignToolsWindow : EditorWindow
    {
        const int AXIS_X = 0;
        const int AXIS_Y = 1;
        const int AXIS_Z = 2;
        const int AXIS_LEFT = 0;
        const int AXIS_RIGHT = 1;
        const int AXIS_TOP = 2;
        const int AXIS_DOWN = 3;

        private Ruler _ruler;
        private string[] _modesStr = new string[] { "场景" };

        private bool needPepaintScene = false;

        // Update the editor window when user changes something (mainly useful when selecting objects)
        void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            // head
            EditorGUI.BeginChangeCheck();
            needPepaintScene = EditorGUI.EndChangeCheck();
            ShowWorldMode();
            AdjustPosition.Execute();
            if (needPepaintScene)
                SceneView.RepaintAll();
        }

        // private void ShowUGUIMode()
        // {
        //     EditorGUILayout.BeginHorizontal();
        //     DrawButton("align_left", AlignTools.AlignToMin, AXIS_X, "Align Left");
        //     DrawButton("align_center_h", AlignTools.AlignToCenter, AXIS_X, "Align Center by Horizontal");
        //     DrawButton("align_right", AlignTools.AlignToMax, AXIS_X, "Align Right");
        //     EditorGUILayout.EndHorizontal();
        //     EditorGUILayout.BeginHorizontal();
        //     DrawButton("align_top", AlignTools.AlignToMax, AXIS_Y, "Align Top");
        //     DrawButton("align_center_v", AlignTools.AlignToCenter, AXIS_Y, "Align Center by Vertical");
        //     DrawButton("align_bottom", AlignTools.AlignToMin, AXIS_Y, "Align Bottom");
        //     EditorGUILayout.EndHorizontal();
        //
        //     DrawLine();
        //     EditorGUILayout.BeginHorizontal();
        //     DrawButton("distribution_h", AlignTools.DistributionGap, AXIS_X, "Distribute by Horizontal");
        //     DrawButton("distribution_v", AlignTools.DistributionGap, AXIS_Y, "Distribute by Vertical");
        //     EditorGUILayout.LabelField("Order By", GUILayout.Width(60), GUILayout.ExpandWidth(false));
        //     Settings.DistributionOrder = (DistributionOrder)EditorGUILayout.EnumPopup(Settings.DistributionOrder);
        //     EditorGUILayout.EndHorizontal();
        //
        //     DrawLine();
        //     EditorGUILayout.BeginHorizontal();
        //     DrawButton("expand_h", AlignTools.Expand, AXIS_X, "Expand Size by Horizontal");
        //     DrawButton("expand_v", AlignTools.Expand, AXIS_Y, "Expand Size by Vertical");
        //     EditorGUILayout.EndHorizontal();
        //     EditorGUILayout.BeginHorizontal();
        //     DrawButton("shrink_h", AlignTools.Shrink, AXIS_X, "Shrink Size by Horizontal");
        //     DrawButton("shrink_v", AlignTools.Expand, AXIS_Y, "Shrink Size by Vertical");
        //     EditorGUILayout.EndHorizontal();
        //
        //
        //     DrawLine();
        //     Settings.AdjustPositionByKeyboard =
        //         EditorGUILayout.ToggleLeft("Adjust Position By Keyboard", Settings.AdjustPositionByKeyboard);
        //     DrawLine();
        //     if (null == _ruler) _ruler = new Ruler();
        //     EditorGUI.BeginChangeCheck();
        //     Settings.ShowRuler = EditorGUILayout.ToggleLeft("Show Ruler", Settings.ShowRuler);
        //     needPepaintScene |= EditorGUI.EndChangeCheck();
        //
        //     if (Settings.ShowRuler)
        //     {
        //         EditorGUI.BeginChangeCheck();
        //         Settings.RulerLineColor = EditorGUILayout.ColorField("Ruler Line Color", Settings.RulerLineColor);
        //         needPepaintScene |= EditorGUI.EndChangeCheck();
        //     }
        // }

        private void ShowWorldMode()
        {
            EditorGUILayout.BeginHorizontal();
            DrawButton("align_left", AlignToolsWorld.AlignToMin, AXIS_X, "左对齐");
            DrawButton("align_center_h", AlignToolsWorld.AlignToCenter, AXIS_X, "中对齐");
            DrawButton("align_right", AlignToolsWorld.AlignToMax, AXIS_X, "右对齐");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            DrawButton("align_top", AlignToolsWorld.AlignToMax, AXIS_Y, "上对齐");
            DrawButton("align_center_v", AlignToolsWorld.AlignToCenter, AXIS_Y, "Y 轴中心对齐");
            DrawButton("align_bottom", AlignToolsWorld.AlignToMin, AXIS_Y, "下对齐");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            // DrawButton("align_max_z", AlignToolsWorld.AlignToMax, AXIS_Z, "Align Min by Axis Z");
            // DrawButton("align_center_z", AlignToolsWorld.AlignToCenter, AXIS_Z, "Align Center by Axis Z");
            // DrawButton("align_min_z", AlignToolsWorld.AlignToMin, AXIS_Z, "Align Max by Axis Z");
            EditorGUILayout.EndHorizontal();

            // DrawLine();
            // EditorGUILayout.BeginHorizontal();
            // DrawButton("distribution_h", AlignToolsWorld.Distribution, AXIS_X, "按X轴分布");
            // DrawButton("distribution_v", AlignToolsWorld.Distribution, AXIS_Y, "按Y轴分布");
            // DrawButton("distribution_z", AlignToolsWorld.Distribution, AXIS_Z, "按Z轴分布");
            // EditorGUILayout.LabelField("Order By", GUILayout.Width(60), GUILayout.ExpandWidth(false));
            // Settings.DistributionOrder = (DistributionOrder)EditorGUILayout.EnumPopup(Settings.DistributionOrder);
            // EditorGUILayout.EndHorizontal();

            DrawLine();
            EditorGUILayout.BeginHorizontal();
            DrawButton("left", AlignTools.Tiled, AXIS_LEFT, "往左平铺");
            DrawButton("right", AlignTools.Tiled, AXIS_RIGHT, "往右平铺");
            // DrawButton("top", AlignTools.Tiled, AXIS_TOP, "往上平铺");
            // DrawButton("down", AlignTools.Tiled, AXIS_DOWN, "往下平铺");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            // DrawButton("shrink_h", AlignTools.Distribution, "Shrink Size by Horizontal");
            // DrawButton("shrink_v", AlignTools.Distribution, "Shrink Size by Vertical");
            EditorGUILayout.EndHorizontal();

            DrawLine();
            // Settings.AdjustPositionByKeyboard =
            //     EditorGUILayout.ToggleLeft("Adjust Position By Keyboard", Settings.AdjustPositionByKeyboard);
        }

        private void DrawLine()
        {
            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
        }

        private GUIContent btnContent;

        private void DrawButton(string iconName, System.Action action, string tooltip = null)
        {
            if (null == btnContent) btnContent = new GUIContent();
            btnContent.image = Utils.LoadTexture(iconName);
            btnContent.tooltip = tooltip;
            if (GUILayout.Button(btnContent, GUILayout.ExpandWidth(false)))
                action();
        }

        private void DrawButton(string iconName, System.Action<int> action, int axis, string tooltip = null)
        {
            if (null == btnContent) btnContent = new GUIContent();
            btnContent.image = Utils.LoadTexture(iconName);
            btnContent.tooltip = tooltip;
            if (GUILayout.Button(btnContent, GUILayout.ExpandWidth(false)))
                action(axis);
        }


        private void OnEnable()
        {
            Utils.editorPath =
                System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        private void OnDisable()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemOnGUI;
        }

        private void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            AdjustPosition.Execute();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            AdjustPosition.Execute();
            if (_ruler != null && Settings.OperatorMode == OperatorMode.UGUI)
                _ruler.OnSceneGUI(sceneView);
        }
    }
}