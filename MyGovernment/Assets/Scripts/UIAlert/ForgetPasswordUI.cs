using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgetPasswordUI : UIAlertLayer
{
    public InputField password;
    public InputField repassword;
    public InputField tele;
    public InputField yanzheng;
    public Button yanzhengBT;
    public CountDown countdown;
    public GameObject verify;
    public GameObject reset;
    protected override void Start()
    {
        base.Start();
        setMinSize();
        verify.SetActive(true);
        reset.SetActive(false);
        password.onEndEdit.AddListener(delegate (string str) { lengthlimit(str, password); });
        repassword.onEndEdit.AddListener(delegate (string str) { isSame(repassword, password); });
        tele.onEndEdit.AddListener(delegate (string str) { isPhone(str, tele);  });
        yanzheng.onEndEdit.AddListener(delegate (string str) { isNumber(str, yanzheng); });
        countdown.gameObject.SetActive(false);
        yanzhengBT.gameObject.SetActive(true);
        RegistNotification(this, kNotificationKeys.getForgetVerification, getForgetVerificationFinished);
        RegistNotification(this, kNotificationKeys.checkVerification, checkVerificationFinished);
        RegistNotification(this, kNotificationKeys.updateforgetpassword, updateforgetpasswordFinished);
    }

    private void updateforgetpasswordFinished(Hashtable obj)
    {
       // Debug.Log(obj.toJson());
        _playerCache._Data = new SaveData();
        _playerCore.saveProfile();
        _maskLayer.ShowAlertWithMSG("修改成功");
        onClose();
    }

    private void checkVerificationFinished(Hashtable obj)
    {
        Debug.Log(obj.toJson());
        verify.SetActive(false);
        reset.SetActive(true);
    }

    private void getForgetVerificationFinished(Hashtable obj)
    {
        Debug.Log(obj.toJson());
        startCountDown();
        _maskLayer.ShowAlertWithMSG("获取验证码成功");
    }
    public void back()
    {
        verify.SetActive(true);
        reset.SetActive(false);
        password.text = "";
        repassword.text = "";
    }
    public void ReSetPassword ()
    {
        if (isNull(tele))
        {
            _maskLayer.ShowAlertWithTips("手机号码不能为空");
            return;
        }
        //else if (isNull(yanzheng))
        //{
        //    _maskLayer.ShowAlertWithTips("验证码不能为空");
        //    return;
        //}
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
        hash["accountName"] = tele.text;
        hash["newPassword"] = password.text;
        hash["reNewPassword"] = repassword.text;
        hash["isForget"] = true;
        Debug.Log(hash.toJson());
        _dataManager.updateForgetPassword(hash);
    }
    public void VerifyCode()
    {
        if (isNull(tele))
        {
            _maskLayer.ShowAlertWithTips("手机号码不能为空");
            return;
        }
        if (isNull(yanzheng))
        {
            _maskLayer.ShowAlertWithTips("验证码不能为空");
            return;
        }
        Hashtable hash = new Hashtable();
        hash["phoneNum"] = tele.text;
        hash["code"] = yanzheng.text;
        Debug.Log(hash.toJson());
         _dataManager.checkVerificationCode(hash);
    }
    private void startCountDown()
    {
        yanzhengBT.gameObject.SetActive(false);
        //countdown.gameObject.SetActive(true);
        countdown.StartCountDown();
    }
    public void getVerification()
    {
        if (isNull(tele))
        {
            _maskLayer.ShowAlertWithTips("手机号码不能为空");
            return;
        }
        Hashtable hash = new Hashtable();
        hash["phoneNum"] = tele.text;
        _dataManager.getForgetVerificationCode(hash);
    }

}
