using Editors;
using Manages;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DialogueChoice : MonoBehaviour, IPointerDownHandler
    {
        private DialogueSo _data;

        public DialogueChoice(DialogueSo data)
        {
            this._data = data;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            DialogueManager.Instance.StartDialogue(_data);
        }
    }
}