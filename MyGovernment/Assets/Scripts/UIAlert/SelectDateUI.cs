using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SelectDateUI : UIAlertLayer
{
    public RectTransform content;
    public ToggleGroup toggleGroup;
    public GameObject dateToggle;
    public RectTransform main;
    private kAlertTypes dateType = kAlertTypes.none;
    protected override void Start()
    {
        base.Start();
        setMinSize();
    }

    public override void RefreshData(Hashtable hashtable)
    {
        main.DOLocalMoveY(main.transform.localPosition.y + main.rect.height, 0.3f).SetEase(Ease.OutCirc);
        ArrayList received = new ArrayList();
        dateType =ParseEnum<kAlertTypes>( hashtable["type"].ToString());
        received = hashtable["dates"] as ArrayList;
        RefreshUI(received);
    }
    public void RefreshUI(ArrayList dataList)
    {
        removeAllChild(content.gameObject);
        if (dataList.Count <= 0 || dataList == null)
        {
            _maskLayer.ShowAlertWithTips("出错了，请重新搜索");
            onClose();
            return;
        }
        else
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                Hashtable ssss = dataList[i] as Hashtable;
                GameObject item = Instantiate(dateToggle);
                item.transform.SetParent(content);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.GetComponent<DateToggle>().InitData(ssss, toggleGroup);
            }
            GridLayoutGroup gg = content.GetComponent<GridLayoutGroup>();
            content.sizeDelta = new Vector2(content.sizeDelta.x, gg.padding.top + gg.padding.bottom + (dataList.Count % 3 == 0 ? dataList.Count / 3 : dataList.Count / 3 + 1) * (gg.cellSize.y + gg.spacing.y));

        }
    }

    public void Search()
    {
        Hashtable hashtable = new Hashtable();
        string str = "";
        for (int i = 0; i < content.childCount; i++)
        {
            DateToggle dateToggle = content.GetChild(i).GetComponent<DateToggle>();
            if (dateToggle.toggle.isOn)
            {
                 str = dateToggle.text.text;
                break;
            }
        }
        hashtable["date"] = str;
        hashtable["type"] = dateType.ToString();
        PostNotification(kNotificationKeys.selectDate, hashtable);
        onClose();
    }
    public void All()
    {
        Hashtable hashtable = new Hashtable();
        string str = "";
        hashtable["date"] = str;
        hashtable["type"] = dateType.ToString();
        PostNotification(kNotificationKeys.selectDate, hashtable);
        onClose();
    }
    public override void onClose()
    {
        main.DOLocalMoveY(main.transform.localPosition.y - main.rect.height, 0.3f).SetEase(Ease.OutCirc).OnComplete(delegate { base.onClose(); });
        
    }
}
