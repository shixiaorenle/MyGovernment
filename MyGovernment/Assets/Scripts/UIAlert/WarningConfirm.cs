using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WarningConfirm : UIAlertLayer
{
    public Text boxnum;
    public Text id;
    public Text state;
    public Text detail;
    public Text value;

    public Color no;
    public Color trigger;
    public Color confirm;
    public Color reset;

    private string alarmId;

    protected override void Start()
    {
        base.Start();
        setMinSize();
        RegistNotification(this, kNotificationKeys.confirmAlarm, ConfirmAlarm);

    }



    private void ConfirmAlarm(Hashtable obj)
    {
        // Debug.Log(obj.toJson());
        _maskLayer.ShowAlertWithMSG("确认成功");
        PostNotification(kNotificationKeys.refreshAlarm,obj);
        onClose();
    }

    public override void RefreshData(Hashtable hashtable)
    {
        boxnum.text = _playerCache._playerData.myBoxNum;

        Debug.Log(hashtable.toJson());
        alarmId = hashtable["id"].ToString();
        detail.text = hashtable["alarmMsg"].ToString();
        value.text = hashtable["valueOnLastEvent"].ToString();
        id.text = hashtable["code"].ToString();
        int s = Convert.ToInt32(hashtable["alarmState"]);
        switch (s)
        {
            case 1:
                state.color = trigger;
                state.text = "触发";
                break;
            case 4:
                state.color = confirm;
                state.text = "确认";
                break;
            case 2:
                state.color = reset;
                state.text = "还原";
                break;
            default:
                state.color = no;
                state.text = "无";
                break;
        }
        
    }
    public void Confirm()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["id"] = alarmId;
        //Debug.Log(hashtable.toJson());
        _dataManager.ConfirmAlarm(hashtable);
    }
}
