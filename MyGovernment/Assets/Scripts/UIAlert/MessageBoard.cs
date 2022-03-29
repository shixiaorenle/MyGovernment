using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoard : UIAlertLayer
{

    protected override void Start()
    {
        base.Start();
        setMinSize();
    }

    public void OnClickSure()
    {
        _maskLayer.ShowAlertWithMSG("反馈成功");
        onClose();
    }


}
