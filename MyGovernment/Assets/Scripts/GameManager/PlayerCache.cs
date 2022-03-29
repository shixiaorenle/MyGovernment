
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Events;
using BestHTTP;

/*
 * 更新存档注意：
 * 1.在SaveDataPlayer将 version改为当前版本号，并将新的参数写入
 * 2.需要初始化赋值的写入newGame ()中
 * 3.checkVersion()中写入新版本改动，并将_playerData.version 改为当前正式版本
 * 4.dataFormHastable 需要判断有没有当前KEY,否则可能会出现拉取之前存档报错的情况
 */

[Serializable]
public class SaveDataPlayer //内部属性
{
    //public delegate void Action(string url, Hashtable param, OnRequestFinishedDelegate onRequestFinished, bool isLoding = false);
    //public Action action;

    //public string nowUrl;
    //public Hashtable nowHash;
    //public OnRequestFinishedDelegate nowFinishDel;
    //public bool nowBool;
    public bool delayOpen;
    public string WebsocketIPAddress = "";
    public string HttpIPAddress = "http://39.105.94.235:8888/";
    //public List<string> mStopPoints = new List<string>();//所有停止点
    public Dictionary<string, string> mGreenHouses = new Dictionary<string, string>();//boxNum+名字
    public Dictionary<string, Dictionary<string, string>> mStopPoints = new Dictionary<string, Dictionary<string, string>>();//boxNum+<停止控制点名+停止控制点id>
    public Dictionary<string, Dictionary<string, string>> mQTPoints = new Dictionary<string, Dictionary<string, string>>();//boxNum+<停止控制点名+停止控制点id>
    public Dictionary<string, Dictionary<string, string>> mPoints = new Dictionary<string, Dictionary<string, string>>();//boxNum+<控制点名+控制点id>
    public Dictionary<string, Dictionary<string, string>> mMonitoringPoints = new Dictionary<string, Dictionary<string, string>>();//boxNum+<数据显示点名+数据显示点id>
    public Dictionary<string, Dictionary<string, string>> mDeviceState = new Dictionary<string, Dictionary<string, string>>();//boxNum+<状态点名+状态点id>
    public Dictionary<string, Dictionary<string, string>> mParameterSet = new Dictionary<string, Dictionary<string, string>>();//boxNum+<参数点名+参数点id>
    public Dictionary<string, Dictionary<string, string>> mWeatherSet = new Dictionary<string, Dictionary<string, string>>();//boxNum+<气象点名+气象点id>
    public Dictionary<string, GameObject> mHouseUI = new Dictionary<string, GameObject>();//boxNum+HouseUI
    public Hashtable myData = new Hashtable();
    public string myBoxNum = "";
    public string token = "";
    public string cellPhone = "";
    public bool getCameraAuthorization//是否获取摄像头权限
    {
        get
        {
            return (WebCamTexture.devices != null && WebCamTexture.devices.Length > 0);
        }
    }
    [Serializable]
    public struct user
    {
        //public string roles;
        public string userName;
        public string userID;
        public string userCellPhone ;
        public string real_name;
    }


    public user _User;
    
}

public class SaveData
{
    public bool isRecordUP = true;//是否记录用户名与密码

    public string userName = "";
    public string passWord = "";
    public string userId = "";

    // public List<GreenHouse> greenHouses = new List<GreenHouse>();
   // public ArrayList greenHouses = new ArrayList();
    //public List<string> plants = new List<string>();



    #region 同步数据json
    private T GetValue<T>(string Key, Hashtable data)
    {
        Type tType = typeof(T);
        if (data.ContainsKey(Key))
        {
            if (tType.IsAssignableFrom(typeof(Dictionary<string, Dictionary<string, string>>)))
            {
                return (T)Convert.ChangeType(Convert.ToString(data[Key]).dictWithDictFormJson(), tType);

            }
            else if (tType.IsAssignableFrom(typeof(Dictionary<string, Dictionary<string, Hashtable>>)))
            {
                return (T)Convert.ChangeType(Convert.ToString(data[Key]).dictHashtableFromJson(), tType);
            }
            else if (tType.IsAssignableFrom(typeof(List<string>)))
            {
                return (T)Convert.ChangeType(Convert.ToString(data[Key]).ListSetFormJson(), tType);
            }
            else
            {
                return (T)Convert.ChangeType(data[Key], tType);
            }
        }
        else
        {
            return (T)Convert.ChangeType(data[Key], tType);
            //Debug.Log ("参数：" + Key + "无返回值");
        }


    }
    #endregion
}

public class PlayerCache : Define
{

    static public string VERSION = "1.0";

