using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editors
{
    [CreateAssetMenu(fileName = "editor/Weight During Loading", menuName = "Weight During Loading")]
    [Serializable]
    public class WeightDuringLoading : ScriptableObject
    {
        public float checkAssetStateWeight;
        public float fadeStateWeight;
        public float loadAssetStateWeight;
        public float loadSceneStateWeight;
        public float warmUpStateWeight;
    }
}