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

        private List<AvatarView> _avatars = new List<AvatarView>();
        private Dictionary<string, Sprite> _emojiLookup = new Dictionary<string, Sprite>();
        private List<DialogueLine> _dialogueLines = new List<DialogueLine>();

        public void Initialize(Dictionary<string, Sprite> emojiLookup, List<DialogueLine> dialogueLines)
        {
            _emojiLookup = emojiLookup;
            _dialogueLines = dialogueLines;
        }
        
        public void AddAvatar(Sprite sprite, string avatarName, string position)
        {
            AvatarView avatarView = Instantiate(avatarPrefab, position == "left" ? leftAvatarContainer : rightAvatarContainer);
            avatarView.Initialize(sprite, avatarName);
            
            _avatars.Add(avatarView);
        }

        public void NextDialogueLine()
        {
            
        }
    }
}