using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ControlType
{
    open, close, stop, opentime, closetime, none
}
public class DeviceUI : Define
{
    public KDevicetype deviceType;//设备类型
    public Text title;
    protected string boxNum;
    protected string mName;
    protected string id;
    protected string deviceName;
    protected string devicePinyin;
    public Button OpenBt;
    public Button OpenStopBt;
    public Button CloseBt;
    public Button CloseStopBt;
    protected string openId;//控制开启id
    protected string closeId;//控制关闭id
    protected string stopId;//控制停止id
    protected string openTimeId;//控制延时停止 开启id
    protected string closeTimeId;//控制延时停止 关闭id
    protected string openStateId;//开启中状态id
    protected string closeStateId;//关闭中状态id
    public Button preventOpen;
    public Button preventClose;
    public InputField i_Open;
    public InputField i_Close;
    public Animator ani;
    protected float openNum;//延时停止 开启时间
    protected float closeNum;//延时停止 关闭时间
    [HideInInspector]
    public ControlType controlType = ControlType.none;
    protected virtual void Update()
    {
       // Debug.Log(CloseBt.gameObject.activeSelf);
    }
    public void OnClickOpen()
    {

        controlType = ControlType.open;
        GetWeatherInfo();
    }
    public void OnClickClose()
    {

        controlType = ControlType.close;
        GetWeatherInfo();
    }
    public void OnClickStop()
    {
        CancelDelayStop(deviceType);
        controlType = ControlType.stop;
        GetWeatherInfo();
    }
    public void SetOpenState(bool ison)//true是按下开，当状态是开，即开隐藏，停显示，false是按下停，当状态是停，即开显示，停隐藏
    {
        if (ison)
        {
            OpenBt.gameObject.SetActive(false);
            OpenStopBt.gameObject.SetActive(true);
            preventClose.gameObject.SetActive(true);
            ani.SetBool("open", true);
            
        }
        else
        {
            OpenBt.gameObject.SetActive(true);
            OpenStopBt.gameObject.SetActive(false);
            preventClose.gameObject.SetActive(false);
            ani.SetBool("open", false);
        }
    }
    
