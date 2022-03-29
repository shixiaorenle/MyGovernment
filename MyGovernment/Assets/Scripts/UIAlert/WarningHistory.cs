using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningHistory : UIAlertLayer
{
    public RectTransform content;
    public GameObject WarningItem;
    public Dropdown dropdown;
   // public DatePicker Time_module;
    public Text t;
    long SerchBeginTime = 0;
    long SerchEndTime = 0;
    public int maxMonthNum;
    public RectTransform item;
    public RectTransform dropdownContent;
    public Text dateText;
    private List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
    private Dictionary<Dropdown.OptionData, DateTime> mDic = new Dictionary<Dropdown.OptionData, DateTime>();//下拉框数据
    private Dictionary<string, DateTime> mStringDic = new Dictionary<string, DateTime>();//string类数据
    protected override void Start()
    {
        base.Start();
        setMinSize();
        RegistNotification(this, kNotificationKeys.refreshAlarmHistory, RefreshAlarmHistory);
        RegistNotification(this, kNotificationKeys.confirmAlarm, ConfirmAlarm);
        RegistNotification(this, kNotificationKeys.selectDate, SelectDate);
        DateTime nowMonth = DateTime.Now;
        DateTime lastYearMonth;
        dropdown.ClearOptions();
        Dropdown.OptionData optionData = new Dropdown.OptionData();
        optionData.text = "全部记录";
        optionDatas.Add(optionData);

        for (int i = 0; i < maxMonthNum; i++)
        {
            lastYearMonth = nowMonth.AddMonths(-i);

            optionData = new Dropdown.OptionData();
            optionData.text = lastYearMonth.Year.ToString() + "/" + lastYearMonth.Month.ToString();
            optionDatas.Add(optionData);
            mDic.Add(optionData, lastYearMonth);
        }
        dropdown.AddOptions(optionDatas);

        dropdown.onValueChanged.AddListener(ChangeDropDown);
    }
    /// <summary>
    /// 自己插件筛选
    /// </summary>
    /// <param name="obj"></param>
    private void SelectDate(Hashtable obj)
    {
        kAlertTypes kAlertTypes = ParseEnum<kAlertTypes>(obj["type"].ToString());
        if (kAlertTypes.Equals(Type))
        {
            string date = obj["date"].ToString();
            if (date.Equals(""))
            {
                dateText.text = "所有历史";
                SerchEndTime = ConvertStringToTimeTamp(DateTime.Now);
                SerchBeginTime = ConvertStringToTimeTamp(DateTime.Now.AddYears(-1));
            }
            else
            {
                DateTime dateTime;
                if (mStringDic.TryGetValue(date, out dateTime))
                {
                    DateTime firstDay = new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
                    DateTime lastDay = firstDay.AddMonths(1).AddSeconds(-1);
                    Debug.Log(DateTime.Today.ToString("yyyy-MM-dd"));
                    SerchBeginTime = ConvertStringToTimeTamp(firstDay);
                    SerchEndTime = ConvertStringToTimeTamp(lastDay);
                    dateText.text = date;
                }
            }
            GetAlarm(SerchEndTime, SerchBeginTime);
        }
        
    }
    /// <summary>
    /// 下拉框筛选
    /// </summary>
    /// <param name="arg0"></param>
    private void ChangeDropDown(int arg0)
    {

        Dropdown.OptionData option = optionDatas[arg0];
        string value = option.text;
        Hashtable h = new Hashtable();
        if (value.Equals("全部记录"))
        {
            h["boxNumber"] = _playerCache._playerData.myBoxNum;
            SerchEndTime = ConvertStringToTimeTamp(DateTime.Now);
            SerchBeginTime = ConvertStringToTimeTamp(DateTime.Now.AddYears(-1));
            Debug.Log(DateTime.Now);
        }
        else
        {
            DateTime dateTime;
            if (mDic.TryGetValue(option, out dateTime))
            {
                DateTime firstDay = new DateTime(dateTime.Year, dateTime.Month, 1,0,0,0);
                DateTime lastDay =firstDay.AddMonths(1).AddSeconds(-1);
                Debug.Log(DateTime.Today.ToString("yyyy-MM-dd"));
                SerchBeginTime = ConvertStringToTimeTamp( firstDay);
                SerchEndTime = ConvertStringToTimeTamp(lastDay);
            }


        }
        GetAlarm(SerchEndTime, SerchBeginTime);
        // _dataManager.RefreshAlarmHistory(h);
    }

    private void ConfirmAlarm(Hashtable obj)
    {
        Refresh();
    }

    private void RefreshAlarmHistory(Hashtable obj)
    {
        //Debug.Log(obj.toJson());
        _maskLayer.ShowAlertWithMSG("查询成功");
        RefreshData(obj);
    }

    public void Refresh()
    {
        dropdown.value = 0;
        SerchEndTime = ConvertStringToTimeTamp(DateTime.Now);
        SerchBeginTime = ConvertStringToTimeTamp(DateTime.Now.AddYears(-1));
        GetAlarm(SerchEndTime, SerchBeginTime);
    }

    private void GetAlarm(long SerchEndTime,long SerchBeginTime)
    {
        Hashtable h = new Hashtable();
        while (SerchEndTime < 1000000000000 && SerchEndTime != 0)
        {
            SerchEndTime *= 10;
        }
        while (SerchBeginTime < 1000000000000 && SerchBeginTime != 0)
        {
            SerchBeginTime *= 10;
        }
        h["boxNumber"] = _playerCache._playerData.myBoxNum;
        h["beginTimeTamp"] = SerchBeginTime;
        h["endTimeTamp"] = SerchEndTime;
        Debug.Log(h.toJson());
        _dataManager.RefreshAlarmHistory(h);
    }
    public override void RefreshData(Hashtable hash)
    {
        ArrayList received = new ArrayList();
        Debug.Log("报警中心数据是" + hash.toJson());
        received = hash["data"] as ArrayList;

        int pages = received.Count;
        GridLayoutGroup gridLayoutGroup = content.GetComponent<GridLayoutGroup>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, pages * (gridLayoutGroup.cellSize.y+ gridLayoutGroup.spacing.y) + gridLayoutGroup.padding.top);
        RefreshUI(received);
    }

    public void RefreshUI(ArrayList dataList)
    {
        removeAllChild(content.gameObject);

        if (dataList.Count <= 0 || dataList == null)
        {
            t.text = "报警历史为空";
            return;
        }
        else
        {
            t.text = "";
            for (int i = dataList.Count - 1; i >= 0; i--)
            {
                Hashtable ssss = dataList[i] as Hashtable;
                GameObject item = Instantiate(WarningItem);
                item.transform.SetParent(content);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                ssss["index"] = i;
                item.GetComponent<WarningItem>().initData(ssss, () =>
                {
                    ClickDetail(item.GetComponent<WarningItem>());
                });

            }
        }
       
    }
    public void ClickDetail(WarningItem warningitem)
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.WarningDetail, KMasklayerCenter.warning, warningitem.AData);
    }
    public void SelectDate()
    {
        DateTime nowMonth = DateTime.Now;
        DateTime lastYearMonth;
        ArrayList arrayList = new ArrayList();
        mStringDic.Clear();
        for (int i = 0; i < maxMonthNum; i++)
        {
            lastYearMonth = nowMonth.AddMonths(-i);

            Hashtable hash = new Hashtable();
            string str = lastYearMonth.Year.ToString() + "/" + lastYearMonth.Month.ToString();
            mStringDic.Add(str, lastYearMonth);
            hash["date"] = str;
            if (dateText.text.Equals(str))
            {
                hash["select"] = true;
            }
            else
            {
                hash["select"] = false;
            }

            arrayList.Add(hash);
        }
        Hashtable hashtable = new Hashtable();
        hashtable["type"] = Type.ToString();
        hashtable["dates"] = arrayList;
        _maskLayer.ShowAlertOnly(kAlertTypes.Date, KMasklayerCenter.masklayer, hashtable);
    }
}
