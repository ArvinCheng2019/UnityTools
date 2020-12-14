using System;
using UnityEditor;
using UnityEngine;

namespace Arvin
{
    [CreateAssetMenu]
    public class OptimizastionSetting : ScriptableObject
    {
        // 图片
        [Tooltip("默认关闭图片的Mip Map")] public bool CloseMipMap = true;
        [Tooltip("默认的图片压缩格式")] public TextureImporterFormat DefaultFormat = TextureImporterFormat.ASTC_6x6;

        // 音效
        [Tooltip("默认的短音效处理")] public AudioRules DefaultAudioClipFormat = new AudioRules()
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

        [Tooltip("默认中长音频短处理")] public AudioRules DefaultMusicFormat = new AudioRules()
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

        public bool IsImportUV2 = false;

        [Tooltip("OptimizeMesh:顶点优化选项,开启后顶点将被重新排序,GPU性能可以得到提升")]
        public bool OptimizeMesh = true;

        [Tooltip("优化骨骼节点")] public bool OptimizeGameObjects = true;
        [Tooltip("不需要导入Material")] public bool ImportMaterials = false;

        [Tooltip("是否导入摄像机")] public bool ImportCameras = false;
        [Tooltip("是否导入灯光")] public bool ImportLights = false;
        [Tooltip("模型压缩方式")] public ModelImporterMeshCompression MeshCompression = ModelImporterMeshCompression.Off;

        [Tooltip("动画压缩方式")]
        public ModelImporterAnimationCompression AnimationCompression = ModelImporterAnimationCompression.Optimal;
        public ModelImporterTangents ImportTangents = ModelImporterTangents.None;
        public ModelImporterNormals ImportNormals = ModelImporterNormals.Import;
        [Tooltip("是否可读写，常规只有特效需要开启")] public bool IsReadable = false;
        [Tooltip("是否导入 UV2")] public bool GenerateSecondaryUV = false;

        // 动画
        public bool IsRemoveAnimScale = true;

        // 特效
        [Tooltip("特效发射的最大粒子数")] public int MaxParticleCount = 50;

        [Tooltip("当特效上当Render 是Disable 状态时，关闭发射器")]
        public bool CloseParticleEmmit = true;

        private void OnValidate()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}