using UnityEditor;


namespace Arvin.AlignTools
{
    public static class AlignToolsMenu
    {
        private const string WindowMenuPath = "Kunpo/对齐工具 %#K";

        // Creation of window
        [MenuItem(WindowMenuPath)]
        private static void AlignToolsWindows()
        {
            AlignToolsWindow window = EditorWindow.GetWindow<AlignToolsWindow>(false, "对齐工具", true);
            window.Show();
            window.autoRepaintOnSceneChange = true;
        }
    }
}


