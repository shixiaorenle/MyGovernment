using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Record : UIAlertLayer
{
    public RectTransform content;
    public GameObject RecordItem;
    public Dropdown dropdown;
    public Text t;
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
        RegistNotification(this,kNotificationKeys.selectDate, SelectDate);
        DateTime nowMonth = DateTime.Now;
        DateTime lastYearMonth;
        dropdown.ClearOptions();
        //Dropdown.OptionData optionData = new Dropdown.OptionData();
        //optionData.text = "全部记录";
        //optionDatas.Add(optionData);

        for (int i = 0; i < maxMonthNum; i++)
        {
            lastYearMonth = nowMonth.AddMonths(-i);

            Dropdown.OptionData optionData = new Dropdown.OptionData();
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

        kAlertTypes kAlertTypes =ParseEnum<kAlertTypes>( obj["type"].ToString());
        if (kAlertTypes.Equals(Type))
        {
            string date = obj["date"].ToString();
            Hashtable h = new Hashtable();
            if (date.Equals(""))
            {
                
                h["boxNumber"] = _playerCache._playerData.myBoxNum;
                dateText.text = "所有记录";
            }
            else
            {
                DateTime dateTime;
                if (mStringDic.TryGetValue(date, out dateTime))
                {
                    DateTime firstDay = new DateTime(dateTime.Year, dateTime.Month, 1);
                    DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                    h["createTimeS"] = firstDay.ToString("yyyy-MM-dd");
                    h["createTimeE"] = lastDay.ToString("yyyy-MM-dd");
                    h["boxNumber"] = _playerCache._playerData.myBoxNum;
                    dateText.text = date;
                }
            }
            _dataManager.GetRecord(h);
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
        }
        else
        {
            DateTime dateTime;
            if (mDic.TryGetValue(option, out dateTime))
            {
                DateTime firstDay = new DateTime(dateTime.Year, dateTime.Month, 1);
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
                h["createTimeS"] = firstDay.ToString("yyyy-MM-dd");
                h["createTimeE"] = lastDay.ToString("yyyy-MM-dd");
                h["boxNumber"] = _playerCache._playerData.myBoxNum;
            }

            
        }
        _dataManager.GetRecord(h);
    }

    public override  void RefreshData(Hashtable hash)
    {

        ArrayList received = new ArrayList();
        //Debug.Log("报警中心数据是" + hash.toJson());
        received = hash["data"] as ArrayList;

        //for (int i = 0; i < received.Count; i++)
        //{
        //    Hashtable hashtable = received[i] as Hashtable;
        //    //Debug.Log(hashtable.toJson());
        //    if (i.Equals(0))
        //    {
        //        Hashtable hh = hashtable["itemInfo"] as Hashtable;
        //        if (hh.ContainsKey("infos"))
        //        {
        //            ArrayList array = hh["infos"] as ArrayList;
        //            foreach (var item in array)
        //            {
        //                Hashtable h = item as Hashtable;
        //                //Debug.Log(h["name"].ToString());
        //            }
        //        }
                
        //    }                        
        //}

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
            t.text = "操作记录为空";
            return;
        }
        else
        {
            t.text = "";
            for (int i = dataList.Count - 1; i >= 0; i--)
            {
                Hashtable ssss = dataList[i] as Hashtable;
                GameObject item = Instantiate(RecordItem);
                item.transform.SetParent(content);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.GetComponent<RecordItem>().initData(ssss, () =>
                {
                    ClickDetail(item.GetComponent<RecordItem>());
                });

            }
        }
            
    }
    public void ClickDetail(RecordItem recorditem)
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.RecordUI, KMasklayerCenter.record, recorditem.AData);
    }
    public void RefreshRecord()
    {
        Hashtable h = new Hashtable();
        h["boxNumber"] = _playerCache._playerData.myBoxNum;
        _dataManager.GetRecord(h);
    }
    public void ResetSearch()
    {
        //SerializableDate emtpy = new SerializableDate();
        //Time_module.SelectedDate = emtpy;

        Hashtable h = new Hashtable();
        h["boxNumber"] = _playerCache._playerData.myBoxNum;
        _dataManager.GetRecord(h);

    }
    public void Search()
    {
      
        
        Debug.Log(DateTime.Today.ToString("yyyy-MM-dd"));
        Hashtable hashtable = new Hashtable();
        //hashtable["createTimeS"] = Time_module.VisibleDate.ToDateString();
        hashtable["createTimeE"] = DateTime.Today.ToString("yyyy-MM-dd");

        hashtable["boxNumber"] = _playerCache._playerData.myBoxNum;
        Debug.Log(hashtable.toJson());
        _dataManager.GetRecord(hashtable);
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
        _maskLayer.ShowAlertOnly(kAlertTypes.Date, KMasklayerCenter.masklayer,hashtable);
    }
}
