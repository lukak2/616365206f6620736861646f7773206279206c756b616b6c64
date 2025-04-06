using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.Runtime.Gameplay.MagicWords
{
    public static class ImageCache
    {
        // TODO: Consider using a more sophisticated caching mechanism (e.g., LRU cache) and caching as files.
        private static Dictionary<string, Texture2D> TextureChache { get; } = new Dictionary<string, Texture2D>();
        
        /// <summary>
        /// Asynchronously gets a Sprite for the given URL.
        /// Checks the cache first, downloads and creates the Sprite if not found.
        /// </summary>
        /// <param name="url">The URL of the image to load.</param>
        /// <returns>A Task resulting in the Sprite, or null if an error occurred or the URL is invalid.</returns>
        public static async UniTask<Texture2D> GetTexture2DAsync(string url)
        {
            // URL is not sanitized when we receive it, so we need to do it here.
            url = SanitizeUrl(url);
            
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("GetAvatarSpriteAsync: URL cannot be null or empty.");
                return null;
            }

            // Check cache first
            if (TextureChache.TryGetValue(url, out Texture2D cachedSprite))
            {
                return cachedSprite;
            }

            // Not in cache, download it
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                var operation = webRequest.SendWebRequest();
                while (!operation.isDone)
                {
                    await UniTask.Yield(); // Wait for the download to complete
                }

#if UNITY_2020_2_OR_NEWER
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
#else
                if (webRequest.isNetworkError || webRequest.isHttpError)
#endif
                {
                    Debug.LogError($"Error downloading image from {url}: {webRequest.error}");
                    return null;
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                    if (texture == null)
                    {
                        Debug.LogError($"Failed to load texture from {url}");
                        return null;
                    }

                    return texture;
                }
            }
            
        }
        
        public static Texture2D PackTexturesIntoAtlas(Texture2D[] sourceTextures, out Rect[] uvs, int padding = 2)
        {
            // Ensure all textures are readable
            for (int i = 0; i < sourceTextures.Length; i++)
            {
                if (!sourceTextures[i].isReadable)
                    Debug.LogWarning($"Texture {i} is not readable!");
            }

            // Create the atlas
            Texture2D atlas = new Texture2D(2048, 2048, TextureFormat.RGBA32, false);
            atlas.name = "EmojiAtlas";
            uvs = atlas.PackTextures(sourceTextures, padding, 2048);

            atlas.Apply();
            return atlas;
        }
        
        public static Sprite ToSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        
        private static string SanitizeUrl(string url)
        {
            // Use regex to remove any port specification (colon followed by numbers before a slash or end of string)
            string sanitized = Regex.Replace(url, ":\\d+(?=/|$)", "");
        
            // If the URL was modified, log the change
            if (url != sanitized)
            {
                Debug.Log($"Sanitized URL from {url} to {sanitized}");
            }
        
            return sanitized;
        }
    }
}