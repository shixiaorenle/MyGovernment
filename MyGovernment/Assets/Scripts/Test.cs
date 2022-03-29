
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Test : Define
{
    public Text t1;
    public Text t2;
    public Text t3;
    //ButtonExtension btn;

    void Start()
    {
        //btn = GetComponent<ButtonExtension>();
        //btn.onClick.AddListener(Click);
        //btn.onPress.AddListener(Press);
        //btn.onDoubleClick.AddListener(DoubleClick);
        //vp_Timer.In(5, TestVP);
        // Invoke("TestVP", 5);
        t1.text = ConvertStringToDateTime(1632559497000.ToString()).ToString();
        t2.text = ConvertStringToDateTime(1632559497000.ToString()).ToLongDateString();
        t3.text = ConvertStringToDateTime(1632559497000.ToString()).ToShortDateString();
        Debug.Log(ConvertStringToDateTime(1632559497000.ToString()).Year.ToString());
        Debug.Log(ConvertStringToDateTime(1632559497000.ToString()).ToLocalTime());
        //Debug.Log(ConvertStringToDateTime(1632559497000.ToString()).ToUniversalTime());
        Debug.Log(ConvertStringToDateTime(1632559497000.ToString()).Month);
        Debug.Log(ConvertStringToDateTime(1632559497000.ToString()).Day);
        
    }

    void Click()
    {
        Debug.Log("click");
    }

    void Press()
    {
        Debug.Log("press");
    }

    void DoubleClick()
    {
        Debug.Log("double click");
    }
    void TestVP()
    {
        Debug.Log(name+33333);
    }
    public void CancelTest()
    {
        vp_Timer.CancelAll("TestVP");
    }
}
 

