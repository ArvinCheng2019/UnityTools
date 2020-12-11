using System.Collections.Generic;
using UnityEngine;

namespace Arvin
{
    public class GameObjectOptimizastion : ScriptableObject
    {
        public List<string> ParticleList = new List<string>();

        public void AddToParticleList( string path)
        {
            if ( !ParticleList.Contains(path))
            {
                ParticleList.Add(path);
            }
        }

        public void RemoveParticleList( string path)
        {
            if ( ParticleList.Contains(path))
            {
                ParticleList.Remove(path);
            }
        }

        public string[] GetParticlePaths()
        {
            return ParticleList.ToArray();
        }
    }
}