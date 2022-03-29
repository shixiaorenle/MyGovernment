using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopUI : UIAlertLayer
{

    protected override void Start()
    {
        base.Start();
        setMinSize();
        RegistNotification(this, kNotificationKeys.controlAllDeviceOne, ControlAllDeviceOne);
        RegistNotification(this, kNotificationKeys.controlAllDeviceTwo, ControlAllDeviceTwo);
    }

    private void ControlAllDeviceOne(Hashtable obj)
    {
        foreach (var item in _playerCache._playerData.mQTPoints)
        {
            Debug.Log(item.Key);

            Hashtable hashtable = new Hashtable();
            hashtable["boxNumber"] = item.Key;
            //hashtable["itemIds"] = "";
            ArrayList arrayList = new ArrayList();
            Dictionary<string, string> dic = item.Value;
            foreach (var pair in dic)
            {
                arrayList.Add(pair.Value);
                //hashtable["itemIds"] = arrayList;
                Debug.Log(pair.Key + pair.Value);
            }
            hashtable["itemIds"] = arrayList;
            Hashtable h = new Hashtable();
            h["type"] = "emergency";
            h["infos"] = new ArrayList();
            hashtable["info"] = h;
            hashtable["value"] = 0;
            Debug.Log(hashtable.toJson());
            _dataManager.ControlAllDeviceTwo(hashtable);
        }
       // onClose();
    }
    private void ControlAllDeviceTwo(Hashtable obj)
    {
        Debug.Log(obj.toJson());
        onClose();
    }
    public void OnClickSure()
    {

        foreach (var item in _playerCache._playerData.mStopPoints)
        {
            Debug.Log(item.Key);

            Hashtable hashtable = new Hashtable();
            hashtable["boxNumber"] = item.Key;
            //hashtable["itemIds"] = "";
            ArrayList arrayList = new ArrayList();
            Dictionary<string, string> dic = item.Value;
            foreach (var pair in dic)
            {
                arrayList.Add(pair.Value);
                //hashtable["itemIds"] = arrayList;
                Debug.Log(pair.Key + pair.Value);
            }
            hashtable["itemIds"] = arrayList;
            Hashtable h = new Hashtable();
            h["type"] = "emergency";
            h["infos"] = new ArrayList();
            hashtable["info"] = h;
            hashtable["value"] = 1;
            Debug.Log(hashtable.toJson());
            _dataManager.ControlAllDeviceOne(hashtable);
        }

    }

}
