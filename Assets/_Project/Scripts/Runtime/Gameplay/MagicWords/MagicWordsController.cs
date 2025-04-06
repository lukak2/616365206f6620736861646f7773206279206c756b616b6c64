using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace Scripts.Runtime.Gameplay.MagicWords
{
    public class MagicWordsController : MonoBehaviour
    {
        [SerializeField] private ApiController apiController;
        [SerializeField] private DialogueController dialogueController;

        [SerializeField] private Material emojiMaterial;

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
                var avatarTexture = await ImageCache.GetTexture2DAsync(avatar.url);
                Sprite avatarSprite = ImageCache.ToSprite(avatarTexture);
                
                dialogueController.AddAvatar(avatarSprite, avatar.name, avatar.position);
            }
            
            var emojiTextures = new Dictionary<string, Texture2D>();
            
            foreach (var emoji in response.emojies)
            {
                var emojiTexture2D = await ImageCache.GetTexture2DAsync(emoji.url);
                
                emojiTextures.Add(emoji.name, emojiTexture2D);
            }
            
            var packedAtlas = ImageCache.PackTexturesIntoAtlas(emojiTextures.Values.ToArray(), out var uvRects);

            List<DialogueLine> dialogueLines = response.dialogue
                .Select(d => new DialogueLine(d.name, d.text)).ToList();
            
            dialogueController.SetSpriteAsset(CreateSpriteAssetFromAtlas(emojiTextures, packedAtlas, uvRects));
            dialogueController.Initialize(dialogueLines);
            
            dialogueController.NextDialogueLine();
        }
        
        public TMP_SpriteAsset CreateSpriteAssetFromAtlas(Dictionary<string, Texture2D> emojiLookup, Texture2D atlas, Rect[] uvRects)
        {
            TMP_SpriteAsset spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
            spriteAsset.name = "RuntimeEmojiSpriteAsset";
            spriteAsset.spriteSheet = atlas;
            
            var material = Instantiate(emojiMaterial);
            material.mainTexture = atlas;
            spriteAsset.material = material;

            spriteAsset.spriteInfoList = new();
            
            uint unicodeStart = 0xE000; // Private Use Unicode block

            var characters = new List<TMP_SpriteCharacter>();
            
            for (int i = 0; i < uvRects.Length; i++)
            {
                uint index = (uint)i;
                Rect uv = uvRects[i];

                // Convert UVs to pixel rects
                Rect pixelRect = new Rect(
                    uv.x * atlas.width,
                    uv.y * atlas.height,
                    uv.width * atlas.width,
                    uv.height * atlas.height
                );

                // Create Sprite manually
                Sprite sprite = Sprite.Create(atlas, pixelRect, new Vector2(0.5f, 0.5f));
                sprite.name = emojiLookup.ElementAt(i).Key;

                // Create Character
                var character = new TMP_SpriteCharacter
                {
                    name = sprite.name,
                    glyphIndex = index,
                    scale = 1f,
                    unicode = unicodeStart + index
                };
                
                spriteAsset.spriteInfoList.Add(new TMP_Sprite()
                {
                    name = sprite.name,
                    unicode = 65534,
                    scale = 1.0f,
                    x = pixelRect.x,
                    y = pixelRect.y,
                    width = pixelRect.width,
                    height = pixelRect.height,
                    xAdvance = pixelRect.width,
                });

                characters.Add(character);
            }
            
            spriteAsset.spriteCharacterTable.Clear();
            
            foreach (var character in characters)
            {
                spriteAsset.spriteCharacterTable.Add(character);
            }
            
            foreach (var glyph in spriteAsset.spriteGlyphTable)
            {
                var metrics = glyph.metrics;
                metrics.horizontalBearingY = 115.6f;
                glyph.metrics = metrics;
            }

            spriteAsset.UpdateLookupTables();

            return spriteAsset;
        }
    }
}