    public void SetCloseState(bool ison)//true是按下关，当状态是关，即关隐藏，停显示，false是按下停，当状态是停，即关显示，停隐藏
    {
        if (ison)
        {
            CloseBt.gameObject.SetActive(false);
            CloseStopBt.gameObject.SetActive(true);
            preventOpen.gameObject.SetActive(true);
            ani.SetBool("close", true);
        }
        else
        {
            CloseBt.gameObject.SetActive(true);
            CloseStopBt.gameObject.SetActive(false);
            preventOpen.gameObject.SetActive(false);
            ani.SetBool("close", false);
        }
    }
    protected void DelayStop(float waitTime,KDevicetype Type)
    {
        if (!_playerCache._playerData.delayOpen)
        {
            return;
        }
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = Type.ToString();
        hashtable["delayTime"] = waitTime.ToString();
        hashtable["boxNum"] = boxNum.ToString();
        PostNotification(kNotificationKeys.delayStopDevice, hashtable);

    }
    protected void CancelDelayStop(KDevicetype Type)
    {
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = Type.ToString();
        hashtable["boxNum"] = boxNum.ToString();
        PostNotification(kNotificationKeys.cancelDelayStopDevice, hashtable);

    }
    /// <summary>
    /// 规定时间内停止设备
    /// </summary>
    /// <param name="obj"></param>
    protected void DelayStopDeviceOver(Hashtable obj)
    {
        string boxNumer = obj["boxNum"].ToString();
        if (!boxNumer.Equals(boxNum))
        {
            return;
        }
        string DeviceType = obj["deviceType"].ToString();
        if (deviceType.ToString().Equals(DeviceType))
        {
            SetOpenState(false);
            SetCloseState(false);
            controlType = ControlType.none;
        }
    }
    protected virtual  void Start()
    {
        RegistNotification(this, kNotificationKeys.getMonitoringInfo, GetMonitoringInfoFinish);
        RegistNotification(this, kNotificationKeys.controlPoint, controlPointFinish);
        RegistNotification(this, kNotificationKeys.delayStopDeviceOver, DelayStopDeviceOver);
        RegistNotification(this, kNotificationKeys.getSingleDeviceState, GetSingleDeviceState);
        i_Open.onValueChanged.AddListener(delegate (string str) { isNumber(str, i_Open);});
        i_Close.onValueChanged.AddListener(delegate (string str) { isNumber(str, i_Close); });
        i_Open.onEndEdit.AddListener(delegate (string value) {
            controlType = ControlType.opentime;
            GetWeatherInfo();
        });
        i_Close.onEndEdit.AddListener(delegate (string value) {
            controlType = ControlType.closetime;

            GetWeatherInfo();


        });
        //timeCountDown = 0;
        //allowCountdown = false;
        switch (deviceType)
        {
            case KDevicetype.juanlianji1:
                title.text = "卷帘1";
                break;
            case KDevicetype.juanlianji2:
                title.text = "卷帘2";
                break;
            case KDevicetype.tongfengkou1:
                title.text = "卷膜1";
                break;
            case KDevicetype.tongfengkou2:
                title.text = "卷膜2";
                break;
            case KDevicetype.tongfengkou3:
                title.text = "卷膜3";
                break;
            case KDevicetype.tongfengkou4:
                title.text = "卷膜4";
                break;
            case KDevicetype.buguangdeng:
                title.text = "补光灯";
                break;
            case KDevicetype.CO2:
                title.text = "CO2发生器";
                break;
            case KDevicetype.shuifei:
                title.text = "施肥泵";
                break;
            case KDevicetype.none:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 获取当前设备状态返回，如果是停止状态，则停止轮询获取设备状态；如果是开启状态，则return，继续获取设备状态
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void GetSingleDeviceState(Hashtable obj)
    {
        ArrayList arrayList = obj["data"] as ArrayList;
        foreach (var item in arrayList)
        {
            Hashtable hash = item as Hashtable;

            string id = hash["id"].ToString();
            if (id.Equals(closeStateId) || id.Equals(openStateId))
            {
                string value = hash["value"].ToString();
                if (value.Equals("1"))
                {
                    return;
                }
            }

        }
        SetCloseState(false);
        SetOpenState(false);
        CancelInvoke("GetMyState");
    }
    /// <summary>
    /// 获取当前设备状态
    /// </summary>
    protected  void GetMyState( )
    {
        Hashtable h = new Hashtable();
        h["boxNumber"] = boxNum;
        ArrayList arrayList = new ArrayList() { closeStateId, openStateId };
        h["ids"] = arrayList;
        _dataManager.GetDeviceState(h);
    }

    /// <summary>
    /// 获取环境参数
    /// </summary>
    protected void GetWeatherInfo()
    {
        Hashtable hash = new Hashtable();
        hash["boxNumber"] = boxNum;
        hash["ids"] = _playerCache.getWeather(boxNum);
        _dataManager.GetWeatherInfo(hash);

    }
    /// <summary>
    /// 控制设备写值成功返回
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void controlPointFinish(Hashtable obj)
    {
        Debug.Log("写值成功返回" + obj.toJson());
        _maskLayer.ShowAlertWithMSG("操作成功");
        switch (controlType)
        {
            case ControlType.open:
                SetOpenState(true);
                float opentime = openNum;
                DelayStop(opentime, deviceType);
                break;
            case ControlType.close:
                SetCloseState(true);
                float closetime = closeNum;
                DelayStop(closetime, deviceType);
                break;
            case ControlType.stop:
                SetOpenState(false);
                SetCloseState(false);
                break;
            case ControlType.opentime:
                break;
            case ControlType.closetime:
                break;
            default:
                break;
        }
        controlType = ControlType.none;
    }

    public void OnClickPrevent()
    {
        _maskLayer.ShowAlertWithTips("请先停止设备再进行操作!");
    }
    /// <summary>
    /// 初始化控制点的点名
    /// </summary>
    /// <param name="hashtable"></param>
    /// <param name="archNum"></param>
    public virtual  void Init(ArrayList arrayList, string archNum)
    {
        switch (deviceType)
        {
            case KDevicetype.juanlianji1:
                deviceName = "卷帘1";
                break;
            case KDevicetype.juanlianji2:
                deviceName = "卷帘2";
                break;
            case KDevicetype.tongfengkou1:
                deviceName = "卷膜1";
                break;
            case KDevicetype.tongfengkou2:
                deviceName = "卷膜2";
                break;
            case KDevicetype.tongfengkou3:
                deviceName = "卷膜3";
                break;
            case KDevicetype.tongfengkou4:
                deviceName = "卷膜4";
                break;
            case KDevicetype.buguangdeng:
                deviceName = "补光灯";
                break;
            case KDevicetype.CO2:
                deviceName = "CO2";
                break;
            case KDevicetype.shuifei:
                deviceName = "水肥机";
                break;
            case KDevicetype.dayao:
                deviceName = "打药机";
                break;
            case KDevicetype.none:
                break;
            default:
                break;
        }
        boxNum = archNum;
        foreach (var item in arrayList)
        {
            Hashtable hashtable = item as Hashtable;
            //Debug.Log(hashtable.toJson());

            mName = hashtable["name"].ToString();
            id = hashtable["id"].ToString();
            if (mName.Contains(deviceName + "关")) 
            {
                closeId = id.ToString();
                //Debug.Log("卷帘关;" + id.ToString());                      
            }
            else if (mName.Contains(deviceName + "停"))
            {
                stopId = id.ToString();
                //Debug.Log("卷帘停:"+id.ToString());
            }
            else if (mName.Contains(deviceName + "开"))
            {
                openId = id.ToString();
                //Debug.Log(deviceName+"开:"+id.ToString());
            }
        }

    }
    /// <summary>
    /// 初始化开关时限参数和设备状态
    /// </summary>
    /// <param name="hashtable"></param>
    public virtual void InitDeviceStateAndParameter(Hashtable hashtable)
    {
        //Debug.Log("设备状态数据:"+hashtable.toJson());
        if (hashtable["name"].ToString().Contains("开启中"))
        {
            openStateId = hashtable["id"].ToString();
            string value = hashtable["value"].ToString();
            if (value.Equals("1"))
            {
                 //Debug.Log(deviceType + "正转中");
                SetOpenState(true);
                controlType = ControlType.open;
                InvokeRepeating("GetMyState", 3, 3);
            }
            else
            {
                //Debug.Log(deviceType+"正转停");
                SetOpenState(false);
            }
            //Debug.Log(_playerCache.GetMonitoringID(hashtable["value"].ToString()));
        }
        else if (hashtable["name"].ToString().Contains("关闭中"))
        {
            closeStateId = hashtable["id"].ToString();
            string value = hashtable["value"].ToString();
            if (value.Equals("1"))
            {
                // Debug.Log(deviceType + "反转中");
                SetCloseState(true);
                controlType = ControlType.close;
                InvokeRepeating("GetMyState", 3, 3);
            }
            else
            {
                //Debug.Log(deviceType+"反转停);
                SetCloseState(false);
            }
            //Debug.Log(_playerCache.GetMonitoringID(hashtable["value"].ToString()));
        }
        else if (hashtable["name"].ToString().Contains("开设定时间"))
        {

            string value = hashtable["value"].ToString();
            openNum = (float)Convert.ToDouble(value) / 10;
            i_Open.text = ((int)openNum).ToString();
            string Id = hashtable["id"].ToString();
            openTimeId = Id.ToString();

            //Debug.Log(deviceType + "开设定时间:" + Id.ToString());
        }
        else if (hashtable["name"].ToString().Contains("关设定时间"))
        {

            string value = hashtable["value"].ToString();
            closeNum = (float)Convert.ToDouble(value) / 10;
            i_Close.text = ((int)closeNum).ToString();
            string Id = hashtable["id"].ToString();
            closeTimeId = Id.ToString();
            //Debug.Log(deviceType + "关设定时间;" + Id.ToString());
        }
    }
    /// <summary>
    /// 获取环境参数返回
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void GetMonitoringInfoFinish(Hashtable obj)
    {
        Hashtable hash = new Hashtable();
        ArrayList info = new ArrayList();

        if (obj.ContainsKey("data"))
        {
            ArrayList array = obj["data"] as ArrayList;
            foreach (Hashtable item in array)
            {
                Hashtable h = new Hashtable();
                foreach (var key in item.Keys)
                {
                    //Debug.Log(key + ":" + item[key].ToString());
                    if (key.Equals("name"))
                    {
                        h[key] = item[key].ToString();
                    }
                    if (key.Equals("value"))
                    {
                        h[key] = item[key].ToString();
                    }
                    if (key.Equals("id"))
                    {
                        h[key] = item[key].ToString();
                    }
                }
                info.Add(h);
            }
        }
        Hashtable infos = new Hashtable();

        switch (deviceType)
        {
            case KDevicetype.juanlianji1:
                devicePinyin = "juanlian1";
                break;
            case KDevicetype.juanlianji2:
                devicePinyin = "juanlian2";
                break;
            case KDevicetype.tongfengkou1:
                devicePinyin = "juanmo1";
                break;
            case KDevicetype.tongfengkou2:
                devicePinyin = "juanmo2";
                break;
            case KDevicetype.tongfengkou3:
                devicePinyin = "juanmo3";
                break;
            case KDevicetype.tongfengkou4:
                devicePinyin = "juanmo4";
                break;
            case KDevicetype.buguangdeng:
                devicePinyin = "buguangdeng";
                break;
            case KDevicetype.CO2:
                devicePinyin = "co2";
                break;
            case KDevicetype.shuifei:
                devicePinyin = "shuifei";
                break;
            case KDevicetype.none:
                break;
            default:
                break;
        }
        switch (controlType)
        {
            case ControlType.open:
                hash["boxNumber"] = boxNum;
                hash["itemId"] = openId;
                hash["value"] = 1;
                hash["oldValue"] = 0;
                infos["infos"] = info;
                infos["type"] = "control";
                // infos["point"] = devicePinyin+"open";//原来用来判断是卷帘还是卷膜，延迟时间一个得除以10一个不用，现在不用了
                infos["point"] = deviceName + "正开";
                hash["info"] = infos;
                
                _dataManager.ControlDevice(hash);

                //Debug.Log(hash.toJson());
                // Debug.Log("kai");
                break;
            case ControlType.close:
                hash["boxNumber"] = boxNum;
                hash["itemId"] = closeId;
                hash["value"] = 1;
                hash["oldValue"] = 0;
                infos["infos"] = info;
                infos["type"] = "control";
                //infos["point"] = devicePinyin+"close";
                infos["point"] = deviceName + "反开";
                hash["info"] = infos;
                
                _dataManager.ControlDevice(hash);
                //Debug.Log(hash.toJson());
                //Debug.Log("guan");
                break;
            case ControlType.stop:
                hash["boxNumber"] = boxNum;
                hash["itemId"] = stopId;
                hash["value"] = 1;
                hash["oldValue"] = 0;
                infos["infos"] = info;
                infos["type"] = "control";
                //infos["point"] = devicePinyin+ "stop";
                infos["point"] = deviceName + "停";
                hash["info"] = infos;
                
                _dataManager.ControlDevice(hash);
                //Debug.Log(hash.toJson());
                // Debug.Log("ting");
                break;
            case ControlType.closetime:
                hash["boxNumber"] = boxNum;
                hash["itemId"] = closeTimeId;
                hash["value"] = i_Close.text + "0";
                hash["oldValue"] = closeNum;
                closeNum = (float)Convert.ToDouble(i_Close.text);
                infos["infos"] = info;
                infos["type"] = "parameter";
                //infos["point"] = devicePinyin+ "guanshijian";
                infos["point"] = deviceName + "反开时间设定";
                hash["info"] = infos;
               
                _dataManager.ControlDevice(hash);

                //Debug.Log(hash.toJson());
                break;
            case ControlType.opentime:
                hash["boxNumber"] = boxNum;
                hash["itemId"] = openTimeId;
                hash["value"] = i_Open.text + "0";
                hash["oldValue"] = openNum;
                openNum = (float)Convert.ToDouble(i_Open.text);
                infos["infos"] = info;
                infos["type"] = "parameter";
               // infos["point"] = devicePinyin+"kaishijian";
                infos["point"] = deviceName + "正开时间设定";
                hash["info"] = infos;
                
                _dataManager.ControlDevice(hash);

                //Debug.Log(hash.toJson());
                break;
            default:
                break;
        }
    }
}
