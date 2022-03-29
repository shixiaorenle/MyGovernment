using MoreMountains.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WarningEdit : UIAlertLayer
{
    private string UID;
    public RectTransform content;
    public GameObject WarningEnable;
    public Button addLinkman;
    public Text t;
    public GameObject listNull;
    public GameObject title;
    private List<string> UIDs = new List<string>();
    protected override void Start()
    {
        base.Start();
        setMinSize();
        RegistNotification(this, kNotificationKeys.changeLinkmanInfo, ChangeLinkmanInfo);
        RegistNotification(this, kNotificationKeys.getAlarmDetail, GetAlarmDetail);
        RegistNotification(this, kNotificationKeys.deleteLinkmanOver, DeleteLinkmanOver);
        RegistNotification(this, kNotificationKeys.addLinkman, AddLinkmanOver);
    }

    private void AddLinkmanOver(Hashtable obj)
    {
        _maskLayer.ShowAlertWithMSG("添加成功");
        Refresh();
    }
    private void DeleteLinkmanOver(Hashtable obj)
    {
        _maskLayer.ShowAlertWithMSG("删除成功");
        Refresh();
    }

    private void ChangeLinkmanInfo(Hashtable obj)
    {
        Debug.Log(obj.toJson());
    }
    private void GetAlarmDetail(Hashtable obj)
    {
        Hashtable hashtable = obj["data"] as Hashtable;
        //ArrayList array = h["contacts"] as ArrayList;
        string uid = hashtable["uid"] as string;
        if (uid.Equals(UID))
        {
            removeAllChild(content.gameObject);
            ArrayList arrayList = hashtable["contacts"] as ArrayList;
            if (arrayList.Count <= 0)
            {
                listNull.SetActive(true);
                title.SetActive(false);
                t.text = "报警联系人为空，请新增！";
                return;
            }
            listNull.SetActive(false);
            title.SetActive(true);
            t.text = "";
            for (int i = 0; i < arrayList.Count; i++)
            {
                Hashtable ssss = arrayList[i] as Hashtable;
                GameObject o = Instantiate(WarningEnable);
                o.transform.SetParent(content);
                o.transform.localScale = Vector3.one;
                o.transform.localPosition = Vector3.zero;
                o.GetComponent<WarningEnable>().initData(ssss,()=> { OnPress(o.GetComponent<WarningEnable>()); });
            }
            GridLayoutGroup gridLayoutGroup = content.GetComponent<GridLayoutGroup>();
            content.sizeDelta = new Vector2(content.sizeDelta.x, (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y) * arrayList.Count + gridLayoutGroup.padding.top);
        }
    }
    private void OnPress(WarningEnable warningEnable)
    {

        MMVibrationManager.Haptic(HapticTypes.Selection);
        Hashtable hashtable = warningEnable.AData;
        hashtable["groupUid"] = UID;
        hashtable["boxNumber"] = _playerCache._playerData.myBoxNum;
        _maskLayer.ShowAlertOnly(kAlertTypes.DeleteLinkman, KMasklayerCenter.masklayer, hashtable);
    }
    public override void RefreshData(Hashtable hash)
    {
        ArrayList received = new ArrayList();
        Debug.Log("报警联系人有" + hash.toJson());
        received = hash["data"] as ArrayList;
        CheckUid(received);
    }
    public void AddLinkman()
    {
        Hashtable hashtable = new Hashtable();
        hashtable["groupUid"] = UID;
        _maskLayer.ShowAlertOnly(kAlertTypes.AddLinkman, KMasklayerCenter.warning, hashtable);
    }
    public void CheckUid(ArrayList dataList)
    {
       
        if (dataList.Count<=0)
        {
            _maskLayer.ShowAlertWithTips("报警联系人为空");
            addLinkman.onClick.AddListener(delegate {
                _maskLayer.ShowAlertWithTips("请先添加报警条目");
            });
            t.text = "报警联系人为空";
            return;
        }
        t.text = "";
        addLinkman.onClick.AddListener(AddLinkman);
       Hashtable ssss = dataList[0] as Hashtable;
        Hashtable hashtable = ssss["group"] as Hashtable;
        Debug.Log("报警编辑数据是:"+ssss.toJson());
        string uid = hashtable["uid"] as string;
        for (int i = dataList.Count - 1; i >= 0; i--)
        {

            
            Hashtable s = dataList[i] as Hashtable;
            Hashtable hash = ssss["group"] as Hashtable;
            string ss = hash["uid"] as string;
            if (!ss.Equals(uid))
            {
                _maskLayer.ShowAlertWithTips("报警分组错误");
                return;
            }
        }
        UID = uid;
        Refresh();
    }
    private void Refresh()
    {
        Hashtable h = new Hashtable();
        h["boxNumber"] = _playerCache._playerData.myBoxNum;
        h["groupUid"] = UID;

        _dataManager.GetAlarmDetail(h);
    }
}
