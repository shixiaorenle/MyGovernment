using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateToggle : Define
{
    public Text text;
    public Toggle toggle;
    void Start()
    {
        
    }
    public void InitData(Hashtable hash,ToggleGroup toggleGroup)
    {
        Debug.Log(hash.toJson());
        text.text = hash["date"].ToString();

        

        bool select = (bool)hash["select"];
        Debug.Log(select);
        if (select)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        toggle.group = toggleGroup;
    }
}
