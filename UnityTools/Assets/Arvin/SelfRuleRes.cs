using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Arvin
{
    // public enum SelfRuleResType
    // {
    //     None,
    //     Texture,
    //     AudioClip,
    //     Prefab,
    //     Model
    // }
    [CreateAssetMenu]
    public class SelfRuleRes : ScriptableObject
    {
        // 除非是装了Odin，否则不能在Inspector 里看到字典的数据，为了方便查看，所以改成list
        //public Dictionary<SelfRuleResType,List<string>> useSelfRulesList = new Dictionary<SelfRuleResType, List<string>>();
        public List<string> usedSelfRules_Texture = new List<string>();
        public List<string> usedSelfRules_AudioClip = new List<string>();
        public List<string> usedSelfRules_Prefab = new List<string>();
        //public List<string> usedSelfRules_Directory = new List<string>();

        public void AddToSelfRule(string path, object res)
        {
            if (res is Texture || res is Texture2D)
            {
                if (!usedSelfRules_Texture.Contains(path))
                {
                    usedSelfRules_Texture.Add(path);
                }
            }
            else if (res is GameObject)
            {
                if (!usedSelfRules_Prefab.Contains(path))
                {
                    usedSelfRules_Prefab.Add(path);
                }
            }
            else if (res is AudioClip)
            {
                if (!usedSelfRules_AudioClip.Contains(path))
                {
                    usedSelfRules_AudioClip.Add(path);
                }
            }
        }

        public void RemoveSelfRule(string path, object res)
        {
            if (res is Texture || res is Texture2D)
            {
                if (usedSelfRules_Texture.Contains(path))
                {
                    usedSelfRules_Texture.Remove(path);
                }
            }
            else if (res is GameObject)
            {
                if (usedSelfRules_Prefab.Contains(path))
                {
                    usedSelfRules_Prefab.Remove(path);
                }
            }
            else if (res is AudioClip)
            {
                if (usedSelfRules_AudioClip.Contains(path))
                {
                    usedSelfRules_AudioClip.Remove(path);
                }
            }
        }

        public bool IsResInSelfRule( string path)
        {
            if ( usedSelfRules_Prefab.Contains(path))
            {
                return true;
            }

            if ( usedSelfRules_Texture.Contains( path))
            {
                return true;
            }

            if ( usedSelfRules_AudioClip.Contains( path))
            {
                return true;
            }
            return false;
        }
    }
}