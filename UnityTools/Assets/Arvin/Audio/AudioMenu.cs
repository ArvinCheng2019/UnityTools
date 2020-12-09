using System;
using UnityEditor;
using UnityEngine;

namespace Arvin
{
    public class AudioMenu : Editor
    {
        
        [MenuItem("Arvin/声音 /执行优化")]
        public static void RunAudio()
        {
            var sound = ScriptableHelper.GetSoundOptimization();
            sound.Run();
        }

        [MenuItem("Assets/Arvin/声音/添加自定义列表")]
        public static void AddToList()
        {
            UnrealBar.ShowUnrealBar();
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                if (obj is AudioClip)
                {
                    string path = AssetDatabase.GetAssetPath(obj);
                    var sound = ScriptableHelper.GetSoundOptimization();
                    sound.AddSelfRule(path);
                }
            }
        }

        [MenuItem("Assets/Arvin/声音/从自定义列表移除")]
        public static void DelFromList()
        {
            UnrealBar.ShowUnrealBar();
            var sound = ScriptableHelper.GetSoundOptimization();
            var objs = Selection.objects;
            foreach (var obj in objs)
            {
                if (obj is AudioClip)
                {
                    string path = AssetDatabase.GetAssetPath(obj);
                    sound.RemoveSelfRule(path);
                }
            }
        }
    }
}