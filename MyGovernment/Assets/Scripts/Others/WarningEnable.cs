using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class WarningEnable : Define
{
    private string UID;
    public Hashtable AData = new Hashtable();
    //public InputField Name;
    //public InputField Cell;
    public Text t_Cell;
    public Text t_Name;
    public Toggle isON;
    public ButtonExtension btn;//长按按钮
    void Start()
    {
        
        
        isON.onValueChanged.AddListener((bool ison) => {
            Hashtable hashtable = new Hashtable();
            hashtable["enabled"] = ison;
            hashtable["contactUid"] = UID;
            _dataManager.ChangeGetInfo(hashtable);
        });
    }

    //private void DeleteLinkmanOver(Hashtable obj)
    //{
    //    string Name = obj["name"] as string;
    //    string uid = obj["contactId"] as string;
    //    if (t_Name.text.Equals(Name)&& uid.Equals(UID))
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public void initData(Hashtable hashtable, UnityAction press)
    {
        AData = hashtable;
        if (hashtable.ContainsKey("name"))
        {
            t_Name.text = hashtable["name"] as string;
        }
        if (hashtable.ContainsKey("cellphone"))
        {
            t_Cell.text = hashtable["cellphone"] as string;
        }
        if (hashtable.ContainsKey("enabled"))
        {
            bool ison = Convert.ToBoolean(hashtable["enabled"]);
            if (ison)
            {
                isON.isOn = true;
            }
            else
            {
                isON.isOn = false;
            }
        }
        if (hashtable.ContainsKey("uid"))
        {
            UID = hashtable["uid"] as string;
        }
        btn.onPress.AddListener(press);
    }
}
