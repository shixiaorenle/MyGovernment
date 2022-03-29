using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RecordItem : Define
{
    public Hashtable AData = new Hashtable();
    public Text operation;
    public Text longdate;
    public Text longtime;
    public Text houseid;
    public Button button;
    public GameObject emergency;
    public void initData(Hashtable hash, UnityAction fuction)
    {
        AData = hash;

        //Debug.Log(AData.toJson());
        string itemId = AData["itemId"].ToString();
        //Debug.Log(_playerCache.GetPointName(itemId));
        string boxNum = AData["boxNumber"].ToString();
        Hashtable hashtable = AData["itemInfo"] as Hashtable;
        
        string type = hashtable["type"].ToString();
        //string point = hashtable["point"].ToString();
        if (type != null && type != "null")
        {
            if (type.Equals("control"))
            {
                //operation.text = _playerCache.GetPointName(itemId);
                operation.text = hashtable["point"].ToString();
                emergency.SetActive(false);
            }
            else if (type.Equals("emergency"))
            {
                operation.text = _playerCache.GetPointName(itemId);

                emergency.SetActive(true);
            }
            else if (type.Equals("parameter"))

            {
                // operation.text = _playerCache.GetParameterName(itemId);
                operation.text = hashtable["point"].ToString();
                emergency.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        string timetamp = AData["createTime"].ToString();
        longdate.text = ConvertStringToDateTime(timetamp).Year.ToString() +"年"+ ConvertStringToDateTime(timetamp).Month.ToString() +"月"+ ConvertStringToDateTime(timetamp).Day.ToString()+"日";
        longtime.text = ConvertStringToDateTime(timetamp).ToLongTimeString();
        houseid.text = AData["id"].ToString();
        //Debug.Log(houseid.text+ type);
        button.onClick.AddListener(fuction);
    }
}
