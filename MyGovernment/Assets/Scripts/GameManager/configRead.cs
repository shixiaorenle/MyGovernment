using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class configRead : Define
{
    private void Awake() {

        StartCoroutine(GetData());
        
    }
    private void Start()
    {
       
       
    }

    IEnumerator GetData()
    {
        var  uri = new  System.Uri(Path.Combine(Application.streamingAssetsPath,"Config.json"));
//        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else
        {
            
            string jsonStr  = www.downloadHandler.text;
           
            _gameManager.configData= jsonStr.hashtableFromJson(); 
            _playerCache._playerData.WebsocketIPAddress  = _gameManager.configData["WebSocketIP"].ToString();
            _playerCache._playerData.HttpIPAddress       = _gameManager.configData["HttpIP"].ToString();

            _playerCache._playerData.cellPhone           = _gameManager.configData["CellPhone"].ToString();
            //开启websocket
            // _gameManager._webSocketManaegr.StartConnect();
            _dataManager.startConnect();
        }
    }
}


