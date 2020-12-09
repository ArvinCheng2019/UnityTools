using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundOptimization : ScriptableObject
{
    public List<string> SelfRules = new List<string>();
    public List<AudioRules> SoundRules = new List<AudioRules>();
    
    public void AddSelfRule(string path)
    {
        if (SelfRules.Contains(path))
        {
            return;
        }

        SelfRules.Add(path);
    }

    public void RemoveSelfRule(string path)
    {
        if (!SelfRules.Contains(path))
        {
            return;
        }

        SelfRules.Remove(path);
    }

    public void CreateDefaultRule()
    {
        SoundRules.Add( new AudioRules()
        {
            length = 5,
            start =  0,
            Setting =  new AudioRuleSetting()
            {
                forceToMono = true,
                Ambisonic =  false,
                loadBackground =  false,
                loadType = AudioClipLoadType.DecompressOnLoad,
                compression = AudioCompressionFormat.ADPCM,
                sampleRate = AudioSampleRateSetting.PreserveSampleRate,
                Rate = SampleRate.HZ_11025
            },
        });
        
        SoundRules.Add( new AudioRules()
        {
            length = 100000,
            start =  5,
            Setting =  new AudioRuleSetting()
            {
                forceToMono = true,
                Ambisonic =  false,
                loadBackground =  false,
                loadType = AudioClipLoadType.Streaming,
                compression = AudioCompressionFormat.Vorbis,
                sampleRate = AudioSampleRateSetting.OverrideSampleRate,
                Rate = SampleRate.HZ_8000
            },
        });
    }
    public void Run()
    {
        if (SoundRules.Count == 0)
        {
            CreateDefaultRule();
        }
        
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
        AudioImporter audioImporter = (AudioImporter) AssetImporter.GetAtPath(clipPath);
        audioImporter.forceToMono = setting.forceToMono;
        if (setting.forceToMono )
        {
            var serializedObject = new SerializedObject(audioImporter);
            var normalize = serializedObject.FindProperty("m_Normalize");
            normalize.boolValue = true;
            serializedObject.ApplyModifiedProperties();
        }
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
        string path = AssetDatabase.GUIDToAssetPath(guid);
        return SelfRules.Contains(path);
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