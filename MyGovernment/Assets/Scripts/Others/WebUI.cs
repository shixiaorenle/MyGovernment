using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebUI : UIAlertLayer
{
    public string url;
    public UniWebView uniWeb;
    public Text text;
    protected override void Start()
    {
        base.Start();
        setMinSize();
        uniWeb.OnPageStarted += delegate (UniWebView webView, string url)
        {
            text.text = "正在加载...";
        };
        uniWeb.OnPageFinished += delegate (UniWebView webView, int statusCode, string url)
        {
            text.text = "加载完成";
        };
        uniWeb.OnPageErrorReceived += delegate(UniWebView webView, int errorCode, string errorMessage) { _maskLayer.ShowAlertWithTips("网页加载错误:"+ errorMessage); };
        uniWeb.Load(url);
        uniWeb.Show();
    }
    public void OnClickQuit()
    {

        uniWeb.CleanCache();
        onClose();
    }
    public void Refresh()
    {
        uniWeb.Load(url);
        uniWeb.Show();
    }
}
