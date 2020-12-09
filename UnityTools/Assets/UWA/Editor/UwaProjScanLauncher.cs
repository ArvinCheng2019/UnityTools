using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UwaProjScan.Tools;
using UwaProjScan.Submodules.ShaderAnalyzer.Build;

#if UNITY_2018_2_OR_NEWER
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Rendering;
#endif

namespace UwaProjScan
{
    [InitializeOnLoad]
    public static class MainScan
    {
        static MainScan()
        {
            ApiCompatibilityUtils.Instance.Setup(ApiCompatibilityImp.Instance);
        }

        [MenuItem("Tools/UWA Scan/Run", false, 1)]
        public static void DoTestFromMenu()
        {
            API.DoMain(true);
        }

        /// <summary>
        /// 用户通过命令行执行时，调用的是该函数
        /// </summary>
        public static void DoTest()
        {
            API.DoMain(false);
        }

#if UNITY_5_3_OR_NEWER
        [MenuItem("Tools/UWA Scan/Run Effects Scanning", false, 2)]
        public static void RunEffectsScanning()
        {
            API.RunEffectsScanning();
        }
#endif

        [MenuItem("Tools/UWA Scan/Run Custom Rules", false, 2)]
        public static void RunCustomRules()
        {
            API.RunCustomRules();
        }

        [MenuItem("Tools/UWA Scan/Run Luachecker", false, 2)]
        public static void RunLuachecker()
        {
            API.RunLuachecker();
        }

        [MenuItem("Tools/UWA Scan/Run Shader Analyzer", false, 2)]
        public static void RunShaderAnalyzer()
        {
            API.RunShaderAnalyzer();
        }

    }


    class ApiCompatibilityImp : ICompatApi
    {
        public static readonly ApiCompatibilityImp Instance = new ApiCompatibilityImp();
        public ApiCompatibilityImp()
        {
#if UNITY_2017_2_OR_NEWER
            EditorApplication.playModeStateChanged += EditorApplication_playmodeStateChanged;
#else
            EditorApplication.playmodeStateChanged += EditorApplication_playmodeStateChanged;
#endif
        }

        private Action _exitplaymodecb = null;
        private Action<bool> _pausemodecb = null;
        public Action<Shader, UPS_SSD, List<UPS_SCD>> _onProcessShader = null;

        private bool _lastPauseState = false;
#if UNITY_2017_2_OR_NEWER
        public void EditorApplication_playmodeStateChanged(PlayModeStateChange p)
#else
        public void EditorApplication_playmodeStateChanged()
#endif
        {
            if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (_exitplaymodecb != null) _exitplaymodecb();
            }

            // invoke only when isPaused changed
            if (_lastPauseState != EditorApplication.isPaused)
            {
                if (_pausemodecb != null) _pausemodecb(EditorApplication.isPaused);
            }

            _lastPauseState = EditorApplication.isPaused;
        }




        #region interface imp
        public void RegisterEditorExitPlayMode(Action cb)
        {
            _exitplaymodecb += cb;
        }

        public void RegisterEditorPauseMode(Action<bool> cb)
        {
            _pausemodecb += cb;
        }

        public void RegisterProcessShader(Action<Shader, UPS_SSD, List<UPS_SCD>> onProcessShaderInternal)
        {
            _onProcessShader += onProcessShaderInternal;
        }

        public void Reset()
        {
            _exitplaymodecb = null;
            _pausemodecb = null;
            _onProcessShader = null;
        }

        public void UPS_BuildPlayer(string[] scenesInBuild)
        {
#if UNITY_2018_2_OR_NEWER
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenesInBuild;
            buildPlayerOptions.target = EditorUserBuildSettings.activeBuildTarget;

            switch(EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    buildPlayerOptions.locationPathName = "./UWAScan_SA_BuildTest_Standalone/UWAScan_SA_BuildTest.exe";
                    break;
                case BuildTarget.Android:
                    buildPlayerOptions.locationPathName = "./UWAScan_SA_BuildTest_Android";
                    break;
                case BuildTarget.iOS:
                    buildPlayerOptions.locationPathName = "./UWAScan_SA_BuildTest_iOS";
                    break;
                default:
                    buildPlayerOptions.locationPathName = "./UWAScan_SA_Build_Test";
                    break;
            }


            buildPlayerOptions.options = BuildOptions.None;

            BuildPipeline.BuildPlayer(buildPlayerOptions);

#endif
        }

        #endregion

    }


#if UNITY_2018_2_OR_NEWER

    class UPS_SA_BuildProcessor : IPreprocessShaders
    {

        public int callbackOrder { get { return 0; } }

        //以vertex函数作为变体个数的依据
        public void OnProcessShader(
            Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerDatas)
        {
            if (!ApiCompatibilityUtils.UwaShaderProcessorOn) return;

            UPS_SSD u_snippet = new UPS_SSD();
            u_snippet.shaderType = (UPS_SSD.UPS_ShaderType)Enum.Parse(typeof(UPS_SSD.UPS_ShaderType), snippet.shaderType.ToString());
            //Debug.Log(u_snippet.shaderType);
            List<UPS_SCD> u_SCDs = new List<UPS_SCD>();

            for (int i = 0; i < shaderCompilerDatas.Count; ++i)
            {
                ShaderKeywordSet ks = shaderCompilerDatas[i].shaderKeywordSet;
                UPS_SCD u_SCD = new UPS_SCD();
                foreach (ShaderKeyword kw in ks.GetShaderKeywords())
                {
                    string kname;
#if UNITY_2019_3_OR_NEWER
                    kname = ShaderKeyword.GetKeywordName(shader, kw);
#else
                    kname = kw.GetKeywordName();
#endif
                    u_SCD.shaderKeywordSet.Add(kname);
                }
                u_SCDs.Add(u_SCD);

            }

            ApiCompatibilityImp.Instance._onProcessShader.Invoke(shader, u_snippet, u_SCDs);
            shaderCompilerDatas.Clear();

        }
    }
#endif
}
