using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
public class UITips : UIAlertLayer
{
    protected override void Start()
    {
        gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        gameObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        if (Param != null)
        {

            string detail = Param["tips"].ToString();
            bool autoDestroy =Convert.ToBoolean( Param["autoDestroy"]);
            gameObject.GetComponentInChildren<Text>().text = detail;
            if (autoDestroy)
            {
                vp_Timer.In(3, delegate ()
                {
                    if (this != null)
                    {
                        onClose();
                    }

                });
            }

        }

       
    }

}
