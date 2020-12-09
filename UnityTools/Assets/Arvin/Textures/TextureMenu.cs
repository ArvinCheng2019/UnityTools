using System;
using System.Threading.Tasks;
using UnityEditor;
using TextureTool;
using UnityEngine;

namespace Arvin
{
    public class TextureMenu : EditorWindow
    {
        [MenuItem("Assets/Arvin/图片优化/添加到统一优化列表")]
        private static void AddFolderToList()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            UnrealBar.ShowUnrealBar();
            var obj = Selection.activeObject;
            var type = obj.GetType();
            if (type != typeof(DefaultAsset))
            {
                EditorUtility.DisplayDialog("请选择文件夹", "这个优化不针对单个文件，只针对文件夹", "了解");
                return;
            }

            string path = AssetDatabase.GetAssetPath(obj);
            texture.AddTexturePath(path);
        }

        [MenuItem("Assets/Arvin/图片优化/从优化列表删除")]
        private static void DelFolderToList()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            UnrealBar.ShowUnrealBar();
            var obj = Selection.activeObject;
            if (obj is DefaultAsset)
            {
                EditorUtility.DisplayDialog("请选择文件夹", "这个优化不针对单个文件，只针对文件夹", "了解");
                return;
            }

            string path = AssetDatabase.GetAssetPath(obj);
            texture.RemoveTextures(path);
        }

        [MenuItem("Assets/Arvin/图片优化/添加到自定义处理")]
        private static void AddToSelfCompress()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            UnrealBar.ShowUnrealBar();
            var objs = Selection.objects;
            bool isReturn = false;
            foreach (var obj in objs)
            {
                if (obj is DefaultAsset)
                {
                    EditorUtility.DisplayDialog("这个功能只针对单个文件", "这个功能只是针对文件，如果整个文件夹不做处理，请使用其他命令", "了解");
                    isReturn = true;
                    break;
                }
            }

            if (isReturn)
            {
                return;
            }

            foreach (var obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                texture.AddResToSelfCompress(path);
            }
        }

        [MenuItem("Assets/Arvin/图片优化/从自定义列表移除")]
        private static void DelFromSelfCompress()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            UnrealBar.ShowUnrealBar();
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                texture.DelResFromSelfCompress(path);
            }
        }

        [MenuItem("Arvin/图片/执行图片优化")]
        private static void RunTexureOptimization()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            texture.Run();
        }

        [MenuItem("Arvin /图片/图片查看器 &T")]
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