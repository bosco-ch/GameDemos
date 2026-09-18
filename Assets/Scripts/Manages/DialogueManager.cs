using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Editors;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Manages
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        private DialoguePanel _dialoguePanel;
        public TextMeshProUGUI speakName;
        public TextMeshProUGUI content;
        public GameObject choiceButton;
        public DialogueSo currentDialogueSo;
        private bool _isTyping;
        private Coroutine _typingCoroutine;

        public GameObject choiceParent
        {
            get => _currentChoice.Task.Result;
        } //选择框预制体

        public GameObject choicesChild;
        [SerializeField] private float typingSpeed;
        public bool ChoiceParentIsLoad => choiceParent != null;
        public bool ChoiceChildIsLoad => choicesChild != null;

        private readonly List<DialogueSo> _dialogueList = new();//对话数据

        //预加载
        AsyncOperationHandle<GameObject> _currentChoice
            = Addressables.LoadAssetAsync<GameObject>("ChoiceGroup");

        protected override void Awake()
        {
            base.Awake();
            _ = LoadDialogueData();
            _ = LoadChoices();
            // var test = Addressables.LoadAssetAsync<GameObject>("");//以后还是在加载页面的时候就弄完 免得到时候要用的时候还没有加载完
        }

        //加载选择框
        async Task LoadChoices()
        {
            for (int i = 0; i < currentDialogueSo._dialogues.Length; i++)
            {
                choicesChild = await Addressables.LoadAssetAsync<GameObject>("Choice").Task;
                if (choicesChild != null)
                {
                    choicesChild.name = $"choicesChild{i}";
                    Instantiate(choicesChild, choicesChild.transform);
                }
            }
        }

        /// <summary>
        /// 加载对话数据
        /// </summary>
        public Task LoadDialogueData()
        {
            //加载全部对话
            Addressables.LoadAssetsAsync<DialogueSo>("AllDialogue", null)
                .Completed += op =>
            {
                if (op.Result != null)
                {
                    _dialogueList.AddRange(op.Result);
                    StartDialogue(_dialogueList.Find(a => a.index == 1));
                }
                else
                {
                    Debug.LogError("未找到对应的值");
                }
            };
            return Task.CompletedTask;
        }

        //开启对话
        public void StartDialogue(DialogueSo node)
        {
            choiceParent.gameObject.SetActive(false);
            currentDialogueSo = node;
            ShowDialogueContent();
            //先生成选择框预制体，设置为visable
        }

        private async void ShowDialogueContent()
        {
            if (currentDialogueSo == null)
            {
                EndDialogue();
            }

            ClearAllChoice();
            speakName.text = currentDialogueSo._speakName;
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);
            StartCoroutine(currentDialogueSo.content);
            if (currentDialogueSo._dialogues.Length > 0 && choiceParent != null)
            {
                choiceParent.SetActive(true);
                int index = 0;
                foreach (var dialogue in currentDialogueSo._dialogues)
                {
                    var choiceTask = Addressables.LoadAssetAsync<GameObject>("Choice");
                    await choiceTask.Task;
                    choicesChild = choiceTask.Result;
                    choicesChild.gameObject.name = $"choice_{index}";
                    // choicesChild.GetComponentInChildren<Text>().text =
                    //     currentDialogueSo._dialogues[index].choice。ToString();
                }
            }
        }

        private void ClearAllChoice()
        {
            for (int i = 0; i < choiceParent.transform.childCount; i++)
                Destroy(choiceParent.transform.GetChild(i).gameObject);
            choiceParent.SetActive(false);
        }

        private void EndDialogue()
        {
            throw new System.NotImplementedException();
        }

        //实现打字效果
        IEnumerable TypingText(string text)
        {
            _isTyping = true;
            foreach (var c in text)
            {
                content.text += c;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            _isTyping = false;
            yield return null;
        }
    }
}