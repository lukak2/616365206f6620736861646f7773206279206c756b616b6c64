using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts.Runtime.Gameplay.MagicWords
{
    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private RectTransform leftAvatarContainer;
        [SerializeField] private RectTransform rightAvatarContainer;
        
        [SerializeField] private AvatarView avatarPrefab;
        
        
        [SerializeField] private TMP_Text dialogueText;

        private Dictionary<string, AvatarView> _avatarLookup = new Dictionary<string, AvatarView>();
        private Dictionary<string, Sprite> _emojiLookup = new Dictionary<string, Sprite>();
        private List<DialogueLine> _dialogueLines = new List<DialogueLine>();
        
        private int _currentLineIndex = 0;

        public void Initialize(Dictionary<string, Sprite> emojiLookup, List<DialogueLine> dialogueLines)
        {
            _emojiLookup = emojiLookup;
            _dialogueLines = dialogueLines;
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
            
            dialogueText.SetText(line.DialogueText);
        }
    }
}