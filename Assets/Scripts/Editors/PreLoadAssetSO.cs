using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editors
{
    [CreateAssetMenu(menuName = "editor/PreLoadAsset")]
    public class PreLoadAssetSo : ScriptableObject
    {
        public List<string> keys;
        public List<string> labels;
    }
}   