using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Runtime.Gameplay.MagicWords
{
    public class DialogueController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform leftAvatarContainer;
        [SerializeField] private RectTransform rightAvatarContainer;

        [SerializeField] private Button nextButton;
        [SerializeField] private TMP_Text dialogueText;
        
        [Header("Properties")]
        [SerializeField] private AvatarView avatarPrefab;

        private Dictionary<string, AvatarView> _avatarLookup = new Dictionary<string, AvatarView>();
        private List<DialogueLine> _dialogueLines = new List<DialogueLine>();
        
        private int _currentLineIndex = 0;

        public void Initialize(List<DialogueLine> dialogueLines)
        {
            _dialogueLines = dialogueLines;
        }

        private void OnEnable()
        {
            nextButton.onClick.AddListener(NextDialogueLine);
        }

        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(NextDialogueLine);
        }

        public void SetSpriteAsset(TMP_SpriteAsset spriteAsset)
        {
            dialogueText.spriteAsset = spriteAsset;
        }
        
        public void AddAvatar(Sprite sprite, string avatarName, string position)
        {
            AvatarView avatarView = Instantiate(avatarPrefab, position == "left" ? leftAvatarContainer : rightAvatarContainer);
            avatarView.Initialize(sprite, avatarName);
            
            _avatarLookup.Add(avatarName, avatarView);
        }

        public void NextDialogueLine()
        {
            if (_currentLineIndex >= _dialogueLines.Count)
            {
                Debug.Log("No more dialogue lines.");
                nextButton.gameObject.SetActive(false);
                return;
            }
            
            DisplayDialogueLine(_dialogueLines[_currentLineIndex]);
            
            _currentLineIndex++;
        }
        
        private void DisplayDialogueLine(DialogueLine line)
        {
            // Simplest way to hide previous avatar
            foreach (var avatar in _avatarLookup.Values)
            {
                avatar.Hide();
            }
            
            if (_avatarLookup.TryGetValue(line.AvatarName, out AvatarView avatarView))
            {
                avatarView.Show();
            }
            
            // Replace emoji placeholders with sprite tags
            var emojiText = Regex.Replace(line.DialogueText, @"\{([^\{\}]+)\}", "<sprite name=\"$1\">");
            
            dialogueText.SetText(emojiText);
        }
    }
}