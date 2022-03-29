using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningList : UIAlertLayer
{

    public RectTransform content;
    public GameObject WarningListItem;
    public Text t;
    private float refreshTime;
    public float refreshTimeLimit = 60;
    long SerchBeginTime = 0;
    long SerchEndTime = 0;
    protected override void Start()
    {
        base.Start();
        setMinSize();
        RegistNotification(this, kNotificationKeys.getAlarmHistory, ReceivedAlarm);
        RegistNotification(this, kNotificationKeys.getAlarmList, GetAlarmList);
        RegistNotification(this, kNotificationKeys.confirmAlarm, ConfirmAlarm);
        //history.onClick.AddListener(delegate {


        //});
    }

    private void ConfirmAlarm(Hashtable obj)
    {
        RefreshAlarm();
    }

    public void OnClickHistory()
    {
        Hashtable h = new Hashtable();

        h["boxNumber"] = _playerCache._playerData.myBoxNum;
        SerchEndTime = ConvertStringToTimeTamp(DateTime.Now);
        SerchBeginTime = ConvertStringToTimeTamp(DateTime.Now.AddYears(-1));
        while (SerchEndTime < 1000000000000 && SerchEndTime != 0)
        {
            SerchEndTime *= 10;
        }
        while (SerchBeginTime < 1000000000000 && SerchBeginTime != 0)
        {
            SerchBeginTime *= 10;
        }
        h["beginTimeTamp"] = SerchBeginTime;
        h["endTimeTamp"] = SerchEndTime;

        // Debug.Log(h.toJson());
        _dataManager.GetAlarmHistory(h);
    }
    public void OnClickEdit()
    {
        Hashtable h = new Hashtable();
        h["boxNumber"] = _playerCache._playerData.myBoxNum;
        _dataManager.GetAlarmList(h);
        
    }
    private void GetAlarmList(Hashtable obj)
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.WarningEdit, KMasklayerCenter.warning, obj);
    }
    private void ReceivedAlarm(Hashtable obj)
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.WarningHistory, KMasklayerCenter.warning, obj);
    }
    public void RefreshAlarm()
    {
        Hashtable h = new Hashtable();
        h["boxNumber"] = _playerCache._playerData.myBoxNum;

        _dataManager.GetAlarm(h);
    }

    public override void RefreshData(Hashtable hash)
    {
        ArrayList received = new ArrayList();
        Debug.Log("报警中心数据是" + hash.toJson());
        received = hash["data"] as ArrayList;
        //for (int i = 0; i < received.Count; i++)
        //{
        //    Debug.Log(received[i]);
        //}

        int pages = received.Count;
        GridLayoutGroup gridLayoutGroup = content.GetComponent<GridLayoutGroup>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, pages * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y) + gridLayoutGroup.padding.top);
        RefreshUI(received);

    }

    public void RefreshUI(ArrayList dataList)
    {

        removeAllChild(content.gameObject);
        if (dataList.Count <= 0 || dataList == null)
        {
            t.text = "报警条目为空";
            return;
        }
        else
        {
            t.text = "";
            for (int i = 0; i < dataList.Count; i++)
            {
                Hashtable ssss = dataList[i] as Hashtable;
                GameObject item = Instantiate(WarningListItem);
                item.transform.SetParent(content);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                ssss["index"] = i;
                item.GetComponent<WarningListItem>().initData(ssss, () =>
                {
                    ClickDetail(item.GetComponent<WarningListItem>());
                });

            }
        }
        
    }
    public void ClickDetail(WarningListItem warningitem)
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.WarningConfirm, KMasklayerCenter.warning, warningitem.AData);
    }
    protected override void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            refreshTime += Time.deltaTime;
            if (refreshTime >= refreshTimeLimit)
            {
                RefreshAlarm();
                refreshTime = 0;
            }
        }
    }
}
