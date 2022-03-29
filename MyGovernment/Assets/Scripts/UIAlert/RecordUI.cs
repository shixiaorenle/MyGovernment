using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordUI : UIAlertLayer
{
    public Text longdate;
    public Text longtime;
    public Text boxnum;
    public Text boxname;
    public Text operation;
    public RectTransform content;
    public GameObject parameter;
    public GameObject emergency;
    public Text newValue;
    public Text oldValue;
    public Text t;


    public Text tem1;
   // public Text tem2;
    public Text humidity;
    public Text soilTem;
    public Text soilHum;
    public Text CO2;
    public Text lightStrength;
    public Text N;
    public Text P;
    public Text K;
    public Text ec;
    public Text ph;
    protected override void Start()
    {
        base.Start();
        setMinSize();
        //InitData();
    }

    public override void RefreshData(Hashtable hashtable)
    {
        Debug.Log(hashtable.toJson());        
        GridLayoutGroup group = content.GetComponent<GridLayoutGroup>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, (content.childCount % 2 == 0 ? content.childCount / 2 : (content.childCount / 2 + 1)) * (group.cellSize.y + group.spacing.y) + group.padding.top + group.padding.bottom);

        string boxNumber = hashtable["boxNumber"].ToString();
        boxnum.text = boxNumber;
        string xx = _playerCache.GetHouseName(boxNumber);
        if (xx.Equals(null)||xx.Equals(""))
        {
            _maskLayer.ShowAlertWithTips("只能查看已绑定大棚的操作记录");
            onClose();
            return;
        }
        boxname.text = _playerCache.GetHouseName(boxNumber);

        string itemId = hashtable["itemId"].ToString();

        Hashtable hash = hashtable["itemInfo"] as Hashtable;
        //string boxName = hash["point"].ToString();


        string type = hash["type"].ToString();
        
        if (type!=null&&type!="null")
        {
            if (type.Equals("control"))//普通控制
            {
                t.text = "执行操作：";
                // operation.text = _playerCache.GetPointName(itemId);
                operation.text = hash["point"].ToString();
                parameter.SetActive(false);
                emergency.SetActive(false);
                content.gameObject.SetActive(true);
            }
            else if (type.Equals("emergency"))//紧急制动
            {
                t.text = "制动点位：";
                 operation.text = _playerCache.GetPointName(itemId);
                //operation.text = point;
                parameter.SetActive(false);
                emergency.SetActive(true);
                content.gameObject.SetActive(false);
            }
            else//设置时间
            {
                t.text = "执行操作：";
                // operation.text = _playerCache.GetParameterName(itemId);
                operation.text = hash["point"].ToString();
                parameter.SetActive(true);
                //string point = hash["point"].ToString();
                //if (point.Contains("juanmo"))
                //{
                //    int value = (int)Convert.ToDouble(hashtable["itemValue"].ToString()); 
                //    newValue.text = (value/10).ToString();
                //}
                //else
                //{
                //    newValue.text = hashtable["itemValue"].ToString();
                //}
                int value = (int)Convert.ToDouble(hashtable["itemValue"].ToString());
                newValue.text = (value / 10).ToString();
                oldValue.text = hashtable["itemOldValue"].ToString();
                emergency.SetActive(false);
                content.gameObject.SetActive(true);
            }
            
        }
        ArrayList arrayList = hash["infos"] as ArrayList;
        foreach (var item in arrayList)
        {
            Hashtable h = item as Hashtable;
            string str = h["name"].ToString();
            //Debug.Log(str);
            if (str.Equals("空气温度1"))
            {
                tem1.text = h["value"].ToString();
            }
            else if (str.Equals("空气温度2"))
            {
                //tem2.text = h["value"].ToString();
            }
            else if (str.Equals("空气湿度"))
            {
                humidity.text = h["value"].ToString();
            }
            else if (str.Equals("土壤湿度"))
            {
                soilHum.text = h["value"].ToString();
            }
            else if (str.Equals("土壤温度"))
            {
                soilTem.text = h["value"].ToString();
            }
            else if (str.Equals("CO2浓度"))
            {
                CO2.text = h["value"].ToString();
            }
            else if (str.Equals("光照强度"))
            {
                lightStrength.text = h["value"].ToString();
            }
            else if (str.Equals("氮浓度"))
            {
                N.text = h["value"].ToString();
            }
            else if (str.Equals("钾浓度"))
            {
                K.text = h["value"].ToString();
            }
            else if (str.Equals("磷浓度"))
            {
                P.text = h["value"].ToString();
            }
            else if (str.Equals("电导率"))
            {
                ec.text = h["value"].ToString();
            }
            else if (str.Equals("土壤PH"))
            {
                ph.text = h["value"].ToString();
            }
        }
        string timetamp = hashtable["createTime"].ToString();
        longdate.text = ConvertStringToDateTime(timetamp).Year.ToString() + "年" + ConvertStringToDateTime(timetamp).Month.ToString() + "月" + ConvertStringToDateTime(timetamp).Day.ToString() + "日";
        longtime.text = ConvertStringToDateTime(timetamp).ToLongTimeString();
    }
}
