using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.Runtime.Gameplay.MagicWords
{
    public static class ImageCache
    {
        // TODO: Consider using a more sophisticated caching mechanism (e.g., LRU cache) and caching as files.
        public static Dictionary<string, Sprite> AvatarSprites { get; } = new Dictionary<string, Sprite>();
        
        /// <summary>
        /// Asynchronously gets a Sprite for the given URL.
        /// Checks the cache first, downloads and creates the Sprite if not found.
        /// </summary>
        /// <param name="url">The URL of the image to load.</param>
        /// <returns>A Task resulting in the Sprite, or null if an error occurred or the URL is invalid.</returns>
        public static async UniTask<Sprite> GetAvatarSpriteAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("GetAvatarSpriteAsync: URL cannot be null or empty.");
                return null;
            }

            // Check cache first
            if (AvatarSprites.TryGetValue(url, out Sprite cachedSprite))
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

                    Sprite newSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    if (newSprite != null)
                    {
                        // Add to cache
                        AvatarSprites[url] = newSprite;
                        return newSprite;
                    }
                    else
                    {
                        Debug.LogError($"Failed to create sprite from texture downloaded from {url}");
                        // Clean up the created texture if sprite creation failed
                        Object.Destroy(texture);
                        return null;
                    }
                }
            }
        }
    }
}