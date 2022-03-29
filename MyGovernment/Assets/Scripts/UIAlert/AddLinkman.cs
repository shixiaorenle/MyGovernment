using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddLinkman : UIAlertLayer
{
    public InputField Name;
    public InputField Cell;
    private string uid;
    protected override void Start()
    {
        base.Start();
        setMinSize();
        Cell.onEndEdit.AddListener(delegate (string str) { isPhone(str, Cell); });
        Name.onEndEdit.AddListener(delegate (string str) { containsSpace(str, Name); });
    }
    public override void RefreshData(Hashtable hashtable)
    {
        Debug.Log("新增联系人数据是:" + hashtable.toJson());
        if (hashtable.ContainsKey("groupUid"))
        {
            uid = hashtable["groupUid"] as string;
        }
    }
    public void Add()
    {
        if (isNull(Name))
        {
            _maskLayer.ShowAlertWithTips("联系人姓名不能为空");
            return;
        }
        else if (isNull(Cell))
        {
            _maskLayer.ShowAlertWithTips("电话号码不能为空");
            return;
        }
        Hashtable hashtable = new Hashtable();
        hashtable["boxNumber"] = _playerCache._playerData.myBoxNum;
        hashtable["groupUid"] = uid;
        hashtable["name"] = Name.text ;
        hashtable["cellphone"] = Cell.text;
        Debug.Log(hashtable.toJson());
        _dataManager.AddLinkman(hashtable);
        onClose();
    }

}
