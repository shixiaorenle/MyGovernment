// modification by bob 2017.9.26
using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Threading;
using UnityEngine.UI;
using System.Linq;
//using Facebook.Unity;
//using BestHTTP;
//using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(PlayerCore))]

public class GameManager : Define
{
	
	public bool deletePlayerPrefs 									= false;
    public bool debugMode = false;



    public static GameManager instance 								= null;
	public PlayerCore playerCore 									= null;
	public MaskLayer maskLayer;
	public DataManager dataManager                                  = null;
	public webSocketManager _webSocketManaegr 						= null;
	public ModManager _modManager								    = null;	
	public UI2DManager _UI2Dmanager                                 = null;
    public PCUIManager _PCUIManager = null;
    //public AudioMixer AudioMixer;
    //public AudioMixer BGMMixer;//背景声音
    public AudioMixer alarmVoice;//报警声音
	public bool alarmSound;//是否有报警声音
	public Hashtable configData = new Hashtable();

	[HideInInspector]
	public bool isScocketOnline = false;//socket是否在线

    void Awake ()
	{
		
		playerCore = GetComponent<PlayerCore> ();
		
		DontDestroyOnLoad (gameObject);

		if (instance == null)
		{
			instance = this;
			this.name = "GameManager";
			Debug.Log ("GameManager init");
			return;

		} else if (instance != this)
		{
			Destroy (gameObject);   
		}
		
		
		
		
	}


	void initGameManager()
	{
		if (!_isScreenConfigInited) 
		{
			_realScreenSize = new Vector2 (Screen.width, Screen.height);
            Debug.Log(_realScreenSize);
			_realScreenSizeCenter = _realScreenSize / 2;
			float screenSizeRatio =  _screenSize.x /_screenSize.y  ;
			float realScreenSizeRatio =  _realScreenSize.x /_realScreenSize.y;
			float scaledScreenSizeWidth = 0.0f;
			float scaledScreenSizeHeight = 0.0f;
			if (realScreenSizeRatio > screenSizeRatio) 
			{
				scaledScreenSizeHeight = _screenSize.y;
				_scaleFactor = _screenSize.y / _realScreenSize.y;
				_scaleFactorY = _scaleFactor;
				_scaleFactorX = _screenSize.x / _realScreenSize.x;
				scaledScreenSizeWidth = _realScreenSize.x * _scaleFactor;
			} else
			{
				scaledScreenSizeWidth = _screenSize.x;
				_scaleFactor = _screenSize.x / _realScreenSize.x;
				_scaleFactorX = _scaleFactor;
				_scaleFactorY = _screenSize.y / _realScreenSize.y;
				scaledScreenSizeHeight = _realScreenSize.y * _scaleFactor;
			}
			_scaledScreenSize = new Vector2 (scaledScreenSizeWidth, scaledScreenSizeHeight);
//						Screen.SetResolution((int)_scaledScreenSize.x,(int)_scaledScreenSize.y,false);
			_isScreenConfigInited = true;
		}
	}

	void Start()
	{
		initGameManager();
        StartCoroutine(StartGPS());
        
        //	debugMode = true;//暂时开启debug模式
        //_gameManager.BGMMixer.SetFloat("BGM",-80 +_playerCache._Data.backAudioPercent *100 );
        //if (!_playerCache._Data.isWarningAudio)
        //	_gameManager.alarmVoice.SetFloat("alarmVoice", -80f);
        //else
        //{
        //	float ss = Mathf.Log10(_playerCache._Data.warningAudioPercent) * 20f;
        //	_gameManager.alarmVoice.SetFloat("alarmVoice", ss);
        //}


    }
    public IEnumerator StartGPS()
    {
        // Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置    
        // LocationService.isEnabledByUser 用户设置里的定位服务是否启用  
        if (!Input.location.isEnabledByUser)
        {
            //_maskLayer.ShowAlertWithTips("请打开GPS");
            yield return false;
        }
        // LocationService.Start();// 启动位置服务的更新,最后一个位置坐标会被使用    
        Input.location.Start(10.0f, 10.0f);
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            // 暂停协同程序的执行(1秒)    
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1)
        {
            _maskLayer.ShowAlertWithTips("GPS初始化超时");
            yield return false;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            _maskLayer.ShowAlertWithTips("GPS初始化失败");
            yield return false;
        }
        else
        {
            //GetGps = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
            Debug.Log("Keyidingwei");
            yield return new WaitForSeconds(100);
        }
    }
    private void Update() 
	{

        //if (Input.GetKeyDown(KeyCode.Escape))//返回按钮
        //{
        //    //kAlertTypes nowType = _playerCache._playerData.alertTypes[_playerCache._playerData.alertTypes.Count - 1];
        //    if (_maskLayer.Windows.Count<1)
        //    {
        //        return;
        //    }
           
        //    GameObject o = _maskLayer.Windows[_maskLayer.Windows.Count - 1];
        //    if (o.name.Contains("LoadingAlert"))
        //    {
        //        return;
        //    }
        //    else if (o.name.Contains("UITIps")|| o.name.Contains("UIMSG"))
        //    {
        //        return;
        //    }
        //    else if(o.name.Contains("Center"))
        //    {
        //        return;
        //    }
        //    else if(o.name.Contains("WarningList(Clone)")|| o.name.Contains("record(Clone)")|| o.name.Contains("Control(Clone)")|| o.name.Contains("SetUI(Clone)") || o.name.Contains("data(Clone)"))
        //    {
        //        _dataManager.GetUserGreenHouseList();
        //    }
        //    else
        //    {
        //        Hashtable hashtable = new Hashtable();
        //        hashtable["name"] = o.name;
        //        Debug.Log(o.name);
        //        PostNotification(kNotificationKeys.androidReturn, hashtable);
        //    }
            

        //}

    }


}
