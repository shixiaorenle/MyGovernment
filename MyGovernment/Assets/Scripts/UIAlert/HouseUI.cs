using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HouseUI : UIAlertLayer
{
    private EventSystem system;
    public enum DelayType
    {
        open, close, none
    }
    public Button DelayOpen;
    public Button DelayClose;
    public InputField m_name;
    public Text m_num;
    public Text id;
    public InputField describe;
    public Button working;
    public RectTransform content;
    public DeviceUI juanlianji1;
    public DeviceUI juanlianji2;
    public DeviceUI juanmo1;
    public DeviceUI juanmo2;
    public DeviceUI juanmo3;
    public DeviceUI juanmo4;
    public DeviceUITwo dayaoji;
    public DeviceUITwo buguangdeng;
    public DeviceUITwo shuifeiji;
    public DeviceUITwo CO2;
    public GameObject nZhezhao;
    public GameObject dZhezhao;
    string boxNumber; string ID; string mname;
    private string delayID;
    private DelayType delayType = DelayType.none;
    protected override void Start()
    {
        base.Start();
        system = EventSystem.current;
        setMinSize();
        RegistNotification(this, kNotificationKeys.getGreenHouseMonitoringpoint, getGreenHouseMonitoringpoint);
        RegistNotification(this, kNotificationKeys.getGreenHouseControlpoint, getControlpointFinish);
        RegistNotification(this, kNotificationKeys.delayStopDevice, DelayStopDevice);
        RegistNotification(this, kNotificationKeys.controlDelay, ControlDelay);
        RegistNotification(this, kNotificationKeys.postuserdata, OnUploadFinished);
        DelayOpen.onClick.AddListener(CloseDelay);
        DelayClose.onClick.AddListener(OpenDelay);
        m_name.onEndEdit.AddListener(delegate (string value)
        {
            //if (value.Equals(m_name.text))
            //{
            //    return;
            //}
            Hashtable hashtable = new Hashtable();
            hashtable["id"] = Convert.ToInt32(ID);
            hashtable["archName"] = value;
            Debug.Log(hashtable.toJson());
            _dataManager.PostUserData(hashtable);
            nZhezhao.SetActive(true);
        });
        describe.onEndEdit.AddListener(delegate (string value)
        {
            //if (value.Equals(describe.text))
            //{
            //    return;
            //}
            Hashtable hashtable = new Hashtable();
            hashtable["id"] = Convert.ToInt32(ID);
            hashtable["archDesc"] = value;
            Debug.Log(hashtable.toJson());
            _dataManager.PostUserData(hashtable);
            dZhezhao.SetActive(true);
        });
        //InitData();
        working.onClick.AddListener(delegate
        {
            _gameManager._modManager.ChangeModstr("workingsurface");
            Hashtable hash = new Hashtable();
            hash["boxNumber"] = boxNumber;
            hash["ids"] = _playerCache.getWeather(boxNumber);
            _dataManager.GetUserWeatherParam(hash);
        });
        dZhezhao.SetActive(true);
        nZhezhao.SetActive(true);
    }

    private void OnUploadFinished(Hashtable obj)
    {
        Debug.Log(obj.toJson());
        string code = obj["code"].ToString();
        if (code.Equals("0"))
        {
            _maskLayer.ShowAlertWithMSG("修改成功");
            _playerCache._playerData.myData["archName"] = m_name.text;
            _playerCache._playerData.myData["archDesc"] = describe.text;
        }
    }

    private void DelayIsOpen(bool yes)
    {
        DelayOpen.gameObject.SetActive(yes);
        DelayClose.gameObject.SetActive(!yes);
        _playerCache._playerData.delayOpen = yes;
    }
    private void ControlDelay(Hashtable obj)
    {
        switch (delayType)
        {
            case DelayType.open:
                DelayIsOpen(true);
                break;
            case DelayType.close:
                DelayIsOpen(false);
                break;
            case DelayType.none:
                break;
            default:
                break;
        }
        delayType = DelayType.open;
    }

    public void OpenDelay()
    {
        ArrayList info = new ArrayList();
        Hashtable infos = new Hashtable();
        Hashtable hash = new Hashtable();
        hash["boxNumber"] = boxNumber;
        hash["itemId"] = delayID;
        hash["value"] = 0;
        hash["oldValue"] = 1;
        infos["infos"] = info;
        infos["type"] = "null";
        hash["info"] = infos;
        delayType = DelayType.open;
        _dataManager.ControlDelay(hash);
    }
    public void CloseDelay()
    {
        ArrayList info = new ArrayList();
        Hashtable infos = new Hashtable();
        Hashtable hash = new Hashtable();
        hash["boxNumber"] = boxNumber;
        hash["itemId"] = delayID;
        hash["value"] = 1;
        hash["oldValue"] = 0;
        infos["infos"] = info;
        infos["type"] = "null";
        hash["info"] = infos;
        delayType = DelayType.close;
        _dataManager.ControlDelay(hash);
    }
    private void DelayTongfengkou4()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = KDevicetype.tongfengkou4.ToString();
        hashtable["boxNum"] = boxNumber.ToString();
        PostNotification(kNotificationKeys.delayStopDeviceOver, hashtable);
        Debug.Log("延时停止通风口4");
    }
    private void DelayTongfengkou3()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = KDevicetype.tongfengkou3.ToString();
        hashtable["boxNum"] = boxNumber.ToString();
        PostNotification(kNotificationKeys.delayStopDeviceOver, hashtable);
        Debug.Log("延时停止通风口3");
    }
    private void DelayTongfengkou2()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = KDevicetype.tongfengkou2.ToString();
        hashtable["boxNum"] = boxNumber.ToString();
        PostNotification(kNotificationKeys.delayStopDeviceOver, hashtable);
        Debug.Log("延时停止通风口2");
    }
    private void DelayTongfengkou1()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = KDevicetype.tongfengkou1.ToString();
        hashtable["boxNum"] = boxNumber.ToString();
        PostNotification(kNotificationKeys.delayStopDeviceOver, hashtable);
        Debug.Log("延时停止通风口1");
    }
    private void DelayJuanlianji1()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = KDevicetype.juanlianji1.ToString();
        hashtable["boxNum"] = boxNumber.ToString();
        PostNotification(kNotificationKeys.delayStopDeviceOver, hashtable);
        Debug.Log("延时停止卷帘机1");
    }
    private void DelayJuanlianji2()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = KDevicetype.juanlianji2.ToString();
        hashtable["boxNum"] = boxNumber.ToString();
        PostNotification(kNotificationKeys.delayStopDeviceOver, hashtable);
        Debug.Log("延时停止卷帘机2");
    }
    private void DelayDataoji()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["deviceType"] = KDevicetype.dayao.ToString();
        hashtable["boxNum"] = boxNumber.ToString();
        PostNotification(kNotificationKeys.delayStopDeviceOver, hashtable);
        Debug.Log("延时停止打药机");
    }
    private void DelayStopDevice(Hashtable obj)
    {
        string boxNum = obj["boxNum"].ToString();
        if (!boxNum.Equals(boxNumber))
        {
            return;
        }
        KDevicetype devicetype = (KDevicetype)Enum.Parse(typeof(KDevicetype), obj["deviceType"].ToString());
        float waitTime = (float)Convert.ToDouble(obj["delayTime"].ToString());

        switch (devicetype)
        {
            case KDevicetype.juanlianji1:
                Invoke("DelayJuanlianji1", waitTime);
                //juanlian1time = waitTime;
                break;
            case KDevicetype.juanlianji2:
                Invoke("DelayJuanlianji2", waitTime);
                break;
            case KDevicetype.tongfengkou1:
                Invoke("DelayTongfengkou1", waitTime);
                break;
            case KDevicetype.tongfengkou2:
                Invoke("DelayTongfengkou2", waitTime);
                break;
            case KDevicetype.tongfengkou3:
                Invoke("DelayTongfengkou3", waitTime);
                break;
            case KDevicetype.tongfengkou4:
                Invoke("DelayTongfengkou4", waitTime);
                break;
            case KDevicetype.buguangdeng:
                break;
            case KDevicetype.CO2:
                break;
            case KDevicetype.shuifei:
                break;
            case KDevicetype.dayao:
                Invoke("DelayDataoji", waitTime);
                break;
            case KDevicetype.none:
                break;
            default:
                break;
        }
    }
    private void getGreenHouseMonitoringpoint(Hashtable hashtable)
    {
        Debug.Log("刷新控制柜数据:" + hashtable.toJson());
        if (hashtable.ContainsKey("data"))
        {
            ArrayList list = hashtable["data"] as ArrayList;
            foreach (var aaa in list)
            {
                Hashtable item = aaa as Hashtable;
                if (item != null)
                {
                    if (item.ContainsKey("name"))
                    {

                        //需要先判断再让不同设备InitDeviceStateAndeter
                        if (item["name"].ToString().Contains("卷帘1"))
                        {
                            juanlianji1.InitDeviceStateAndParameter(item);
                        }
                        if (item["name"].ToString().Contains("卷帘2"))
                        {
                            juanlianji2.InitDeviceStateAndParameter(item);
                        }
                        if (item["name"].ToString().Contains("卷膜1"))
                        {
                            juanmo1.InitDeviceStateAndParameter(item);
                        }
                        if (item["name"].ToString().Contains("卷膜2"))
                        {
                            juanmo2.InitDeviceStateAndParameter(item);
                        }
                        if (item["name"].ToString().Contains("卷膜3"))
                        {
                            juanmo3.InitDeviceStateAndParameter(item);
                        }
                        if (item["name"].ToString().Contains("卷膜4"))
                        {
                            juanmo4.InitDeviceStateAndParameter(item);
                        }
                        if (item["name"].ToString().Contains("开启关闭延时"))
                        {
                            Debug.Log(item.toJson());
                            Debug.Log(item["name"].ToString());
                            float value = (float)Convert.ToDouble(item["value"]);
                            if (value.Equals(0))
                            {
                                DelayIsOpen(true);
                                //isDelay.isOn = true;
                            }
                            else
                            {
                                DelayIsOpen(false);

                            }
                        }

                    }
                }

            }

        }
    }

    public override void RefreshData(Hashtable hashtable)
    {
        Debug.Log(hashtable.toJson());
        GridLayoutGroup gg = content.GetComponent<GridLayoutGroup>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, content.childCount * (gg.cellSize.y) + (content.childCount + 2) * gg.spacing.y + gg.padding.top);
        boxNumber = hashtable["archNum"] as string;

        ID = hashtable["id"].ToString();
        mname = hashtable["archName"] as string;
        describe.text = hashtable["archDesc"] as string;
        m_name.text = mname;
        m_num.text = boxNumber;
        id.text = ID;
        getPoint(boxNumber);

    }
    private void GetHouseState()
    {
        Hashtable h = new Hashtable();
        h["boxNumber"] = boxNumber;
        h["ids"] = _playerCache.getDeviceStateAndParameter(boxNumber);
        //Debug.Log(h.toJson());
        _dataManager.GetUserGreenHouseParam(h);
    }

    public void Refresh()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.Control, KMasklayerCenter.control, _playerCache._playerData.myData);
    }
    ///// <summary>
    ///// 获取当前大棚详细信息
    ///// </summary>
    ///// <param name="ID"></param>
    //private void getData(int ID)
    //{
    //    Hashtable datas = new Hashtable();
    //    datas["id"] = Convert.ToInt32(ID);
    //    _dataManager.GetUserGreenHouseData(datas);
    //}
    /// <summary>
    /// 获取当前大棚控制点
    /// </summary>
    /// <param name="archNum"></param>
    private void getPoint(string archNum)
    {
        Hashtable points = new Hashtable();
        points["boxNumber"] = archNum;
        Debug.Log(points.toJson());
        _dataManager.GetUserGreenHouseControlPoint(points);
    }
    /// <summary>
    /// 获取所有点位
    /// </summary>
    /// <param name="obj"></param>
    private void getControlpointFinish(Hashtable obj)
    {
        Debug.Log("所有点位：" + obj.toJson());
        ArrayList list = obj["data"] as ArrayList;
        foreach (var item in list)
        {
            Hashtable j = item as Hashtable;
            foreach (string str in j.Keys)
            {
                //默认组、数据显示、设备控制、参数设置、设备状态
                if (j[str].Equals("设备控制"))
                {
                    // Debug.Log(str + ":" + j[str]);
                    ArrayList items = j["items"] as ArrayList;
                    juanlianji1.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    juanlianji2.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    juanmo1.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    juanmo2.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    juanmo3.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    juanmo4.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    dayaoji.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    buguangdeng.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    shuifeiji.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    CO2.Init(items, boxNumber);//每个设备直接Init(设定开关停id)就可以，然后每个设备单独判断
                    foreach (var array in items)
                    {
                        Hashtable hash = array as Hashtable;
                        foreach (string key in hash.Keys)
                        {

                            if (key.Equals("name"))
                            {

                                SaveControl(hash, boxNumber);

                                //Debug.Log(hash.toJson());
                                //Debug.Log(key + ":" + hash[key]);

                            }

                        }
                    }
                }
                else if (j[str].Equals("数据显示"))
                {
                    //Debug.Log(str + ":" + j[str]);
                    ArrayList items = j["items"] as ArrayList;
                    foreach (var array in items)
                    {
                        Hashtable hash = array as Hashtable;
                        foreach (string key in hash.Keys)
                        {
                            //Debug.Log(key + ":" + hash[key]);
                            if (key.Equals("name"))
                            {

                                SaveMonitoring(hash, boxNumber);
                                //Debug.Log(hash.toJson());
                                //Debug.Log(key + ":" + hash[key]);
                                //Hashtable hashtable = new Hashtable();

                                //AData["monitoring"] = hash;
                            }

                        }
                    }
                }
                else if (j[str].Equals("设备状态"))
                {
                    //Debug.Log(str + ":" + j[str]);
                    ArrayList items = j["items"] as ArrayList;
                    foreach (var array in items)
                    {
                        Hashtable hash = array as Hashtable;
                        foreach (string key in hash.Keys)
                        {
                            //Debug.Log(key + ":" + hash[key]);
                            if (key.Equals("name"))
                            {

                                SaveDeviceState(hash, boxNumber);
                                //Debug.Log(hash.toJson());
                                //Debug.Log(key + ":" + hash[key]);

                                //AData["devicestate"] = hash;
                            }

                        }
                    }
                }
                else if (j[str].Equals("参数设置"))
                {
                    //Debug.Log(str + ":" + j[str]);
                    ArrayList items = j["items"] as ArrayList;
                    foreach (var array in items)
                    {
                        Hashtable hash = array as Hashtable;
                        foreach (string key in hash.Keys)
                        {
                            //Debug.Log(key + ":" + hash[key]);
                            if (key.Equals("name"))
                            {

                                SaveParameter(hash, boxNumber);
                                string n = hash[key].ToString();
                                if (n.Equals("开启关闭延时"))
                                {
                                    delayID = hash["id"].ToString();
                                    //Debug.Log(n);
                                    //Debug.Log(hash["id"]);
                                }

                                // Debug.Log(key + ":" + hash[key]);

                                //AData["parameter"] = hash;
                            }

                        }
                    }
                }
                else if (j[str].Equals("气象数据"))
                {
                    //Debug.Log(str + ":" + j[str]);
                    ArrayList items = j["items"] as ArrayList;
                    foreach (var array in items)
                    {
                        Hashtable hash = array as Hashtable;
                        foreach (string key in hash.Keys)
                        {
                            //Debug.Log(key + ":" + hash[key]);
                            if (key.Equals("name"))
                            {

                                SaveWeather(hash, boxNumber);
                            }

                        }
                    }
                }
            }
        }
        GetHouseState();


        //打印所有保存的控制柜
        //foreach (var item in _playerCache._playerData.mGreenHouses)
        //{
        //    Debug.Log(item.Key +":"+ item.Value);
        //}
        //打印所有保存的控制点
        //foreach (var item in _playerCache._playerData.mPoints)
        //{
        //    foreach (var i in item.Value)
        //    {
        //        Debug.Log(item.Key + ":" + i.Key + "," + i.Value);
        //    }
        //}

        //打印所有保存的监控点
        //foreach (var item in _playerCache._playerData.mMonitoringPoints)
        //{
        //    foreach (var i in item.Value)
        //    {
        //        Debug.Log(item.Key + ":" + i.Key + "," + i.Value);
        //    }
        //}
        //打印所有保存的设备状态点
        //foreach (var item in _playerCache._playerData.mDeviceState)
        //{
        //    foreach (var i in item.Value)
        //    {
        //        Debug.Log(item.Key + ":" + i.Key + "," + i.Value);
        //    }
        //}
    }
    /// <summary>
    /// 保存数据显示信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="boxnum"></param>
    private void SaveMonitoring(Hashtable hash, string boxnum)
    {
        string id = hash["id"].ToString();
        string mName = hash["name"].ToString();
        string boxNum = boxnum;
        if (!_playerCache._playerData.mMonitoringPoints.ContainsKey(boxNum))
        {
            _playerCache._playerData.mMonitoringPoints.Add(boxNum, new Dictionary<string, string>());
        }
        foreach (var item in _playerCache._playerData.mMonitoringPoints)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> dic = item.Value;
                if (!dic.ContainsKey(mName))
                {
                    dic.Add(mName, id);
                }
            }
        }
    }
    /// <summary>
    /// 保存设备状态点信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="boxnum"></param>
    private void SaveDeviceState(Hashtable hash, string boxnum)
    {
        string id = hash["id"].ToString();
        string mName = hash["name"].ToString();
        string boxNum = boxnum;
        if (!_playerCache._playerData.mDeviceState.ContainsKey(boxNum))
        {
            _playerCache._playerData.mDeviceState.Add(boxNum, new Dictionary<string, string>());
        }
        foreach (var item in _playerCache._playerData.mDeviceState)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> dic = item.Value;
                if (!dic.ContainsKey(mName))
                {
                    dic.Add(mName, id);
                }
            }
        }
    }
    /// <summary>
    /// 保存设备参数点信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="boxnum"></param>
    private void SaveParameter(Hashtable hash, string boxnum)
    {
        string id = hash["id"].ToString();
        string mName = hash["name"].ToString();
        string boxNum = boxnum;
        if (!_playerCache._playerData.mParameterSet.ContainsKey(boxNum))
        {
            _playerCache._playerData.mParameterSet.Add(boxNum, new Dictionary<string, string>());
        }
        foreach (var item in _playerCache._playerData.mParameterSet)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> dic = item.Value;
                if (!dic.ContainsKey(mName))
                {
                    dic.Add(mName, id);
                }
            }
        }
    }
    /// <summary>
    /// 保存所有气象数据点信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="boxnum"></param>
    private void SaveWeather(Hashtable hash, string boxnum)
    {
        string id = hash["id"].ToString();
        string mName = hash["name"].ToString();
        string boxNum = boxnum;
        if (!_playerCache._playerData.mWeatherSet.ContainsKey(boxNum))
        {
            _playerCache._playerData.mWeatherSet.Add(boxNum, new Dictionary<string, string>());
        }
        foreach (var item in _playerCache._playerData.mWeatherSet)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> dic = item.Value;
                if (!dic.ContainsKey(mName))
                {
                    dic.Add(mName, id);
                }
            }
        }
    }
    /// <summary>
    /// 保存控制点信息
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="boxnum"></param>
    private void SaveControl(Hashtable hash, string boxnum)
    {
        string id = hash["id"].ToString();

        string mName = hash["name"].ToString();

        //Debug.Log(mName);
        string boxNum = boxnum;

        if (!_playerCache._playerData.mPoints.ContainsKey(boxNum))
        {
            _playerCache._playerData.mPoints.Add(boxNum, new Dictionary<string, string>());
        }
        foreach (var item in _playerCache._playerData.mPoints)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> dic = item.Value;
                if (!dic.ContainsKey(mName))
                {
                    dic.Add(mName, id);
                }
            }
        }
        if (!_playerCache._playerData.mStopPoints.ContainsKey(boxNum))
        {
            _playerCache._playerData.mStopPoints.Add(boxNum, new Dictionary<string, string>());
        }
        if (!_playerCache._playerData.mQTPoints.ContainsKey(boxNum))
        {
            _playerCache._playerData.mQTPoints.Add(boxNum, new Dictionary<string, string>());
        }
        if (mName.Contains("启停"))
        {
            foreach (var item in _playerCache._playerData.mQTPoints)
            {
                if (item.Key.Equals(boxNum))
                {
                    Dictionary<string, string> dic = item.Value;
                    if (!dic.ContainsKey(mName))
                    {
                        dic.Add(mName, id);
                    }
                }
            }
        }
        else if (mName.Contains("停"))
        {
            foreach (var item in _playerCache._playerData.mStopPoints)
            {
                if (item.Key.Equals(boxNum))
                {
                    Dictionary<string, string> dic = item.Value;
                    if (!dic.ContainsKey(mName))
                    {
                        dic.Add(mName, id);
                    }
                }
            }
        }
    }
    public void EditName()
    {
        nZhezhao.SetActive(false);
        system.SetSelectedGameObject(m_name.gameObject);
    }
    public void EditDesc()
    {
        dZhezhao.SetActive(false);
        system.SetSelectedGameObject(describe.gameObject);
    }
}

