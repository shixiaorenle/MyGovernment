using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorkingSurface : Define
{
    private float refreshTime = 0;
    public float refreshTimeLimit = 60;
    public Toggle myToggle;
    public CinemachineBrain myBrain;
    public Canvas canvas;
    public MaxVideo maxVideo;
    public GameObject main;
    public GameObject t1;
    public GameObject t2;
    public GameObject h;
    public GameObject st;
    public GameObject sh;
    public GameObject co2;
    public GameObject l;
    public GameObject n;
    public GameObject p;
    public GameObject k;
    public GameObject ec;
    public GameObject ph;
    private Dictionary<KSensor, GameObject> mCamera = new Dictionary<KSensor, GameObject>();
    private Dictionary<string, KSensor> mSensor = new Dictionary<string, KSensor>();
    public List<WorldUI> worldUIs = new List<WorldUI>();
    protected  void Start()
    {
        //RegistNotification(this, kNotificationKeys.getGreenHouseControlpoint, getGreenHouseControlpoint);
        RegistNotification(this, kNotificationKeys.getGreenHouseWeather, getGreenHouseWeather);
        mCamera.Add(KSensor.temperature1, t1);
        mCamera.Add(KSensor.temperature2, t2);
        mCamera.Add(KSensor.hum, h);
        mCamera.Add(KSensor.soilhum, sh);
        mCamera.Add(KSensor.soiltem, st);
        mCamera.Add(KSensor.CO2, co2);
        mCamera.Add(KSensor.light, l);
        mCamera.Add(KSensor.N, n);
        mCamera.Add(KSensor.P, p);
        mCamera.Add(KSensor.K, k);
        mCamera.Add(KSensor.ec, ec);
        mCamera.Add(KSensor.main, main);
        mCamera.Add(KSensor.PH, ph);
        mSensor.Add("空气温度1", KSensor.temperature1); mSensor.Add("空气温度2", KSensor.temperature2); mSensor.Add("空气湿度", KSensor.hum);
        mSensor.Add("土壤温度", KSensor.soiltem); mSensor.Add("土壤湿度", KSensor.soilhum); mSensor.Add("CO2浓度", KSensor.CO2);

        mSensor.Add("光照强度", KSensor.light); mSensor.Add("磷浓度", KSensor.P); mSensor.Add("氮浓度", KSensor.N);mSensor.Add("钾浓度", KSensor.K); mSensor.Add("电导率", KSensor.ec);
        mSensor.Add("土壤PH", KSensor.PH);       
        myToggle.isOn = true;
        ChangeSensor(KSensor.temperature1);
    }
    private void OnEnable()
    {
        Refresh();
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            refreshTime += Time.deltaTime;
            if (refreshTime >= refreshTimeLimit)
            {
                Refresh();
                refreshTime = 0;
            }
        }
        
    }
    public void Refresh()
    {
        Hashtable hash = new Hashtable();
        hash["boxNumber"] = _playerCache._playerData.myBoxNum;
        hash["ids"] = _playerCache.getWeather(_playerCache._playerData.myBoxNum);
        _dataManager.GetUserWeatherParam(hash);
    }
    private void getGreenHouseWeather(Hashtable obj)
    {
        Debug.Log("传感器状态："+obj.toJson());
        _maskLayer.ShowAlertWithMSG("查询成功");
        ArrayList arrayList = obj["data"] as ArrayList;
        foreach (var item in arrayList)
        {
            Hashtable hashtable = item as Hashtable;
            string str = hashtable["name"].ToString();
            //string value = hashtable["value"].ToString();

            KSensor sensor;
            if (mSensor.TryGetValue(str,out sensor))
            {
                foreach (WorldUI worldUI in worldUIs)
                {
                    if (worldUI.sensor.Equals(sensor))
                    {
                        worldUI.InitData(hashtable);
                    }
                }
            }      
        }
    }
    public void OnClickButton(Transform tran)
    {
        
        if (tran.GetComponent<Toggle>().isOn)
        {
            //Debug.Log(tran.name);
            KSensor sensor = ParseEnum<KSensor>(tran.name);
            ChangeSensor(sensor);
            myBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseIn, 1);

            //myBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0);
        }
        
        

    }
    private void ChangeSensor(KSensor sensor)
    {
        foreach (var item in mCamera)
        {
            if (item.Key.Equals(sensor))
            {
                item.Value.SetActive(true);
            }
            else
            {
                item.Value.SetActive(false);
            }
        }
        foreach (WorldUI item in worldUIs)
        {
            if (item.sensor.Equals(sensor))
            {
                item.Show();
            }
            else
            {
                item.Hide();
            }
        }
    }
    public void OpenCamera()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.MaxVideo, KMasklayerCenter.masklayer);
    }
}
