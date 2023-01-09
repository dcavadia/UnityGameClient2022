using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class HttpClient : MonoBehaviour
{
    public static async Task<T> Get<T>(string endpoint)
    {
        var getRequest = CreateRequest(endpoint);
        getRequest.SendWebRequest();

        while (!getRequest.isDone) await Task.Delay(10);
        var result = JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
        return result;
    }

    public static async Task<T> Post<T>(string endpoint, object payload)
    {
        var getRequest = CreateRequest(endpoint, RequestType.POST, payload);
        getRequest.SendWebRequest();

        while (!getRequest.isDone) await Task.Delay(10);
        var result = JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
        return result;
    }

    private static UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private static void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }

    public enum RequestType
    {
        GET = 0,
        POST = 1
    }
}
