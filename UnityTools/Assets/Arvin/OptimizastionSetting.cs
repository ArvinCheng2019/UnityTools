using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Arvin
{
    public class OptimizastionSetting : ScriptableObject
    {
        // 图片
        [FormerlySerializedAs("CloseMipMap")] [Tooltip("默认关闭图片的Mip Map")]
        public bool Texture_CloseMipMap = true;

        [FormerlySerializedAs("DefaultFormat")] [Tooltip("默认的图片压缩格式")]
        public TextureImporterFormat Texture_DefaultFormat = TextureImporterFormat.ASTC_6x6;

        [Tooltip("默认的UI图片压缩格式")] public TextureImporterFormat UI_DefaultFormat = TextureImporterFormat.ASTC_5x5;

        // 音效
        [FormerlySerializedAs("DefaultAudioClipFormat")] [Tooltip("默认的短音效处理")]
        public AudioRules AudioClip_DefaultFormat = new AudioRules()
        {
            length = 5,
            start = 0,
            Setting = new AudioRuleSetting()
            {
                forceToMono = true,
                Ambisonic = false,
                loadBackground = false,
                loadType = AudioClipLoadType.DecompressOnLoad,
                compression = AudioCompressionFormat.ADPCM,
                sampleRate = AudioSampleRateSetting.PreserveSampleRate,
                Rate = SampleRate.HZ_11025
            }
        };

        [FormerlySerializedAs("DefaultMusicFormat")] [Tooltip("默认中长音频短处理")]
        public AudioRules AudioClip_DefaultMusicFormat = new AudioRules()
        {
            length = 100000,
            start = 5,
            Setting = new AudioRuleSetting()
            {
                forceToMono = true,
                Ambisonic = false,
                loadBackground = false,
                loadType = AudioClipLoadType.Streaming,
                compression = AudioCompressionFormat.Vorbis,
                sampleRate = AudioSampleRateSetting.OverrideSampleRate,
                Rate = SampleRate.HZ_8000
            }
        };

        // 模型

        [FormerlySerializedAs("IsImportUV2")] public bool Model_IsImportUV2 = false;

        [FormerlySerializedAs("OptimizeMesh")] [Tooltip("OptimizeMesh:顶点优化选项,开启后顶点将被重新排序,GPU性能可以得到提升")]
        public bool Model_OptimizeMesh = true;

        [FormerlySerializedAs("OptimizeGameObjects")] [Tooltip("优化骨骼节点")]
        public bool Model_OptimizeGameObjects = true;

        [FormerlySerializedAs("ImportMaterials")] [Tooltip("不需要导入Material")]
        public bool Model_ImportMaterials = false;

        [FormerlySerializedAs("ImportCameras")] [Tooltip("是否导入摄像机")]
        public bool Model_ImportCameras = false;

        [FormerlySerializedAs("ImportLights")] [Tooltip("是否导入灯光")]
        public bool Model_ImportLights = false;

        [FormerlySerializedAs("MeshCompression")] [Tooltip("模型压缩方式")]
        public ModelImporterMeshCompression Model_MeshCompression = ModelImporterMeshCompression.Off;

        [Tooltip("动画压缩方式")]
        public ModelImporterAnimationCompression Model_AnimCompression = ModelImporterAnimationCompression.Optimal;

        public ModelImporterTangents Model_ImportTangents = ModelImporterTangents.None;

        public ModelImporterNormals Model_ImportNormals = ModelImporterNormals.Import;

        [Tooltip("是否可读写，常规只有特效需要开启")] public bool Model_Readable = false;

        [FormerlySerializedAs("GenerateSecondaryUV")] [Tooltip("是否导入 UV2")]
        public bool Model_GenerateSecondaryUV = false;

        // 动画
        [Tooltip("移除动画的缩放系数")] public bool Anim_RemoveAnimScale = true;
        [Tooltip("分离动画到这个目录")] public string Anim_ExportClipPath = "Assets/Res/Animations/";

        // 特效
        [Tooltip("特效发射的最大粒子数")] public int Effect_MaxCount = 50;

        [Tooltip("当特效上当Render 是Disable 状态时，关闭发射器")]
        public bool Effect_CloseEmmit = true;

        private void OnValidate()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}