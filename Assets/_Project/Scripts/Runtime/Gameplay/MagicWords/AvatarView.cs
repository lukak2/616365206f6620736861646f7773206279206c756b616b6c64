using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Runtime.Gameplay.MagicWords
{
    public class AvatarView : MonoBehaviour
    {
        [SerializeField] private Image avatarImage;
        [SerializeField] private TMP_Text nameText;

        public void Initialize(Sprite avatarSprite, string avatarName)
        {
            avatarImage.sprite = avatarSprite;
            nameText.SetText(avatarName);
        }
    }
}