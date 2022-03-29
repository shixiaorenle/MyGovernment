using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceUITwo : Define
{
    public KDevicetype deviceType;//设备类型
    public Text title;
    public Button OpenBt;
    public Button OpenStopBt;

    protected string boxNum;
    protected string mName;
    protected string id;
    protected string deviceName;
    protected string devicePinyin;
    protected string controlId;//控制开启id
    protected string openStateId;//开启状态id
    protected string stopStateId;//停止状态id
    protected string openTimeId;//控制延时停止 开启id
    public InputField i_Open;//时间输入框
    public Animator ani;
    protected float openNum;//延时停止时间
    [HideInInspector]
    public ControlType controlType = ControlType.none;
    void Start()
    {
        RegistNotification(this, kNotificationKeys.getMonitoringInfo, GetMonitoringInfoFinish);
        RegistNotification(this, kNotificationKeys.delayStopDeviceOver, DelayStopDeviceOver);
        RegistNotification(this, kNotificationKeys.controlPoint, controlPointFinish);
        i_Open.onValueChanged.AddListener(delegate (string str) { isNumber(str, i_Open); });
        i_Open.onEndEdit.AddListener(delegate (string value) {
            controlType = ControlType.opentime;
            GetWeatherInfo();
        });
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
            case KDevicetype.dayao:
                title.text = "打药机";
                break;
            case KDevicetype.none:
                break;
            default:
                break;
        }
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
            controlType = ControlType.none;
        }
    }
    /// <summary>
    /// 控制设备写值成功返回
    /// </summary>
    /// <param name="obj"></param>
    protected virtual void controlPointFinish(Hashtable obj)
    {
        Debug.Log("写值成功返回" + obj.toJson());
        switch (controlType)
        {
            case ControlType.open:
                SetOpenState(true);
                //float opentime = openNum;
               // DelayStop(opentime, deviceType);//DeviceTwo暂无延时停止功能
                break;
            case ControlType.stop:
                SetOpenState(false);
                break;
            default:
                break;
        }
        controlType = ControlType.none;
    }
    protected void DelayStop(float waitTime, KDevicetype Type)
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
    public void OnClickOpen()
    {

        controlType = ControlType.open;
        GetWeatherInfo();
    }
    public void OnClickClose()
    {

        controlType = ControlType.stop;
        GetWeatherInfo();
    }
    public void SetOpenState(bool ison)//true是按下开，当状态是开，即开隐藏，停显示，false是按下停，当状态是停，即开显示，停隐藏
    {
        if (ison)
        {
            OpenBt.gameObject.SetActive(false);
            OpenStopBt.gameObject.SetActive(true);
            ani.SetBool("work", true);
        }
        else
        {
            OpenBt.gameObject.SetActive(true);
            OpenStopBt.gameObject.SetActive(false);
            ani.SetBool("work", false);
        }
    }
    /// <summary>
    /// 初始化控制点的点名
    /// </summary>
    /// <param name="hashtable"></param>
    /// <param name="archNum"></param>
    public  void Init(ArrayList arrayList, string archNum)
    {
        switch (deviceType)
        {
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
            

            mName = hashtable["name"].ToString();
            
            id = hashtable["id"].ToString();
            //Debug.Log(hashtable.toJson());
            //Debug.Log(mName+":"+id);
            if (mName.Contains(deviceName + "启停"))
            {
                //Debug.Log(hashtable.toJson());
               // Debug.Log(mName + ":" + id);
                controlId = id.ToString();
                string valueDescription = hashtable["valueDescription"].ToString();
                Debug.Log(valueDescription);
                if (valueDescription.Equals("已关闭"))
                {
                    SetOpenState(false);
                }
                else
                {
                    SetOpenState(true);
                }
            }

        }

    }
    /// <summary>
    /// 获取环境参数返回
    /// </summary>
    /// <param name="obj"></param>
    protected void GetMonitoringInfoFinish(Hashtable obj)
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
            case KDevicetype.dayao:
                devicePinyin = "dayaoji";
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
                hash["itemId"] = controlId;
                //hash["devicename"] = controlId;
                hash["value"] = 1;
                hash["oldValue"] = 0;
                infos["infos"] = info;
                infos["type"] = "control";
                // infos["point"] = devicePinyin + "open";//原来用来判断是卷帘还是卷膜，延迟时间一个得除以10一个不用，现在不用了
                infos["point"] = deviceName + "启动";
                
                hash["info"] = infos;

                _dataManager.ControlDevice(hash);

                //Debug.Log(hash.toJson());
                break;
            case ControlType.stop:
                hash["boxNumber"] = boxNum;
                hash["itemId"] = controlId;
                hash["value"] = 0;
                hash["oldValue"] = 1;
                infos["infos"] = info;
                infos["type"] = "control";
                //infos["point"] = devicePinyin + "stop";
                infos["point"] = deviceName + "停止";
                hash["info"] = infos;

                _dataManager.ControlDevice(hash);
                //Debug.Log(hash.toJson());
                break;
            //DeviceTwo暂无延时停止功能
            //case ControlType.opentime:
            //    hash["boxNumber"] = boxNum;
            //    hash["itemId"] = openTimeId;
            //    hash["value"] = i_Open.text + "0";
            //    hash["oldValue"] = openNum;
            //    openNum = (float)Convert.ToDouble(i_Open.text);
            //    infos["infos"] = info;
            //    infos["type"] = "parameter";
            //    //infos["point"] = devicePinyin + "kaishijian";
            //    infos["point"] = deviceName + "时间设定";
            //    hash["info"] = infos;

            //    _dataManager.ControlDevice(hash);

            //    //Debug.Log(hash.toJson());
            //    break;
            default:
                break;
        }
    }
    /// <summary>
    /// 初始化开关时限参数和设备状态，DeviceTwo用不着了
    /// </summary>
    /// <param name="hashtable"></param>
    public virtual void InitDeviceStateAndParameter(Hashtable hashtable)
    {
        //Debug.Log("设备状态数据:"+hashtable.toJson());
        Debug.Log("设备状态数据:" + hashtable["name"].ToString());
        if (hashtable["name"].ToString().Contains("开启中"))
        {
            openStateId = hashtable["id"].ToString();
            string value = hashtable["value"].ToString();
            if (value.Equals("1"))
            {
                Debug.Log(deviceType + "已开启");
                SetOpenState(true);
                controlType = ControlType.open;
                InvokeRepeating("GetMyState", 3, 3);
            }
            else
            {
                Debug.Log(deviceType + "已停止");
                SetOpenState(false);
            }
            //Debug.Log(_playerCache.GetMonitoringID(hashtable["value"].ToString()));
        }
        else if (hashtable["name"].ToString().Contains("设定时间"))
        {

            string value = hashtable["value"].ToString();
            openNum = (float)Convert.ToDouble(value) / 10;
            i_Open.text = ((int)openNum).ToString();
            string Id = hashtable["id"].ToString();
            openTimeId = Id.ToString();

            Debug.Log(deviceType + "开设定时间:" + Id.ToString());
        }
    }
    public void Prevent()
    {
        _maskLayer.ShowAlertWithTips("此功能暂未开通");
    }
}
