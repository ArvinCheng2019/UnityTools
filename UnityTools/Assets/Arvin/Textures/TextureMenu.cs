using System;
using System.Threading.Tasks;
using UnityEditor;
using TextureTool;
using UnityEngine;

namespace Arvin
{
    public class TextureMenu : EditorWindow
    {
        [MenuItem("Assets/Arvin/优化工具/添加到UI图片列表", false, 1)]
        private static void AddUIFolderToList()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            UnrealBar.ShowUnrealBar();
            var objs = Selection.objects;
            bool isAllReady = true;
            foreach (var obj in objs)
            {
                var type = obj.GetType();
                if (type != typeof(DefaultAsset))
                {
                    EditorUtility.DisplayDialog("请选择文件夹", "这个优化不针对单个文件，只针对文件夹", "了解");
                    isAllReady = false;
                    break;
                }
            }

            if (!isAllReady)
            {
                return;
            }

            foreach (var obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                texture.AddUITexturePath(path);
            }

            EditorUtility.SetDirty(texture);
        }

        [MenuItem("Assets/Arvin/优化工具/从UI图片列表移除", false, 2)]
        private static void RemoveUIFolderFromList()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            UnrealBar.ShowUnrealBar();
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                texture.RemoveUITextures(path);
            }

            EditorUtility.SetDirty(texture);
        }

        [MenuItem("Assets/Arvin/优化工具/添加图片到优化列表", false, 3)]
        private static void AddFolderToList()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            UnrealBar.ShowUnrealBar();
            var objs = Selection.objects;
            bool isAllReady = true;
            foreach (var obj in objs)
            {
                var type = obj.GetType();
                if (type != typeof(DefaultAsset))
                {
                    EditorUtility.DisplayDialog("请选择文件夹", "这个优化不针对单个文件，只针对文件夹", "了解");
                    isAllReady = false;
                    break;
                }
            }

            if (!isAllReady)
            {
                return;
            }

            foreach (var obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                texture.AddTexturePath(path);
            }

            EditorUtility.SetDirty(texture);
        }

        [MenuItem("Assets/Arvin/优化工具/从图片列表删除", false, 4)]
        private static void DelFolderToList()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            UnrealBar.ShowUnrealBar();
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                texture.RemoveTextures(path);
            }

            EditorUtility.SetDirty(texture);
        }

        [MenuItem("Assets/Arvin/优化工具/自定义/添加到自定义处理", false, 5)]
        private static void AddToSelfCompress()
        {
            var selfRule = ScriptableHelper.GetSelfRuleRes();
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
                selfRule.AddToSelfRule(path, obj);
            }

            EditorUtility.SetDirty(selfRule);
        }

        [MenuItem("Assets/Arvin/优化工具/自定义/从自定义列表移除", false, 6)]
        private static void DelFromSelfCompress()
        {
            var selfRule = ScriptableHelper.GetSelfRuleRes();
            UnrealBar.ShowUnrealBar();
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                selfRule.RemoveSelfRule(path, obj);
            }

            EditorUtility.SetDirty(selfRule);
        }

        [MenuItem("Arvin/优化工具/执行图片优化", false, 7)]
        private static void RunTexureOptimization()
        {
            var texture = ScriptableHelper.GetTextureOptimization();
            texture.Run();
            EditorUtility.SetDirty(texture);
        }

        [MenuItem("Arvin/优化工具 /图片查看器 &T", false, 8)]
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