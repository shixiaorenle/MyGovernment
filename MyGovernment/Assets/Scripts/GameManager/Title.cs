
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;

public class Title : Define
{
    private string cityName;
    public Text t1;
    public Text t2;
    public Text t3;
    private float time=3;
    public string WeatherKey;
    public string IPKey;
    public string hefengKey;
    private void Update()
    {
        //time -= Time.deltaTime;
        //if (time<=0)
        //{
        //   // StartCoroutine(GetCityName());
            
        //        StartCoroutine(GetCityWeather("1"));
        //    time = 5;
        //}
    }
    void Start()
    {
        //StartCoroutine(hefeng());

        StartCoroutine(GetCityCode("小店区"));
    }


    IEnumerator hefeng()
    {
        var url = new Uri("https://devapi.qweather.com/v7/weather/now?location=37.79604,112.54716&key="+ hefengKey);
        Debug.Log(url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            t3.text = www.downloadHandler.text;

        }
    }




    IEnumerator GetCityName(string ip)
    {
        var url = new Uri("https://api.ip138.com/ip/?ip=" + ip + "&datatype=jsonp&token=56f9562e888afc3514f814b2256e2108");
        Debug.Log(url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            t3.text = www.downloadHandler.text;
            Hashtable hash = www.downloadHandler.text.hashtableFromJson();
            if (hash.ContainsKey("data"))
            {
                ArrayList array = hash["data"] as ArrayList;
                for (int i = 0; i < array.Count; i++)
                {
                    Debug.Log(array[i]);
                    if (i.Equals(2))
                    {
                        StartCoroutine(GetCityCode(array[i] as string));
                    }
                }
            }
        }
    }
    IEnumerator test()
    {
        var uri = new System.Uri(Path.Combine(Application.streamingAssetsPath, "str.json"));
        //        Debug.Log(uri);
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
            t3.text = www.downloadHandler.text;
            Hashtable hash = www.downloadHandler.text.hashtableFromJson();
            if (hash.ContainsKey("data"))
            {
                ArrayList array = hash["data"] as ArrayList;
                for (int i = 0; i < array.Count; i++)
                {
                    //Debug.Log(array[i]);
                    if (i.Equals(3))
                    {
                        StartCoroutine(GetCityCode(array[i] as string));
                    }
                }
            }
        }
    }
    IEnumerator GetCityCode(string cityname)
    {
        var url = new System.Uri("https://api.gugudata.com/weather/weatherinfo/region?appkey="+WeatherKey+"&keyword=" + cityname);
        //var url = new System.Uri("https://api.gugudata.com/weather/weatherinfo/region?appkey="+WeatherKey+"&keyword=清徐");
        Debug.Log(url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            t2.text = www.downloadHandler.text;
            Hashtable hashtable = www.downloadHandler.text.hashtableFromJson();
            if (hashtable.ContainsKey("Data"))
            {
                ArrayList list = hashtable["Data"] as ArrayList;
                Hashtable hash = list[0] as Hashtable;
                if (hash.ContainsKey("Code"))
                {
                    string str = hash["Code"] as string;
                    Debug.Log(str);
                    StartCoroutine(GetCityWeather(str));
                }
            }
            

        }
    }
    private string GetWeaId(Hashtable hashtable,string cityname)
    {
        string weaid = "";
        if (hashtable.ContainsKey("result"))
        {
            Hashtable hash = hashtable["result"] as Hashtable;
            if (hash.ContainsKey("dtList"))
            {
                Hashtable h = hash["dtList"] as Hashtable;
                foreach (var item in h.Keys)
                {
                    Hashtable data = h[item] as Hashtable;
                    if (data.ContainsKey("cityNm"))
                    {
                        string cityNm = data["cityNm"] as string;
                        if (cityNm.Equals(cityname))
                        {
                            if (data.ContainsKey("weaId"))
                            {
                                weaid = data["weaId"] as string;                               
                            }
                        }
                    }
                }
            }
        }
        return weaid;
    }
    IEnumerator GetCityWeather(string citycode)
    {
        var url = new System.Uri("https://api.gugudata.com/weather/weatherinfo?appkey="+WeatherKey+"&code="+citycode+"&days=1");
        Debug.Log(url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            t1.text = www.downloadHandler.text;
        }
    }
    IEnumerator GetIPWeather(string ip)
    {
       // var url = new System.Uri("http://api.k780.com/?app=ip.get&ip=" + ip + "&appkey=59701&sign=7cbd3622746c6319cd2fed48bfbee8c1&format=json");
        var url = new System.Uri("https://api.ip138.com/ip/?ip="+ip+ "&datatype=jsonp&token="+IPKey);
        Debug.Log(url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            t1.text = www.downloadHandler.text;
        }
    }
    IEnumerator GetIP()
    {       
        var url = new System.Uri("http://icanhazip.com/");
        Debug.Log(url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string str = www.downloadHandler.text.Trim();
            Debug.Log(str);
            t2.text = www.downloadHandler.text;
            StartCoroutine(GetCityName(str));
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    private string getIP()
    {
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adater in adapters)
        {
            if (adater.Supports(NetworkInterfaceComponent.IPv4))
            {
                UnicastIPAddressInformationCollection UniCast = adater.GetIPProperties().UnicastAddresses;
                if (UniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in UniCast)
                    {
                        if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            Debug.Log(uni.Address.ToString());
                            t2.text = uni.Address.ToString();
                            return uni.Address.ToString();
                        }
                    }
                }
            }
        }
        return null;
    }
}
