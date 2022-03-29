using RenderHeads.Media.AVProVideo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MaxVideo : UIAlertLayer
{
    public MediaPlayer media;
    public Image circle;
    public Image connect;
    private string cameraIndexCode;
    public RectTransform video;
    public RectTransform me;
    public Toggle up;
    public Toggle down;
    public Toggle left;
    public Toggle right;
   protected override void Start()
    {
        base.Start();
        RegistNotification(this, kNotificationKeys.getCameradata, GetCameradata);
        RegistNotification(this, kNotificationKeys.getCameraURL, GetCameraURL);
        up.onValueChanged.AddListener(delegate(bool ison) { toggleChange(ison, "UP"); } );
        down.onValueChanged.AddListener(delegate (bool ison) { toggleChange(ison, "DOWN"); });
        left.onValueChanged.AddListener(delegate (bool ison) { toggleChange(ison, "LEFT"); });
        right.onValueChanged.AddListener(delegate (bool ison) { toggleChange(ison, "RIGHT"); });
         video.sizeDelta = new Vector2( me.rect.height, me.rect.width);
        //Screen.orientation = ScreenOrientation.Landscape;
        
    }
    //private void SetScreenOrientation(bool isTrue)
    //{
    //    Screen.autorotateToLandscapeLeft = isTrue;
    //    Screen.autorotateToLandscapeRight = isTrue;
    //}
    private void toggleChange(bool ison,string command)
    {
        //if (cameraIndexCode == "" || cameraIndexCode == null)
        //{
        //    return;
        //}
        if (ison)
        {
            controlCamera(command);
        }
        else
        {
            stopCamera(command);
        }
        Debug.Log(command + ison);
    }

    private void GetCameraURL(Hashtable obj)
    {
        Debug.Log(obj.toJson());
        if (obj.ContainsKey("data"))
        {
            Hashtable hash = obj["data"] as Hashtable;
            if (hash.ContainsKey("rtspUrl"))
            {
                string url = hash["rtspUrl"] as string;
                Debug.Log(url);
                //OnClickVideoBtn(url);
               Play(url);
            }
        }

    }
    public void OnClickVideoBtn(string url)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("videoPlay", url);
    }
    private void GetCameradata(Hashtable obj)
    {
       // Debug.Log(obj.toJson());
        if (obj.ContainsKey("data"))
        {
            ArrayList array=obj["data"]as ArrayList;
            foreach (var item in array)
            {
                Hashtable h = item as Hashtable;
                if (h.ContainsKey("cameraIndexCode"))
                {
                    string str = h["cameraIndexCode"] as string;
                    cameraIndexCode = str;
                    Hashtable hash = new Hashtable();
                    hash["cameraIndexCode"] = str;
                    hash["flag"] = 0;
                    Debug.Log(hash.toJson());
                    _dataManager.GetCameraHLS(hash);
                }
            }

        }
        
    }

   protected override void Update()
    {

        if (media.Info.IsPlaybackStalled())
        {
            circle.enabled = true;
        }
        else
        {
            circle.enabled = false;
        }
    }
    public  void Close()
    {
        
        onClose();
    }
    public override void onClose()
    {
        stopCamera("LEFT");
        stopCamera("RIGHT");
        stopCamera("UP");
        stopCamera("DOWN");
       // Screen.orientation = ScreenOrientation.Portrait;
        base.onClose();
    }
    public override void RefreshData(Hashtable hashtable)
    {
        OpenCamera();
    }
    public void OpenCamera()
    {
        //gameObject.SetActive(true);
        up.isOn = false;
        down.isOn = false;
        left.isOn = false;
        right.isOn = false;
        Hashtable hash = new Hashtable();
        hash["archSerialNum"] = _playerCache._playerData.myBoxNum;
        _dataManager.GetMyCameraData(hash);
    }
    public void controlCamera(string command)
    {
        if (cameraIndexCode==""|| cameraIndexCode==null)
        {
            return;
        }
        Hashtable hash = new Hashtable();
        hash["cameraIndexCode"] = cameraIndexCode;
        hash["command"] = command;
        hash["action"] = 0.ToString();
        _dataManager.ControlCamera(hash);
        //Debug.Log(hash.toJson());
    }
    public void stopCamera(string command)
    {
        if (cameraIndexCode == "" || cameraIndexCode == null)
        {
            return;
        }
        Hashtable hash = new Hashtable();
        hash["cameraIndexCode"] = cameraIndexCode;
        hash["command"] = command;
        hash["action"] = 1.ToString();
        _dataManager.ControlCamera(hash);
        //Debug.Log(hash.toJson());
    }
    private void Play(string url)
    {
        media.OpenMedia(MediaPathType.AbsolutePathOrURL, url, true);
        
        //connect.gameObject.SetActive(true);
        //vp_Timer.In(5, delegate
        //{
        //    connect.gameObject.SetActive(false);
        //});
        
        
        
    }
    private void OnDisable()
    {
        media.Stop();
    }
}
