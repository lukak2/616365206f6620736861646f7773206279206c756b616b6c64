using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts.Runtime.Gameplay.MagicWords
{
    public class MagicWordsController : MonoBehaviour
    {
        [SerializeField] private ApiController apiController;
        [SerializeField] private DialogueController dialogueController;

        [SerializeField] private bool useSampleData;
        
        private void Start()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            Response response = await (useSampleData ? apiController.FetchSample() : apiController.FetchEndpoint());

            foreach (var avatar in response.avatars)
            {
                Sprite avatarSprite = await ImageCache.GetAvatarSpriteAsync(avatar.url);
                
                dialogueController.AddAvatar(avatarSprite, avatar.name, avatar.position);
            }
            
            Dictionary<string, Sprite> emojiLookup = new Dictionary<string, Sprite>();
            
            foreach (var emoji in response.emojies)
            {
                Sprite emojiSprite = await ImageCache.GetAvatarSpriteAsync(emoji.url);
                
                emojiLookup.Add(emoji.name, emojiSprite);
            }

            List<DialogueLine> dialogueLines = response.dialogue
                .Select(d => new DialogueLine(d.name, d.text)).ToList();
            
            // I admit {satisfied} the design of Cookie Crush is quite elegant in its simplicity
            
            dialogueController.Initialize(emojiLookup, dialogueLines);
        }
    }
}
