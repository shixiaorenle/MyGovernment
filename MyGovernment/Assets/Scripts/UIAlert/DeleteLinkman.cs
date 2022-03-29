using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteLinkman : UIAlertLayer
{
    public Text title;
    string Name;
    string contactId;
    string groupUid;
    string boxNum;
    protected override void Start()
    {
        base.Start();
        setMinSize();
        RegistNotification(this, kNotificationKeys.deleteLinkman, DeleteLinkMan);
    }
    private void DeleteLinkMan(Hashtable obj)
    {
        //Debug.Log(obj.toJson());
        Hashtable hashtable = new Hashtable();
        hashtable["name"] = Name;
        hashtable["contactId"] = contactId;
        _maskLayer.ShowAlertWithMSG("删除成功");
        PostNotification(kNotificationKeys.deleteLinkmanOver, hashtable);
        onClose();
    }
    public override void RefreshData(Hashtable hashtable)
    {
        contactId = hashtable["uid"] as string;
        groupUid = hashtable["groupUid"] as string;
        boxNum = hashtable["boxNumber"] as string;
        if (hashtable.ContainsKey("name"))
        {
            Name = Param["name"].ToString();
            title.text = "是否删除联系人" + Name + "?";
        }
        else
        {
            onClose();
        }
    }
    public void OnClickDelete()
    {
        Hashtable hash = new Hashtable();
        hash["boxNumber"] = boxNum;
        hash["groupUid"] = groupUid;
        hash["contactUid"] = contactId;
        _dataManager.DeleteLinkman(hash);
       // 
    }
}
