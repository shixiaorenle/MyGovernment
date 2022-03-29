using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WarningItem : Define
{
    public Hashtable AData = new Hashtable();
    public Text operation;//报警信息
    public Text longdate;
    public Text longtime;
    public Text index;//报警条目编码
    public Text state;
    public Button button;
    public Color no;
    public Color trigger;
    public Color confirm;
    public Color reset;
    public void initData(Hashtable hash, UnityAction fuction)
    {
        AData = hash;
        //Debug.Log(hash.toJson());
        string timetamp = AData["t"].ToString();
        operation.text = AData["m"].ToString();
        index.text = (Convert.ToInt32(AData["index"]) + 1).ToString();
        longdate.text = ConvertStringToDateTime(timetamp).Year.ToString() + "年" + ConvertStringToDateTime(timetamp).Month.ToString() + "月" + ConvertStringToDateTime(timetamp).Day.ToString() + "日";
        longtime.text = ConvertStringToDateTime(timetamp).ToLongTimeString();
        int s = Convert.ToInt32( AData["a"]);
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
        //operation.text = AData["operation"].ToString();
        //date.text = AData["date"].ToString();
        //houseid.text = AData["houseid"].ToString();
        button.onClick.AddListener(fuction);

    }
}
