using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class AddGreenHouseUI : UIAlertLayer
{
    public InputField M_name;
    public InputField SN;
    public InputField describe;


    private List<string> plants = new List<string>()
    {
        "草莓","西红柿","黄瓜","其他"
    };
    protected override void Start()
    {
        base.Start();
        setMinSize();
        RegistNotification(this, kNotificationKeys.scan, Scan);
 
        M_name.onEndEdit.AddListener(delegate (string str) { isChinese(str, M_name); });
        SN.onEndEdit.AddListener(delegate (string str) { isNumber(str, SN); });
        WebCamDevice[] devices = WebCamTexture.devices;//获取摄像头权限
    }

    private void Scan(Hashtable obj)
    {
        SN.text = obj["SN"].ToString();
    }

    public void Close()
    {
        //_maskLayer.Container = _gameManager._UI2Dmanager.transform;
        onClose();
    }
    public void Add()
    {
        //if (isNull(M_name))
        //{
        //    _maskLayer.ShowAlertWithTips("大棚名字不能为空");
        //    return;
        //}
        //else if (isNull(SN))
        //{
        //    _maskLayer.ShowAlertWithTips("SN号码不能为空");
        //    return;
        //}
        if (isNull(SN))
        {
            _maskLayer.ShowAlertWithTips("SN号码不能为空");
            return;
        }

        Hashtable hash = new Hashtable();
        hash["archName"] = M_name.text;
        hash["archNum"] = SN.text;
        if (!isNull(describe))
        {
            hash["archDesc"] = describe.text;
        }
        
        //hash["archPasswd"] = password.text;
        //_maskLayer.Container = _gameManager._UI2Dmanager.transform;
        _dataManager.addGreenHouse(hash);
        onClose();
    }
    public void OpenPhoto()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.Photo, KMasklayerCenter.masklayer);
    }
    private void OnDisable()
    {
        
    }
}
