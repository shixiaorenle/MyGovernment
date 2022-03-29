using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;
using System;
using System.Text.RegularExpressions;

public class webSocketManager : Define
{


    #region Private Fields

    /// <summary>
    /// The WebSocket address to connect
    /// </summary>
 // string address = "ws://192.168.111.144:2222/chat1";//
   //string address = "ws://10.25.125.18:2222/v1.0/chat1";//tashan
    string address;//tashan
    /// <summary>
    /// Default text to send
    /// </summary>
    string msgToSend = "Hello World!";

    /// <summary>
    /// Saved WebSocket instance
    /// </summary>
    WebSocket webSocket;

    /// <summary>
    /// GUI scroll position
    /// </summary>
    Vector2 scrollPos;

    #endregion

  
    void OnDestroy()
    {
        if (webSocket != null)
            webSocket.Close();
    }


    public void StartConnect()
    {
       address = _playerCache._playerData.WebsocketIPAddress;



           if (webSocket == null)
                {
                  
                    // Create the WebSocket instance
                    webSocket = new WebSocket(new Uri(address));

#if !UNITY_WEBGL
                    webSocket.StartPingThread = true;

#if !BESTHTTP_DISABLE_PROXY
                    if (HTTPManager.Proxy != null)
                        webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
#endif
#endif

                    // Subscribe to the WS events
                    webSocket.OnOpen += OnOpen;
                    webSocket.OnMessage += OnMessageReceived;
                    webSocket.OnClosed += OnClosed;
                    webSocket.OnError += OnError;

                    // Start connecting to the server
                    webSocket.Open();
                 
                }

                // if (webSocket != null && webSocket.IsOpen)
                // {
                   
                //        Text += "Sending message...\n";

                //             // Send message to the server
                //         //webSocket.Send(msgToSend);
              
                //         // Close the connection
                //       //  webSocket.Close(1000, "Bye!");
                    
                // }
    }



    public void sendMessage(string data)
    {
        webSocket.Send(data);
    }

    private void Awake() {
        if(_gameManager._webSocketManaegr == null)
        {
            _gameManager._webSocketManaegr = this;

       //     Debug.Log("赋值websocket");  

        }
     
    }
 
#region WebSocket Event Handlers

    /// <summary>
    /// Called when the web socket is open, and we are ready to send and receive data
    /// </summary>
    void OnOpen(WebSocket ws)
    {
       
        Debug.Log("socket连接成功");
        _gameManager.isScocketOnline = true;   

    }

    /// <summary>
    /// Called when we received a text message from the server
    /// </summary>
    void OnMessageReceived(WebSocket ws, string message)
    {
        Hashtable hash = new Hashtable();
        hash = message.hashtableFromJson();
        foreach(var item in hash.Keys)
        {

            
            //cameraData currentData;//摄像头消息
            //再此放入特殊情况的
            if(item.ToString().Equals("workface"))

            {
               

            }

            else
            {
             

            }

        }
     
        PostNotification(kNotificationKeys.webSocketCameraData);

      // Debug.Log("socket获取数据" + hash.toJson());
    }

    /// <summary>
    /// Called when the web socket closed
    /// </summary>
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
       
        webSocket = null;
        _gameManager.isScocketOnline = false;
    }

    /// <summary>
    /// Called when an error occured on client side
    /// </summary>
//    void OnError(WebSocket ws, Exception ex)
//    {
//        string errorMsg = string.Empty;
//#if !UNITY_WEBGL || UNITY_EDITOR
//        if (ws.InternalRequest.Response != null)
//            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
//#endif

//        Debug.Log("webSocket连接失败");

//        webSocket = null;
//        _gameManager.isScocketOnline = false;

//        重连
//        vp_Timer.In(10f, () =>
//        {
//            Debug.Log("重新连接");
//            StartConnect();
//        });
//    }
    private void OnError(WebSocket ws, string reason)
    {
        string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws.InternalRequest.Response != null)
            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif

        //   Debug.Log("webSocket连接失败");

        webSocket = null;
        _gameManager.isScocketOnline = false;

        //重连 
        vp_Timer.In(10f, () =>
        {
            Debug.Log("重新连接");
            StartConnect();
        });
    }
    #endregion


    public bool IsNumber(string strInput){
 
       Regex reg = new Regex("^[0-9]*$");
       if (reg.IsMatch (strInput)) {
           return true;
       } else {
           return false;
       }
   }
    private void Start()
    {
        
    }
}
