using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DeleteHouse : UIAlertLayer
{
    public Text title;
    string id;
    protected override void Start()
    {
        base.Start();
        setMinSize();
        Debug.Log(Param.toJson());
        //InitTitle();
    }
    public override void RefreshData(Hashtable hashtable)
    {
        Debug.Log(hashtable);
        if (hashtable.ContainsKey("id"))
        {
            id = Param["id"].ToString();
            title.text = "是否删除" + id + "号大棚？";
        }
        else
        {
            onClose();
        }
    }
    private void InitTitle()
    {
        if (Param.ContainsKey("id"))
        {
            id = Param["id"].ToString();
            title.text = "是否删除" + id + "号大棚？";
        }
        else
        {
            onClose();
        }
    }
    public void OnClickDelete()
    {
        Hashtable hash = new Hashtable();
        hash["id"] = id;
        _dataManager.deleteGreenHouse(hash);
        onClose();
    }
}
