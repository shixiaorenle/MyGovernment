
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
public class Center : UIAlertLayer
{

    private ArrayList myList;
    public RectTransform content;
    public GameObject HouseItem;
    public Text t;
    public GameObject web;
    public GameObject listNull;
    public GameObject title;
    GameObject houseUI = null;
    //void Update()
    //{
    //    //轮询获取报警（现在不用了，因为有短信提醒）
    //    //time += Time.deltaTime;
    //    //if (time >= 10)
    //    //{
    //    //    GetAlarm();

    //    //    time = 0;
    //    //}
    //}
    protected override void Start()
    {
        base.Start();

        setLeftize();
        RegistNotification(this, kNotificationKeys.realTimeAlarm, GetAlarmFinish);//获取报警
        
    }
    /// <summary>
    /// 刷新大棚列表
    /// </summary>
    public void RefreshCenter()
    {
        _dataManager.GetUserGreenHouseList();
    }
    private void GetAlarm()
    {
        if (myList != null)
        {
            for (int i = 0; i < myList.Count; i++)
            {
                Hashtable hash = myList[i] as Hashtable;
                string boxNum = hash["archNum"].ToString();
                Hashtable h = new Hashtable();
                h["boxNumber"] = boxNum;
                _dataManager.RealTimeAlarm(h);
            }
        }
    }
    private void GetAlarmFinish(Hashtable obj)
    {
        Debug.Log("报警数据：" + obj.toJson());
    }
    public void AddGreenHouse()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.AddGreenHouse);
    }
    public override void RefreshData(Hashtable hash)
    {        
        ArrayList received = new ArrayList();
        Debug.Log("大棚中心数据是" + hash.toJson());
        received = hash["data"] as ArrayList;
        myList = received;
        int pages = received.Count;
        GridLayoutGroup gg = content.GetComponent<GridLayoutGroup>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, pages * (gg.cellSize.y + gg.spacing.y) + gg.padding.top);
        RefreshUI(received);
    }
    public void RefreshUI(ArrayList dataList)
    {
        removeAllChild(content.gameObject);
        if (dataList.Count<=0||dataList==null)
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

        _gameManager._UI2Dmanager.OpenControl();
        onClose();
    }
    public void PressDetail(HouseItem houseitem)
    {

        MMVibrationManager.Haptic(HapticTypes.Selection);
        _maskLayer.ShowAlertOnly(kAlertTypes.DeleteHouse,KMasklayerCenter.masklayer, houseitem.AData);
    }
    public void Call()
    {
        string phoneNumber = _playerCache._playerData.cellPhone;
        Application.OpenURL("tel://" + phoneNumber);
    }
    public void OpenWeb()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.WebUI, KMasklayerCenter.none);
    }
    public void UpdatePassword()
    {
        // _maskLayer.Container = _maskLayer.transform;
        _maskLayer.ShowAlertOnly(kAlertTypes.UpdatePassword, KMasklayerCenter.masklayer);
    }
    public void Guide()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.GuideUI, KMasklayerCenter.masklayer);
    }
    public void ResLog()
    {
        _maskLayer.Container = _maskLayer.transform;

        _playerCache._Data = new SaveData();
        _playerCore.saveProfile();
        _maskLayer.Windows.Clear();
        
        SceneManager.LoadScene("Enter");
        onClose();
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
