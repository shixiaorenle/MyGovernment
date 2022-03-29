using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WorldUI : Define
{
    public Text m_name;//名字
    public Text value;//值
    public Text status;//点位状态
    public Image i_image;//点位状态图
    public Text connState;//盒子状态
    public Text unit;
    public KSensor sensor;
    public Color normal;
    public Color unnormal;
    public CanvasGroup cg;
    public GameObject mSensor;
    void Start()
    {
        
    }
    public void InitData(Hashtable hashtable)
    {
        Debug.Log(hashtable.toJson());
        value.text = hashtable["value"].ToString();
        m_name.text = hashtable["name"].ToString();
        Debug.Log(m_name.text+""+ value.text);
        int i = Convert.ToInt32(hashtable["connState"].ToString());
        switch (i)
        {
            case 1:
                connState.text = "已连接";
                break;
            case 2:
                connState.text = "超时";
                break;
            case 3:
                connState.text = "断开";
                break;
            default:
                connState.text = "未知";
                break;
        }
        int j = Convert.ToInt32(hashtable["status"].ToString());
        switch (j)
        {
            case 0:
                status.text = "正常";
                i_image.color = normal;
                break;
            case 1:
                status.text = "无数据";
                i_image.color = unnormal;
                break;
            case 2:
                status.text = "超时";
                i_image.color = unnormal;
                break;
            case 3:
                status.text = "错误";
                i_image.color = unnormal;
                break;
            case 4:
                status.text = "Socket异常";
                i_image.color = unnormal;
                break;
            case 5:
                status.text = "FDS错误";
                i_image.color = unnormal;
                break;
            default:
                status.text = "未完成";
                i_image.color = unnormal;
                break;

        }
        switch (sensor)
        {
            case KSensor.temperature1:
                unit.text = "℃";
                break;
            case KSensor.temperature2:
                unit.text = "℃";
                break;
            case KSensor.hum:
                unit.text = "%";
                break;
            case KSensor.soiltem:
                unit.text = "℃";
                break;
            case KSensor.soilhum:
                unit.text = "%";
                break;
            case KSensor.light:
                unit.text = "Lux";
                break;
            case KSensor.CO2:
                unit.text = "ppm";
                break;
            case KSensor.N:
                unit.text = "g/kg";
                break;
            case KSensor.P:
                unit.text = "g/kg";
                break;
            case KSensor.K:
                unit.text = "g/kg";
                break;
            case KSensor.ec:
                unit.text = "S/m";
                break;
            case KSensor.main:
                break;
            case KSensor.PH:
                unit.text = "";
                break;
            default:
                break;
        }
    }
    public void Show()
    {
        cg.DOFade(1, 1);
        mSensor.SetActive(true);
    }
    public void Hide()
    {
        cg.alpha = 0;
        mSensor.SetActive(false);
    }
    void Update()
    {
        
    }
}
