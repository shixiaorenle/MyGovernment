using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdatePasswordUI : UIAlertLayer
{
    public InputField oldpasswrod;
    public InputField password;
    public InputField repassword;

    protected override void Start()
    {
        base.Start();
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        password.onEndEdit.AddListener(delegate (string str) { lengthlimit(str,  password); });
        repassword.onEndEdit.AddListener(delegate (string str) { isSame(repassword, password); });
        oldpasswrod.onEndEdit.AddListener(delegate (string str) {  });
        RegistNotification(this, kNotificationKeys.updatepassword, updatePasswordOver);
    }

    private void updatePasswordOver(Hashtable obj)
    {
        //Debug.Log(obj.toJson());
        //_maskLayer.Container = _maskLayer.transform;

        _playerCache._Data = new SaveData();
        _playerCore.saveProfile();
        _maskLayer.ShowAlertWithMSG("修改成功");
        _maskLayer.RemoveAll();

        //onClose();
        
        SceneManager.LoadScene("Enter");
        

    }
    public void Close()
    {
       // _maskLayer.Container = _gameManager._UI2Dmanager.transform;
        onClose();
    }
    public void UpdatePassword()
    {
        
         if (isNull(oldpasswrod))
        {
            _maskLayer.ShowAlertWithTips("旧密码不能为空");
            return;
        }
        else if (isNull(password))
        {
            _maskLayer.ShowAlertWithTips("登录密码不能为空");
            return;
        }
        else if (isNull(repassword))
        {
            _maskLayer.ShowAlertWithTips("登录密码不能为空");
            return;
        }


        Hashtable hash = new Hashtable();
        hash["newPassword"] = password.text;
        hash["reNewPassword"] = repassword.text;
        hash["accountName"] = _playerCache._playerData._User.userCellPhone;
        //hash["accountName"] = tele.text;
        hash["oldPassword"] = oldpasswrod.text;
        Debug.Log(hash.toJson());
         _dataManager.updatePassword(hash);
    }
}
