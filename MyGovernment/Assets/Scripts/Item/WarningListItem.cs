using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WarningListItem : Define
{
    public Hashtable AData = new Hashtable();
    public Text detail;//报警内容
    public Text id;
    public Text t_state;
    public Button button;
    public Color no;
    public Color trigger;
    public Color confirm;
    public Color reset;
    public void initData(Hashtable hash, UnityAction fuction)
    {
        AData = hash;
        Debug.Log(hash.toJson());
        detail.text = AData["alarmMsg"].ToString();
        id.text = AData["code"].ToString();
        int s = Convert.ToInt32(AData["alarmState"]);
        switch (s)
        {
            case 1:
                t_state.color = trigger;
                t_state.text = "触发";
                break;
            case 4:
                t_state.color = confirm;
                t_state.text = "确认";
                break;
            case 2:
                t_state.color = reset;
                t_state.text = "还原";
                break;
            default:
                t_state.color = no;
                t_state.text = "无";
                break;
        }
        button.onClick.AddListener(fuction);

    }
}
