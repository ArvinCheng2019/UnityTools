using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class SoundOptimization : ScriptableObject
{
    [Tooltip("对中长度音效音乐，使用Streaming + Vorbis 。 对短音效，使用Decompress On Load + ADPCM")]
    public List<AudioClip> SelfRules = new List<AudioClip>();

    public List<AudioRules> SoundRules = new List<AudioRules>();

    void Run()
    {
        string[] guids = AssetDatabase.FindAssets("t:AudioClip");
        int index = 0, length = guids.Length;
        EditorUtility.DisplayProgressBar("正在处理", $"正在处理声音文件 {index}/{length}", 0);
        foreach (var guid in guids)
        {
            if (IsSelfRule(guid))
            {
                continue;
            }

            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            float audioLength = clip.length;
            AudioRules rule = SoundRules.Find((x) =>
            {
                int start = x.start;
                int max = x.length;
                if (audioLength >= start && audioLength < max)
                {
                    return true;
                }

                return false;
            });

            if (rule == null)
            {
                continue;
            }

            EditorUtility.DisplayProgressBar("正在处理", $"正在处理声音文件 {index}/{length}", (float) index / length);
            ChangeSound(path, rule.Setting);
            index++;
        }

        EditorUtility.ClearProgressBar();
    }

    private void ChangeSound(string clipPath, AudioRuleSetting setting)
    {
        AudioImporter audioImporter = (AudioImporter) AudioImporter.GetAtPath(clipPath);
        audioImporter.forceToMono = setting.forceToMono;
        audioImporter.ambisonic = setting.Ambisonic;
        audioImporter.loadInBackground = setting.loadBackground;

        //Android设置
        AudioImporterSampleSettings androidSettings = new AudioImporterSampleSettings();
        androidSettings.loadType = setting.loadType;
        androidSettings.compressionFormat = setting.compression;
        androidSettings.quality = 100;
        androidSettings.sampleRateSetting = setting.sampleRate;
        if (setting.sampleRate == AudioSampleRateSetting.OverrideSampleRate)
        {
            androidSettings.sampleRateOverride = (uint) setting.Rate;
        }

        audioImporter.SetOverrideSampleSettings("Android", androidSettings);

        //iOS设置
        AudioImporterSampleSettings iOSSettings = new AudioImporterSampleSettings();
        iOSSettings.loadType = setting.loadType;
        iOSSettings.compressionFormat = setting.compression;
        iOSSettings.quality = 100;

        iOSSettings.sampleRateSetting = setting.sampleRate;
        if (setting.sampleRate == AudioSampleRateSetting.OverrideSampleRate)
        {
            iOSSettings.sampleRateOverride = (uint) setting.Rate;
        }

        audioImporter.SetOverrideSampleSettings("iOS", iOSSettings);
        audioImporter.SaveAndReimport();
    }

    private bool IsSelfRule(string guid)
    {
        return false;
        // bool isFound = false;
        // foreach (var clip in SelfRules)
        // {
        //     string tmp = clip.GetGUID();
        //     if (tmp == guid)
        //     {
        //         isFound = true;
        //         break;
        //     }
        // }
        //
        // return isFound;
    }
}


[Serializable]
public class AudioRules
{
    public int start;
    public int length;
    public AudioRuleSetting Setting = new AudioRuleSetting();
}

[Serializable]
public class AudioRuleSetting
{
    public bool forceToMono = false;

    public bool loadBackground = false;

    public bool Ambisonic = false;

    public AudioClipLoadType loadType = AudioClipLoadType.Streaming;

    public AudioCompressionFormat compression = AudioCompressionFormat.VAG;


    public AudioSampleRateSetting sampleRate = AudioSampleRateSetting.PreserveSampleRate;

  //  [LabelText("采样率设置"), ShowIf("sampleRate", AudioSampleRateSetting.OverrideSampleRate)]
    public SampleRate Rate = SampleRate.HZ_8000;
}

public enum SampleRate
{
    HZ_8000 = 8000,
    HZ_11025 = 11025,
    HZ_22050 = 22050,
    HZ_44100 = 44100,
    HZ_48000 = 48000,
    HZ_96000 = 96000,
    HZ_192000 = 192000
}