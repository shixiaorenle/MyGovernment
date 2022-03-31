using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitlePC : Define
{
    public CameraPathAnimator cameraPath;
    public Toggle roamOn;
    public Toggle roamOff;
    void Start()
    {
        roamOn.onValueChanged.AddListener(RoamOn);
        roamOff.onValueChanged.AddListener(RoamOff);
    }

    private void RoamOff(bool arg0)
    {
        if (arg0)
        {
            cameraPath.Pause();
        }
    }

    private void RoamOn(bool arg0)
    {
        if (arg0)
        {
            cameraPath.Play();
        }
    }
    public void OnClickSet()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.Set);
    }
    public   void OnClickSuggest()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.MessageBoard);
    }
    void Update()
    {
        
    }
}
