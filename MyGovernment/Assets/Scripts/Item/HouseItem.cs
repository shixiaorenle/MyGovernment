using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HouseItem : Define
{
    public Hashtable AData = new Hashtable();
    public Text m_name;
    public Text SN;
    public Text Des;
    public Text id;
    public Image state;
    public Text stateTx;
    public Button button;
    public ButtonExtension btn;//长按按钮
    [HideInInspector]
    public string boxNumber;//大棚序列号
    [HideInInspector]
    public string mname;//大棚名字
    public Color onLine;
    public Color offLine;
    //public Sprite Net;//以太网
    //public Sprite GPRS;
    //public Sprite net_4G;
    //public Sprite offline;


    private void Start()
    {

        RegistNotification(this, kNotificationKeys.getHouseItemState, getHouseItemStateFinish);

        RegistNotification(this, kNotificationKeys.cancelDelayStopDevice, CancelDelayStopDevice);
    }

    private void CancelDelayStopDevice(Hashtable obj)
    {
        string boxNum = obj["boxNum"].ToString();
        if (!boxNum.Equals(boxNumber))
        {
            return;
        }
        KDevicetype devicetype = (KDevicetype)Enum.Parse(typeof(KDevicetype), obj["deviceType"].ToString());
        switch (devicetype)
        {
            case KDevicetype.juanlianji1:
                CancelInvoke("DelayJuanlianji1");
                break;
            case KDevicetype.juanlianji2:
                CancelInvoke("DelayJuanlianji2");
                break;
            case KDevicetype.tongfengkou1:
                CancelInvoke("DelayTongfengkou1");
                break;
            case KDevicetype.tongfengkou2:
                CancelInvoke("DelayTongfengkou2");
                break;
            case KDevicetype.tongfengkou3:
                CancelInvoke("DelayTongfengkou3");
                break;
            case KDevicetype.tongfengkou4:
                CancelInvoke("DelayTongfengkou4");
                break;
            case KDevicetype.buguangdeng:
                break;
            case KDevicetype.CO2:
                break;
            case KDevicetype.shuifei:
                break;
            case KDevicetype.dayao:
                CancelInvoke("DelayDataoji");
                break;
            case KDevicetype.none:
                break;
            default:
                break;
        }
    }



    private void getHouseItemStateFinish(Hashtable obj)
    {
        Debug.Log(obj.toJson());
        Hashtable data = obj["data"] as Hashtable;

        string boxNo = data["boxNo"].ToString();
        if (boxNo.Equals(boxNumber))
        {
            string connectionState = data["connectionState"].ToString();

            if (connectionState.Equals("1"))
            {
                state.color = onLine;
                string net = data["net"].ToString();
                button.enabled = true;
                switch (net)
                {

                    case "1":
                        
                        stateTx.text = "以太网在线";
                        break;
                    case "2":
                        stateTx.text = "GPRS在线";
                        break;
                    case "5":
                        stateTx.text = "4g在线";
                        break;
                    default:
                        stateTx.text = "未知";
                        break;
                }
            }
            else if (connectionState.Equals("2"))
            {
                state.color = offLine;
                stateTx.text = "连接超时";
            }
            else
            {
                state.color = offLine;
                stateTx.text = "离线";
            }
        }
         
        
        
    }


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="click"></param>
    /// <param name="press"></param>
    public void initData(Hashtable hash,  UnityAction click, UnityAction press)
    {
        AData = hash;
         Debug.Log(AData.toJson());
        boxNumber = AData["archNum"].ToString();
        mname = AData["archName"] as string;
        m_name.text = mname;
        id.text = AData["id"].ToString();
        SN.text = AData["archNum"].ToString();
        Des.text = AData["archDesc"] as string;
        button.onClick.AddListener(click);
        
        btn.onPress.AddListener(press);
        button.enabled = false;
        if (!_playerCache._playerData.mGreenHouses.ContainsKey(boxNumber))
        {
            _playerCache._playerData.mGreenHouses.Add(boxNumber, mname);
        }
        RefreshDeviceState();
    }
    /// <summary>
    /// 刷新设备状态
    /// </summary>
    public void RefreshDeviceState()
    {
        Hashtable h = new Hashtable();
        h["boxNumber"] = boxNumber;
        Debug.Log(h.toJson());
        _dataManager.GetHouseItemState(h);
    }
}