    #region  需存储数据结
    public SaveDataPlayer _playerData = new SaveDataPlayer();
    public SaveData _Data = new SaveData();
    #endregion
    void Awake()
    {


    }
    void Start()
    {

    }
    /// <summary>
    /// 判断版本对新版本数据进行操作
    /// </summary>
    public void checkVersion()
    {

    }
    public void newGame()
    {
    }
    /// <summary>
    /// 获取该盒子下所有数据显示点
    /// </summary>
    /// <param name="boxNum"></param>
    /// <returns></returns>
    public ArrayList getMoniroting(string boxNum)
    {
        ArrayList array = new ArrayList();
        foreach (var item in _playerData.mMonitoringPoints)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> keyValuePairs = item.Value;
                foreach (string id in keyValuePairs.Values)
                {
                    array.Add(id);
                }
            }
        }

        return array;
    }
    /// <summary>
    /// 获取该盒子下所有气象数据点
    /// </summary>
    /// <param name="boxNum"></param>
    /// <returns></returns>
    public ArrayList getWeather(string boxNum)
    {
        ArrayList array = new ArrayList();
        foreach (var item in _playerData.mWeatherSet)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> keyValuePairs = item.Value;
                foreach (string id in keyValuePairs.Values)
                {
                    array.Add(id);
                }
            }
        }

        return array;
    }
    /// <summary>
    /// 获取该盒子下所有设备状态和参数
    /// </summary>
    /// <param name="boxNum"></param>
    /// <returns></returns>
    public ArrayList getDeviceStateAndParameter(string boxNum)
    {
        ArrayList array = new ArrayList();
        foreach (var item in _playerData.mDeviceState)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> keyValuePairs = item.Value;
                foreach (string id in keyValuePairs.Values)
                {
                    array.Add(id);
                }
            }
        }
        foreach (var item in _playerData.mParameterSet)
        {
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> keyValuePairs = item.Value;
                
                foreach (var id in keyValuePairs)
                {
                   // Debug.Log(id.Key + ":" + id.Value);
                    array.Add(id.Value);
                }
            }
        }
        return array;
    }
    /// <summary>
    /// 获取该盒子下所有设备状态
    /// </summary>
    /// <param name="boxNum"></param>
    /// <returns></returns>
    public ArrayList getDeviceState(string boxNum)
    {
        ArrayList array = new ArrayList();
        foreach (var item in _playerData.mDeviceState)
        {
            if (!item.Key.Equals(_playerData.myBoxNum))
            {
                break;
            }
            if (item.Key.Equals(boxNum))
            {
                Dictionary<string, string> keyValuePairs = item.Value;
                foreach (string id in keyValuePairs.Values)
                {
                    array.Add(id);
                }
            }
        }
        return array;
    }
    /// <summary>
    /// 获取该id控制点对应的名字
    /// </summary>
    /// <param name="pointName"></param>
    /// <returns></returns>
    public string GetPointName(string pointID)
    {
        string Name = "";
        foreach (var item in _playerData.mPoints)
        {
            if (!item.Key.Equals(_playerData.myBoxNum))
            {
                break;
            }
            foreach (var i in item.Value)
            {
                //Debug.Log(i.Key);
                if (i.Value.Equals(pointID))
                {
                    Name = i.Key;
                }

            }
        }
        return Name;
    }
    /// <summary>
    /// 获取该id监控点对应的名字
    /// </summary>
    /// <param name="pointName"></param>
    /// <returns></returns>
    public string GetMonitoringName(string pointID)
    {
        string Name = "";
        foreach (var item in _playerData.mMonitoringPoints)
        {
            if (!item.Key.Equals(_playerData.myBoxNum))
            {
                break;
            }
            foreach (var i in item.Value)
            {
                //Debug.Log(i.Key);
                if (i.Value.Equals(pointID))
                {
                    Name = i.Key;
                }

            }
        }
        return Name;
    }
    /// <summary>
    /// 获取该id参数点点对应的名字
    /// </summary>
    /// <param name="pointName"></param>
    /// <returns></returns>
    public string GetParameterName(string pointID)
    {
        string Name = "";
        foreach (var item in _playerData.mParameterSet)
        {
            if (!item.Key.Equals(_playerData.myBoxNum))
            {
                break;
            }
            foreach (var i in item.Value)
            {
                //Debug.Log(i.Key);
                if (i.Value.Equals(pointID))
                {
                    Name = i.Key;
                }

            }
        }
        return Name;
    }
    /// <summary>
    /// 获取该名字控制点对应的id
    /// </summary>
    /// <param name="pointName"></param>
    /// <returns></returns>
    public string GetPointID(string pointName)
    {
        string id = "";
        foreach (var item in _playerData.mPoints)
        {
            if (!item.Key.Equals(_playerData.myBoxNum))
            {
                break;
            }
            foreach (var i in item.Value)
            {
                //Debug.Log(i.Key);
                if (i.Key.Equals(pointName))
                {
                    id = i.Value;
                }

            }
        }
        return id;
    }
    /// <summary>
    /// 获取该名字监控点对应的id
    /// </summary>
    /// <param name="pointName"></param>
    /// <returns></returns>
    public string GetMonitoringID(string pointName)
    {
        string id = "";
        foreach (var item in _playerData.mMonitoringPoints)
        {
            foreach (var i in item.Value)
            {
                //Debug.Log(i.Key);
                if (i.Key.Equals(pointName))
                {
                    id = i.Value;
                }

            }
        }
        return id;
    }
    /// <summary>
    /// 获取该名字参数点对应的id
    /// </summary>
    /// <param name="pointName"></param>
    /// <returns></returns>
    public string GetParameterID(string pointName)
    {
        string id = "";
        foreach (var item in _playerData.mParameterSet)
        {
            foreach (var i in item.Value)
            {
                //Debug.Log(i.Key);
                if (i.Key.Equals(pointName))
                {
                    id = i.Value;
                }

            }
        }
        return id;
    }
    /// <summary>
    /// 获取boxNumber对应的名字
    /// </summary>
    /// <param name="pointName"></param>
    /// <returns></returns>
    public string GetHouseName(string boxNum)
    {
        string mName = "";
        foreach (var item in _playerData.mGreenHouses)
        {
            if (item.Key.Equals(boxNum))
            {
                mName = item.Value;
            }
        }
        return mName;
    }
    private void OnApplicationQuit()
    {
        //if (!_Data.isRecordUP)
        //{
        //    _playerCache._Data.isRecordUP = false;
        //    _playerCache._Data.userName = "";
        //    _playerCache._Data.passWord = "";
        //    _playerCache._Data.greenHouses.Clear();
        //    _playerCore.saveProfile();
        //}
    }
}
