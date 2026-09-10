using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editors
{
    [System.Serializable]
    public class Dialogue
    {
        [SerializeField] public int choice;
        [SerializeField] public DialogueSo _nextDialogue;
    }

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
    public class DialogueSo : ScriptableObject
    {
        public int index;
        public string _speakName;
        public Sprite _sprite;//图片
        public Dialogue[] _dialogues;
        public string content;
        public bool IsOption => _dialogues.Length >1;
    }
}