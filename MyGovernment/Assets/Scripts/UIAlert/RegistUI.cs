using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RegistUI : UIAlertLayer
{
    public InputField M_name;
    public InputField password;
    public InputField repassword;
    public InputField tele;
    public InputField yanzheng;


    public Button yanzhengBT;
    public CountDown countdown;
    protected override void Start()
    {
        base.Start();
        RegistNotification(this, kNotificationKeys.getRegistVerification, GetRegistVerification);
        RegistNotification(this, kNotificationKeys.regist, RegistFinish);
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, -0);
        yanzhengBT.gameObject.SetActive(true);
        countdown.gameObject.SetActive(false);
        M_name.onEndEdit.AddListener(delegate (string str) { /*isChinese(str, M_name); */containsSpace(str, M_name); });
        password.onEndEdit.AddListener(delegate (string str) { lengthlimit(str,password); });
        repassword.onEndEdit.AddListener(delegate (string str) { isSame(repassword, password); });
        tele.onEndEdit.AddListener(delegate (string str) { isPhone(str, tele);  });
        //id.onEndEdit.AddListener(delegate (string str) { isID(str, id); containsSpace(str, id); });
        yanzheng.onEndEdit.AddListener(delegate (string str) { isNumber(str, yanzheng); });
        //email.onEndEdit.AddListener(delegate (string str) { isEmail(str, email); containsSpace(str, email); });
    }

    private void RegistFinish(Hashtable hash)
    {
        Debug.Log(hash.toJson());
        _maskLayer.ShowAlertWithMSG("注册成功");
        onClose();
    }

    private void GetRegistVerification(Hashtable hash)
    {
        Debug.Log(hash.toJson());
        startCountDown();
        _maskLayer.ShowAlertWithMSG("获取验证码成功");
    }

    public void YanZheng()
    {
        if (isNull(tele))
        {
            _maskLayer.ShowAlertWithTips("手机号码不能为空");
            return;
        }


        //else if (isNull(M_name))
        //{
        //    _maskLayer.ShowAlertWithTips("姓名不能为空");
        //    return;
        //}

        Hashtable hash = new Hashtable();
        hash["phoneNum"] = tele.text;
        _dataManager.getRegistVerificationCode(hash);
    }

    private void startCountDown()
    {
        yanzhengBT.gameObject.SetActive(false);
        //countdown.gameObject.SetActive(true);
        countdown.StartCountDown();
    }
    public void Regist()
    {
        if (isNull(M_name))
        {
            _maskLayer.ShowAlertWithTips("用户名不能为空");
            return;
        }
       else if (!lengthOverLimit(M_name,1,8))
        {
            _maskLayer.ShowAlertWithTips("输入长度有误");
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
        else if (isNull(tele))
        {
            _maskLayer.ShowAlertWithTips("手机号码不能为空");
            return;
        }
        else if (isNull(yanzheng))
        {
            _maskLayer.ShowAlertWithTips("验证码不能为空");
            return;
        }
        Hashtable hash = new Hashtable();
        hash["userName"] = M_name.text;
        hash["password"] = password.text;
        hash["repassword"] = repassword.text;
        hash["telephone"] = tele.text;
        hash["code"] = yanzheng.text;
        //if (male.isOn)
        //{
        //    hash["sex"] = 0.ToString();
        //}
        //else
        //{
        //    hash["sex"] = 1.ToString();
        //}
        //hash["email"] = email.text;
        //hash["idCarNum"] = id.text;
        Debug.Log(hash.toJson());
        _dataManager.addUser(hash);
    }
}
