using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIAlertLayer :Define
{
	public kAlertTypes Type = kAlertTypes.none;
	public Hashtable Param;
	[HideInInspector]
	public bool isStateBarAlert = false;
	[HideInInspector]
	public bool isQueue = false;

	void Awake()
	{

	}

    protected virtual void Start () 
	{
        RegistNotification(this, kNotificationKeys.androidReturn, AndroidReturn);
	}

	protected virtual void Update ()
    {
       

    }
    public void setLeftize()
    {
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 200);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(-1520, -270);
        //gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 204);
        //gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, -426);
    }
    public void setMinSize()
    {
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        //gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 204);
        //gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, -426);
    }
    public void setMidSize()
    {
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, -426);
    }
    public virtual void RefreshData(Hashtable hashtable)
    {

    }
	public virtual void onClose()
	{


        _maskLayer.RemoveAlert (gameObject);

	}
    public virtual void AndroidReturn(Hashtable hashtable)
    {
        
        //_playerCache._playerData.alertTypes.RemoveAt(_playerCache._playerData.alertTypes.Count);
        string nnn = hashtable["name"].ToString();
        if (nnn.Equals(name))
        {
            Debug.Log(nnn);
            onClose();
        }

    }
	
}
