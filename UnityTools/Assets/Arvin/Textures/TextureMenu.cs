using UnityEditor;
using TextureTool;
using UnityEngine;

namespace Arvin
{
    public class TextureMenu : EditorWindow
    {
        [MenuItem("Assets/Arvin/图片优化/添加文件夹")]
        private static void AddFolderToList()
        {
            var obj = Selection.activeObject;
            var type = obj.GetType();
            if (type != typeof(DefaultAsset))
            {
                EditorUtility.DisplayDialog("请选择文件夹", "这个优化不针对单个文件，只针对文件夹", "了解");
                return;
            }

            string path = AssetDatabase.GetAssetPath(obj);
            var texture = ScriptableHelper.GetTextureOptimization();
            texture.AddTexturePath(path);
        }

        [MenuItem("Assets/Arvin/图片优化/删除文件夹")]
        private static void DelFolderToList()
        {
            var obj = Selection.activeObject;
            var type = obj.GetType();
            if (type != typeof(DefaultAsset))
            {
                EditorUtility.DisplayDialog("请选择文件夹", "这个优化不针对单个文件，只针对文件夹", "了解");
                return;
            }

            string path = AssetDatabase.GetAssetPath(obj);
            var texture = ScriptableHelper.GetTextureOptimization();
            texture.RemoveTextures(path);
        }

        [MenuItem("Arvin/图片/执行图片优化")]
        private static void RunTexureOptimization()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            texture.Run();
        }

        [MenuItem("Arvin /图片/图片查看器 &q")]
        private static void OpenWindow()
        {
            var window = GetWindow<TextureViewerWindow>();
            window.titleContent = ToolConfig.WindowTitle;

            var position = window.position;
            position.width = ToolConfig.InitialHeaderTotalWidth + 50f;
            position.height = 400f;
            window.position = position;

            window.CreateTreeView();
        }
    }
}