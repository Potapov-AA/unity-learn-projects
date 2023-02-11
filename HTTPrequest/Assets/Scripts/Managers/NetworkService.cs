using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetworkService
{
    private const string xmlApi = "https://api.openweathermap.org/data/2.5/weather?q=Chicago,us&mode=xml&APPID=8e43960578173476a6de9d8f68cd904d";
    private const string jsonAPI = "https://api.openweathermap.org/data/2.5/weather?q=Chicago,us&APPID=8e43960578173476a6de9d8f68cd904d";

    private const string webImage = "https://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    private IEnumerator CallAPI(string url, Action<string> callback){
        using(UnityWebRequest request = UnityWebRequest.Get(url)){

            yield return request.Send();

            if(request.isNetworkError){
                Debug.LogError("network problem: " + request.error);
            } else if (request.responseCode != (long)System.Net.HttpStatusCode.OK) {
                Debug.LogError("response error: " + request.responseCode);
            } else {
                callback(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetWeatherXML(Action<string> callback){
        return CallAPI(xmlApi, callback);
    }

    public IEnumerator GetWeatherJSON(Action<string> callback){
        return CallAPI(jsonAPI, callback);
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback) {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
        yield return request.Send();
        callback(DownloadHandlerTexture.GetContent(request));
    }
}
