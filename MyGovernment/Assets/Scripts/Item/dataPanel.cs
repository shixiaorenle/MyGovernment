using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dataPanel : Define
{
    public Text m_name;//名字
    public Text value;//值
    public Text status;//点位状态
    public Image i_image;//点位状态图
    public Text connState;//盒子状态
    public Text unit;
    public Color normal;
    public Color unnormal;
    public GameObject sensorOne;
    public GameObject sensorTwo;
    public Material sensor1;
    public Material sensor2;
    public Texture Tem;
    public Texture Hum;
    public Texture soilTem;
    public Texture soilHum;
    public Texture co2;
    public Texture Light;
    public Texture P;
    public Texture N;
    public Texture K;
    public Texture EC;
    public Texture PH;
    void Start()
    {

    }
    public void InitData(Hashtable hashtable, KSensor sensor)
    {
        Debug.Log(hashtable.toJson());
        value.text = hashtable["value"].ToString();
        m_name.text = hashtable["name"].ToString();
        Debug.Log(m_name.text + "" + value.text);
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
        sensorOne.GetComponent<SensorRotate>().ResetRot();
        sensorTwo.GetComponent<SensorRotate>().ResetRot();
        switch (sensor)
        {
            case KSensor.temperature1:
                sensorOne.SetActive(true);
                sensorTwo.SetActive(false);
                sensor1.mainTexture = Tem;
                unit.text = "℃";
                break;
            case KSensor.temperature2:
                sensorOne.SetActive(true);
                sensorTwo.SetActive(false);
                sensor1.mainTexture = Tem;
                unit.text = "℃";
                break;
            case KSensor.hum:
                sensorOne.SetActive(true);
                sensorTwo.SetActive(false);
                sensor1.mainTexture = Hum;
                unit.text = "%";
                break;
            case KSensor.soiltem:
                sensorOne.SetActive(false);
                sensorTwo.SetActive(true);
                sensor2.mainTexture = soilTem;
                unit.text = "℃";
                break;
            case KSensor.soilhum:
                sensorOne.SetActive(false);
                sensorTwo.SetActive(true);
                sensor2.mainTexture = soilHum;
                unit.text = "%";
                break;
            case KSensor.light:
                sensorOne.SetActive(true);
                sensorTwo.SetActive(false);
                sensor1.mainTexture = Light;
                unit.text = "Lux";
                break;
            case KSensor.CO2:
                sensorOne.SetActive(true);
                sensorTwo.SetActive(false);
                sensor1.mainTexture = co2;
                unit.text = "ppm";
                break;
            case KSensor.N:
                sensorOne.SetActive(false);
                sensorTwo.SetActive(true);
                sensor2.mainTexture = N;
                unit.text = "g/kg";
                break;
            case KSensor.P:
                sensorOne.SetActive(false);
                sensorTwo.SetActive(true);
                sensor2.mainTexture = P;
                unit.text = "g/kg";
                break;
            case KSensor.K:
                sensorOne.SetActive(false);
                sensorTwo.SetActive(true);
                sensor2.mainTexture = K;
                unit.text = "g/kg";
                break;
            case KSensor.ec:
                sensorOne.SetActive(false);
                sensorTwo.SetActive(true);
                sensor2.mainTexture = EC;
                unit.text = "S/m";
                break;
            case KSensor.main:
                break;
            case KSensor.PH:
                sensorOne.SetActive(false);
                sensorTwo.SetActive(true);
                sensor2.mainTexture = PH;
                unit.text = "";
                break;
            default:
                break;
        }
    }

}
