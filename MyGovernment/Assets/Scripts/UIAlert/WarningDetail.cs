using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WarningDetail : UIAlertLayer
{
    public Text boxnum;
    public Text id;
    public Text state;
    public Text detail;
    public Text rn;
    public Text longdate;
    public Text longtime;
    public Color no;
    public Color trigger;
    public Color confirm;
    public Color reset;

    private string alarmId;
    private string timetamp;

    protected override  void  Start()
    {
        base.Start();
        setMinSize();
    }


    public override void RefreshData(Hashtable hashtable)
    {
        Debug.Log(hashtable.toJson());
        Debug.Log(hashtable["rn"].ToString());
        boxnum.text = hashtable["boxNumber"].ToString();
        
        alarmId = hashtable["id"].ToString();       
        detail.text = hashtable["m"].ToString();
        rn.text = hashtable["rn"].ToString();//报警条目
        id.text = (Convert.ToInt32(hashtable["index"]) + 1).ToString();
        timetamp = hashtable["t"].ToString();
        longdate.text = ConvertStringToDateTime(timetamp).Year.ToString() + "年" + ConvertStringToDateTime(timetamp).Month.ToString() + "月" + ConvertStringToDateTime(timetamp).Day.ToString() + "日";
        longtime.text = ConvertStringToDateTime(timetamp).ToLongTimeString();
        int s = Convert.ToInt32(hashtable["a"]);
        switch (s)
        {
            case 1:
                state.color = trigger;
                state.text = "触发";
                break;
            case 2:
                state.color = confirm;
                state.text = "确认";
                break;
            case 3:
                state.color = reset;
                state.text = "还原";
                break;
            default:
                state.color = no;
                state.text = "无";
                break;
        }
    }
}
