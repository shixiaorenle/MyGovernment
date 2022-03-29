using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAndroid : MonoBehaviour
{
    public InputField m_InputFiled;
    public Text m_Label;

    public void OnClickButton1()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("U3D_ShowToast1");
    }

    public void OnClickButton2()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("U3D_ShowToast2", m_InputFiled.text);
    }

    public void OnClickButton3()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        m_Label.text = jo.Call<string>("U3D_GetValue");
    }
}
