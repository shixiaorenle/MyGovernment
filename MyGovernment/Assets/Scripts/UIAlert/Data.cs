using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data : UIAlertLayer
{
    public RectTransform content;
    private Dictionary<string, KSensor> mSensor = new Dictionary<string, KSensor>();
    private Dictionary<KSensor, Hashtable> mData = new Dictionary<KSensor, Hashtable>();

    public List<Parameter> Parameters = new List<Parameter>();
    private void Awake()
    {
        
        mSensor.Add("空气温度1", KSensor.temperature1); mSensor.Add("空气温度2", KSensor.temperature2); mSensor.Add("空气湿度", KSensor.hum);
        mSensor.Add("土壤温度", KSensor.soiltem); mSensor.Add("土壤湿度", KSensor.soilhum); mSensor.Add("CO2浓度", KSensor.CO2);

        mSensor.Add("光照强度", KSensor.light); mSensor.Add("磷浓度", KSensor.P); mSensor.Add("氮浓度", KSensor.N); mSensor.Add("钾浓度", KSensor.K); mSensor.Add("电导率", KSensor.ec);
        mSensor.Add("土壤PH", KSensor.PH);
    }
    protected override void Start()
    {
        base.Start();
        setMinSize();
        
    }
    public override void RefreshData(Hashtable hash)
    {
        
        Debug.Log(hash.toJson());
        mData.Clear();
        ArrayList arrayList = hash["data"] as ArrayList;
        foreach (var item in arrayList)
        {
            Hashtable hashtable = item as Hashtable;
            string str = hashtable["name"].ToString();
            //string value = hashtable["value"].ToString();

            KSensor sensor;
            if (mSensor.TryGetValue(str, out sensor))
            {
                mData.Add(sensor, hashtable);
            }
        }

        foreach (Parameter item in Parameters)
        {
            Hashtable hashtable;
            if (mData.TryGetValue(item.sensor,out hashtable))
            {
                item.InitData(hashtable);
            }
        }
        GridLayoutGroup gg = content.GetComponent<GridLayoutGroup>();
        //int pages = gg.transform.childCount % 2 == 0 ? gg.transform.childCount / 2 : gg.transform.childCount / 2 + 1;
        int pages = gg.transform.childCount;
        content.sizeDelta = new Vector2(content.sizeDelta.x, pages * (gg.cellSize.y + gg.spacing.y) + gg.padding.top + gg.padding.bottom);
    }
    public void RefreshData()
    {
        Hashtable hash = new Hashtable();
        hash["boxNumber"] = _playerCache._playerData.myBoxNum;
        hash["ids"] = _playerCache.getWeather(_playerCache._playerData.myBoxNum);
        _dataManager.GetUserWeatherParam(hash);
    }
    public void OpenCamera()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.MaxVideo, KMasklayerCenter.masklayer);
    }
}
