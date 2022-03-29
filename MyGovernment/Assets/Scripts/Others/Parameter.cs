using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parameter : Define
{
    public KSensor sensor;
    public Image i_image;
    public Text m_name;
    public Text value;
    public Text unit;
    public Color normal;
    public Color unnormal;
    void Start()
    {
        
    }

    public void InitData(Hashtable hashtable)
    {
        value.text = hashtable["value"].ToString();
        m_name.text = hashtable["name"].ToString() + ":";
        int j = Convert.ToInt32(hashtable["status"].ToString());
        switch (j)
        {
            case 0:

                i_image.color = normal;
                break;
            case 1:

                i_image.color = unnormal;
                break;
            case 2:

                i_image.color = unnormal;
                break;
            case 3:

                i_image.color = unnormal;
                break;
            case 4:

                i_image.color = unnormal;
                break;
            case 5:

                i_image.color = unnormal;
                break;
            default:

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
    void Update()
    {
        
    }
}
