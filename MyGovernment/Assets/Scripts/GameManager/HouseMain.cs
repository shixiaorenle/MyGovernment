using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HouseMain : Define
{
    string boxNumber; string ID; string mname;
    private EventSystem system;
    private Hashtable Param;
    public InputField boxName;
    public Text boxNum;
    public Toggle tg_Control;
    public InputField describe;
    private bool firstInitControl = true;
    private bool firstInitRecord = true;
    private bool firstInitAlarm = true;
    private bool firstInitData = true;
    private ToggleType _MtoggleType = ToggleType.none;
    private Dictionary<ToggleType, Transform> m_Dic = new Dictionary<ToggleType, Transform>();
    public Transform control;
    public Transform record;
    public Transform warning;
    public Transform set;
    //public Transform working;//3d工作面，手机端卡顿不用了
    public Transform data;

    void Start()
    {
        system = EventSystem.current;
        m_Dic.Add(ToggleType.control, control);
        m_Dic.Add(ToggleType.record, record);
        m_Dic.Add(ToggleType.warning, warning);
        m_Dic.Add(ToggleType.set, set);
        m_Dic.Add(ToggleType.data, data);
        RegistNotification(this, kNotificationKeys.getGreenHouselist, ReceivedHouseData);//刷新大棚页面
        RegistNotification(this, kNotificationKeys.addGreenHouse, AddHouseFinish);//添加大棚结束后刷新大棚列表
        RegistNotification(this, kNotificationKeys.deleteGreenHouse, DeleteHouseFinish);//删除大棚结束后刷新大棚列表
        RegistNotification(this, kNotificationKeys.getAlarm, GetAlarmFinish);//获取报警中心数据
        RegistNotification(this, kNotificationKeys.getRecord, ReceivedRecord);//获取记录结束后刷新记录列表
        RegistNotification(this, kNotificationKeys.getGreenHouseWeather, getGreenHouseWeather);//获取监控数据结束后刷新监控数据列表
        boxName.onEndEdit.AddListener(delegate (string value)
        {
            Hashtable hashtable = new Hashtable();
            hashtable["id"] = Convert.ToInt32(ID);
            hashtable["archName"] = value;
            _dataManager.PostUserData(hashtable);
        });
        describe.onEndEdit.AddListener(delegate (string value)
        {
            Hashtable hashtable = new Hashtable();
            hashtable["id"] = Convert.ToInt32(ID);
            hashtable["archDesc"] = value;
            _dataManager.PostUserData(hashtable);
        });
    }

    private void AddHouseFinish(Hashtable hash)
    {
        _maskLayer.ShowAlertWithMSG("添加成功");
        _dataManager.GetUserGreenHouseList(hash);
    }
    private void DeleteHouseFinish(Hashtable hash)
    {
        _maskLayer.ShowAlertWithMSG("删除成功");
        _dataManager.GetUserGreenHouseList(hash);
    }
    private void GetAlarmFinish(Hashtable obj)
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.WarningList, KMasklayerCenter.warning, obj);
    }
    private void getGreenHouseWeather(Hashtable obj)
    {
        //_maskLayer.ShowAlertWithMSG("查询成功");
        _maskLayer.ShowAlertOnly(kAlertTypes.Data, KMasklayerCenter.Data, obj);
    }

    private void ReceivedRecord(Hashtable obj)
    {
        // Debug.Log(obj.toJson());
        // _maskLayer.ShowAlertWithMSG("查询成功");
        _maskLayer.ShowAlertOnly(kAlertTypes.Record, KMasklayerCenter.record, obj);
    }

    private void ReceivedHouseData(Hashtable obj)
    {
        firstInitControl = true;
        firstInitRecord = true;
        firstInitAlarm = true;
        firstInitData = true;
        _maskLayer.RemoveAll();
       // _gameManager._PCUIManager.Showlist(true);
        // _maskLayer.ShowAlertOnly(kAlertTypes.Center, KMasklayerCenter.masklayer, obj);


    }

    public void OpenControl(Hashtable hashtable)
    {
        Debug.Log(11111);
        _MtoggleType = ToggleType.none;
        firstInitControl = true;
        firstInitRecord = true;
        firstInitAlarm = true;
        firstInitData = true;
        if (!tg_Control.isOn)
        {
            tg_Control.isOn = true;
        }
        else
        {
            ChangeType(ToggleType.control);     
        }
        boxNumber = hashtable["archNum"] as string;
        ID = hashtable["id"].ToString();
        mname = hashtable["archName"] as string;
        describe.text = hashtable["archDesc"] as string;
        boxNum.text = hashtable["archNum"].ToString();
        boxName.text = hashtable["archName"] as string;
    }
    public void OnToggleChange(Transform transform)
    {
        ChangeType(ParseEnum<ToggleType>(transform.name));
    }
    public void ChangeType(ToggleType toggle)
    {
        if (_MtoggleType.Equals(toggle))
        {
            return;
        }
        if (_playerCache._playerData.myData.Equals(null) || _playerCache._playerData.myBoxNum.Equals(null) || _playerCache._playerData.myBoxNum.Equals(""))
        {
            return;
        }
        _MtoggleType = toggle;
        foreach (var item in m_Dic)
        {
            if (item.Key.Equals(toggle))
            {
                item.Value.gameObject.SetActive(true);
            }
            else
            {
                item.Value.gameObject.SetActive(false);
            }
        }
        
        switch (_MtoggleType)
        {
            case ToggleType.control:
                //UICamera.gameObject.SetActive(true);
                if (firstInitControl)
                {

                    _maskLayer.ShowAlertOnly(kAlertTypes.Control, KMasklayerCenter.control, _playerCache._playerData.myData);
                    firstInitControl = false;
                }
                break;
            case ToggleType.record:
                //UICamera.gameObject.SetActive(true);
                if (firstInitRecord)
                {
                    Hashtable h = new Hashtable();
                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                    //h["createTimeS"] = firstDay.ToDateString();
                    //h["createTimeE"] = lastDay.ToDateString();
                    h["boxNumber"] = _playerCache._playerData.myBoxNum;


                    _dataManager.GetRecord(h);
                    firstInitRecord = false;
                }
                break;
            case ToggleType.warning:
                //UICamera.gameObject.SetActive(true);
                if (firstInitAlarm)
                {

                    Hashtable h = new Hashtable();
                    h["boxNumber"] = _playerCache._playerData.myBoxNum;

                    _dataManager.GetAlarm(h);
                    firstInitAlarm = false;
                }
                break;
            case ToggleType.working:

                //UICamera.gameObject.SetActive(false);
                break;
            case ToggleType.data:

                //UICamera.gameObject.SetActive(true);

                if (firstInitData)
                {

                    Hashtable hash = new Hashtable();
                    hash["boxNumber"] = _playerCache._playerData.myBoxNum;
                    hash["ids"] = _playerCache.getWeather(_playerCache._playerData.myBoxNum);
                    _dataManager.GetUserWeatherParam(hash);
                    firstInitData = false;
                }
                break;
            default:
                break;
        }
    }
    void Update()
    {
        
    }
}
