using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseList : Define
{
    public RectTransform content;
    public GameObject HouseItem;
    public GameObject listNull;
    public GameObject title;
    public Text t;
    private   bool show = false;
    void Start()
    {
        _dataManager.GetUserGreenHouseList();
        RegistNotification(this, kNotificationKeys.getGreenHouselist, ReceivedHouseData);//刷新大棚页面
    }
    /// <summary>
    /// 刷新大棚列表
    /// </summary>
    public void RefreshCenter()
    {
        _dataManager.GetUserGreenHouseList();
    }
    private void ReceivedHouseData(Hashtable hash)
    {
        ArrayList received = new ArrayList();
        Debug.Log("大棚中心数据是" + hash.toJson());
        received = hash["data"] as ArrayList;
        int pages = received.Count;
        GridLayoutGroup gg = content.GetComponent<GridLayoutGroup>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, pages * (gg.cellSize.y + gg.spacing.y) + gg.padding.top);
        RefreshUI(received);
    }
    public void RefreshUI(ArrayList dataList)
    {
        removeAllChild(content.gameObject);
        if (dataList.Count <= 0 || dataList == null)
        {
            t.text = "大棚列表为空,请添加！";
            listNull.SetActive(true);
            title.SetActive(false);
            return;
        }
        else
        {
            listNull.SetActive(false);
            title.SetActive(true);
            t.text = "";

            for (int i = 0; i < dataList.Count; i++)
            {
                Hashtable ssss = dataList[i] as Hashtable;
                GameObject item = Instantiate(HouseItem);
                item.transform.SetParent(content);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.GetComponent<HouseItem>().initData(ssss, () =>
                {
                    ClickDetail(item.GetComponent<HouseItem>());
                }, () => {

                    PressDetail(item.GetComponent<HouseItem>());
                });

            }
        }

    }
    public void ClickDetail(HouseItem houseitem)
    {
        _playerCache._playerData.myData = houseitem.AData;
        _playerCache._playerData.myBoxNum = houseitem.boxNumber;
        _gameManager._PCUIManager.Showlist(false);
        _gameManager._PCUIManager.houseMain.OpenControl(houseitem.AData);
        //onClose();
    }
    public void PressDetail(HouseItem houseitem)
    {

        _maskLayer.ShowAlertOnly(kAlertTypes.DeleteHouse, KMasklayerCenter.masklayer, houseitem.AData);
    }
    public void AddGreenHouse()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.AddGreenHouse);
    }
    void Update()
    {
        
    }
}
