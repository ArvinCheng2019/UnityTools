using System;
using Arvin;
using UnityEditor;
using UnityEngine;

public class OptimizastionWindow : EditorWindow
{
    private static OptimizastionSetting setting;
    private Editor editor;

    [MenuItem("Kunpo/优化工具/默认设置窗口")]
    public static void Open()
    {
        var window = EditorWindow.GetWindow<OptimizastionWindow>(true, "OptimizastionWindow", true);
        setting = ScriptableHelper.GetOptimizastionSetting();
        window.editor = Editor.CreateEditor(setting);

        // EditorWindow window = EditorWindow.GetWindow(typeof(OptimizastionWindow));
        // window. = Editor.CreateEditor(ScriptableObject.CreateInstance<ShowObject>());
        // window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("说明", EditorStyles.boldLabel);
        GUILayout.Label("以下为通用设置，除图片压缩方式，音效长度范围两项，其他的选项尽量不做修改，以下策略对加入自定义列表的资源不生效,", EditorStyles.label);
        GUILayout.Label("一定要点在最底下的保存按钮，不点不生效。", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        this.editor.OnInspectorGUI();

        EditorGUILayout.Space();
        if (GUILayout.Button("保存 & 修改"))
        {
            EditorUtility.SetDirty(setting);
            var texture = ScriptableHelper.GetTextureOptimization();
            texture.UpdateTextureSetting(setting.Texture_DefaultFormat);
            var sound = ScriptableHelper.GetSoundOptimization();
            
        }
    }

    // public void OnGUI()
    // {
    //     EditorGUILayout.Space(10);
    //     GUILayout.Label("说明", EditorStyles.boldLabel);
    //     GUILayout.Label("以下为通用设置，除图片压缩方式，音效长度范围两项，其他的选项尽量不做修改，以下策略对加入自定义列表的资源不生效,", EditorStyles.label);
    //     GUILayout.Label("一定要点在最底下的保存按钮，不点不生效。", EditorStyles.boldLabel);
    //     EditorGUILayout.Space(20);
    //
    //     GUILayout.Label("图片项默认设置", EditorStyles.boldLabel);
    //     EditorGUILayout.Space();
    //     setting.CloseMipMap = EditorGUILayout.Toggle("关闭 MipMap", setting.CloseMipMap);
    //     setting.DefaultFormat = (TextureImporterFormat) EditorGUILayout.EnumPopup("默认的图片压缩方式", setting.DefaultFormat);
    //     EditorGUILayout.Space(20);
    //     GUILayout.Label("音频项默认设置", EditorStyles.boldLabel);
    //     EditorGUILayout.Space();
    //
    //     EditorGUILayout.Space(20);
    //     GUILayout.Label("模型项默认设置", EditorStyles.boldLabel);
    //     EditorGUILayout.Space();
    //     setting.ImportCameras = EditorGUILayout.Toggle("打开 导入摄像机选项", setting.ImportCameras);
    //     setting.ImportLights = EditorGUILayout.Toggle("打开 导入灯光选项", setting.ImportLights);
    //     setting.ImportMaterials = EditorGUILayout.Toggle("打开 导入材质球选项", setting.ImportMaterials);
    //     setting.IsImportUV2 = EditorGUILayout.Toggle("打开 导入UV2选项", setting.IsImportUV2);
    //     setting.OptimizeMesh = EditorGUILayout.Toggle("优化mesh", setting.OptimizeMesh);
    //     setting.GenerateSecondaryUV = EditorGUILayout.Toggle("GenerateSecondaryUV", setting.GenerateSecondaryUV);
    //     setting.OptimizeGameObjects = EditorGUILayout.Toggle("优化 meshGameObjects ", setting.OptimizeGameObjects);
    //     setting.IsReadable = EditorGUILayout.Toggle("设置成 可读", setting.IsReadable);
    //     setting.ImportNormals =
    //         (ModelImporterNormals) EditorGUILayout.EnumPopup("导入 Normals 选项", setting.ImportNormals);
    //     setting.ImportTangents =
    //         (ModelImporterTangents) EditorGUILayout.EnumPopup("导入 Tangents 选项", setting.ImportTangents);
    //     setting.MeshCompression =
    //         (ModelImporterMeshCompression) EditorGUILayout.EnumPopup("压缩模型选项", setting.MeshCompression);
    //
    //     EditorGUILayout.Space(20);
    //     GUILayout.Label("动画项默认设置", EditorStyles.boldLabel);
    //     EditorGUILayout.Space();
    //     setting.IsRemoveAnimScale = EditorGUILayout.Toggle("是否移除动画里的缩放", setting.IsRemoveAnimScale);
    //     setting.AnimationCompression =
    //         (ModelImporterAnimationCompression) EditorGUILayout.EnumPopup("动画导入方式", setting.AnimationCompression);
    //
    //     EditorGUILayout.Space(20);
    //     GUILayout.Label("特效项默认设置", EditorStyles.boldLabel);
    //     setting.MaxParticleCount = EditorGUILayout.IntField("特效发射的最大粒子数 ", setting.MaxParticleCount);
    //     setting.CloseParticleEmmit =
    //         EditorGUILayout.Toggle("无render关发射器", setting.CloseParticleEmmit);
    //     EditorGUILayout.Space(20);
    //

    // }
}