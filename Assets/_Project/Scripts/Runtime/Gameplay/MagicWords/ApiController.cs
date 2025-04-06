using System;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace Scripts.Runtime.Gameplay.MagicWords
{
    public class ApiController : MonoBehaviour
    {
        [SerializeField] private string endpointUrl;
        [SerializeField] private string sampleData;

        public async UniTask<Response> FetchSample()
        {
            Response data = JsonConvert.DeserializeObject<Response>(sampleData);

            return data;
        }
        
        public async UniTask<Response> FetchEndpoint()
        {
            var url = endpointUrl;
            
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Send the request and wait for the response asynchronously
                var operation = webRequest.SendWebRequest();
                while (!operation.isDone)
                {
                    await UniTask.Yield();
                }

                // Check for errors
#if UNITY_2020_2_OR_NEWER
                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
#else
                    if (webRequest.isNetworkError || webRequest.isHttpError)
#endif
                {
                    Debug.LogError($"Error fetching data from {url}: {webRequest.error}");
                    return null;
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    
                    try
                    {
                        Response data = JsonConvert.DeserializeObject<Response>(jsonResponse);
                        Debug.Log($"Successfully fetched and deserialized data from {url}");
                        return data;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error deserializing JSON from {url}: {e.Message}\nJSON: {jsonResponse}");
                        return null;
                    }
                }
            }
        }
    }
}