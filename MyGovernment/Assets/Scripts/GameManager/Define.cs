using BestHTTP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DelayToInvoke : MonoBehaviour//类似BLOCK函数
{
    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}
public static class LoadIndex
{
    public static int loadingIndex = 0;
}
public class Define : MonoBehaviour
{
    static protected bool _isScreenConfigInited = false;
    static protected Vector2 _screenSize = new Vector2(480, 800);
    static protected Vector2 _realScreenSize;
    static protected Vector2 _realScreenSizeCenter;
    static protected Vector2 _scaledScreenSize;
    static protected float _scaleFactor = 1.0f;
    static protected float _scaleFactorX = 1.0f;
    static protected float _scaleFactorY = 1.0f;
    static protected List<object> _notificationsDelegate = new List<object>();
    static protected List<kNotificationKeys> _notificationsKey = new List<kNotificationKeys>();
    static protected List<Action<Hashtable>> _notificationsCallBack = new List<Action<Hashtable>>();

    protected GameManager _gameManager
    {
        get
        {
            return GameManager.instance;
        }
    }
    protected PlayerCore _playerCore
    {
        get
        {
            return GameManager.instance.playerCore;
        }
    }
    protected PlayerCache _playerCache
    {
        get
        {
            return GameManager.instance.playerCore.playerCache;
        }

    }
    public MaskLayer _maskLayer
    {
        get
        {
            return _gameManager.maskLayer;
        }
    }

    public DataManager _dataManager
    {
        get
        {
            return _gameManager.dataManager;
        }
    }
    protected void RemoveNotification(object pDelegate)
    {
        while (_notificationsDelegate.Contains(pDelegate))
        {
            int index = 0;
            foreach (object obj in _notificationsDelegate)
            {
                if (obj == pDelegate)
                {
                    _notificationsKey.RemoveAt(index);
                    _notificationsCallBack.RemoveAt(index);
                    _notificationsDelegate.RemoveAt(index);
                    break;
                }
                index++;
            }
        }
    }
    static public void PostNotification(kNotificationKeys pKey)
    {
        PostNotification(pKey, null);
    }

    static public void PostNotification(kNotificationKeys pKey, Hashtable pTable)
    {
        if (_notificationsKey.Contains(pKey))
        {
            int index = 0;
            foreach (kNotificationKeys key in _notificationsKey)
            {
                if (key == pKey)
                {
                    if (_notificationsDelegate[index] != null)
                    {
                        _notificationsCallBack[index](pTable);
                    }
                }
                index++;
            }
        }
    }
    protected void RegistNotification(object pDelegate, kNotificationKeys pKey, Action<Hashtable> pCallBack)
    {
        _notificationsKey.Add(pKey);
        _notificationsCallBack.Add(pCallBack);
        _notificationsDelegate.Add(pDelegate);


    }
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    void OnDestroy()
    {
        RemoveNotification(this);
    }
    public void removeAllChild(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject go = obj.transform.GetChild(i).gameObject;
            //			Debug.Log ("名字" + go.name);
            Destroy(go);
        }

    }
    public static string AesEncrypt(string str, string key = "pflyandpflyand==")
    {

        if (string.IsNullOrEmpty(str)) return null;
        Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

        System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
        {
            Key = Encoding.UTF8.GetBytes(key),
            Mode = System.Security.Cryptography.CipherMode.ECB,
            Padding = System.Security.Cryptography.PaddingMode.PKCS7
        };

        System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateEncryptor();
        Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    public static string md5String(string str)
    {
        try
        {
            using (System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] retVal = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
    /// <summary>
    /// 字符串长度
    /// </summary>
    /// <param name="oString"></param>
    /// <returns></returns>
    public int StringLength(string oString)
    {
        byte[] strArray = System.Text.Encoding.Default.GetBytes(oString);
        int res = strArray.Length;
        return res;
    }
    #region 请求Http
    public void RequestHttp(string url, Hashtable param, OnRequestFinishedDelegate onRequestFinished, bool isLoding = false)
    {
        		//Debug.Log(url);
        // 显示loading;
        if (isLoding)
        {
            if (LoadIndex.loadingIndex.Equals(0))
            {
                //_playerCache._playerData.nowUrl = url;
                //_playerCache._playerData.nowHash = param;
                //_playerCache._playerData.nowFinishDel = onRequestFinished;
                //_playerCache._playerData.nowBool = isLoding;
                _maskLayer.ShowLoadingAlert(kAlertTypes.loading);
            }
            LoadIndex.loadingIndex++;
           // Debug.Log(LoadIndex.loadingIndex);


        }

        HTTPMethods current = HTTPMethods.Get;
        if (param != null)
        {
            current = HTTPMethods.Post;
        }


        HTTPRequest request = new HTTPRequest(new Uri(url),
                                  current,
                                  onRequestFinished);
        if (param != null)
        {
            request.RawData = Encoding.UTF8.GetBytes(param.toJson());
            	
        }
        
        request.Timeout = TimeSpan.FromSeconds(30);
        request.ConnectTimeout = TimeSpan.FromSeconds(30);

        if (!_playerCache._playerData.token.Equals(""))//加入用户
        {
            request.AddHeader("Authorization", /*"Bearer " + */_playerCache._playerData.token);
        }

        //if (!_playerCache._Data.user_id.Equals(""))//加入用户
        //{
        //    request.AddHeader("userId", _playerCache._Data.user_id);
        //}
        request.Send();
    }
    #endregion

    #region 请求回调
    public void OnRequestFinished(HTTPRequest request, HTTPResponse response, Action<Hashtable> successCallBack, Action errorCallBack = null,bool closeLoad=true)
    {
        // 取消loading;

        if (closeLoad)
        {
            LoadIndex.loadingIndex--;
            if (LoadIndex.loadingIndex.Equals(0))
                PostNotification(kNotificationKeys.closeLoading);
        }
        
        if (request.State != HTTPRequestStates.Finished)
        {

            if (errorCallBack != null)
            {
                errorCallBack();
               // _maskLayer.ShowAlertWithTips("请检查网络");
            }
            return;
        }
        		Debug.Log("内容:"+response.DataAsText+ "地址:" + request.Uri);
        Hashtable data = response.DataAsText.hashtableFromJson();
        //if (!data.ContainsKey("data"))
        //{//无返回情况

        //    if (errorCallBack != null)
        //    {
        //        errorCallBack();
        //        _maskLayer.ShowAlertWithTips("请检查网络");
        //    }
        //    return;
        //}

        int code = Convert.ToInt32(data["code"]);
        if (code != 0)
        {
            Debug.Log("错误码" + code+request.Uri);
            //Debug.Log("message:" + data["message"].ToString());
            if (data.ContainsKey("msg"))
            {
                _maskLayer.ShowAlertWithTips(data["msg"].ToString(),false);
            }

            errorCallBack?.Invoke();
            return;
        }

        successCallBack(data);
    }
    #endregion
    #region 判断字符串类型
    /// <summary>
    /// 判断输入是否为纯数字
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="inputField"></param>
    protected void isNumber(string arg0, InputField inputField)
    {
        if (arg0 == null || arg0 == "")
        {
            return;
        }
        Regex regex = new Regex("^(-?[0-9]*[.]*[0-9]{0,3})$");
        bool b = regex.IsMatch(arg0);
        if (b == true)
        {
           // Debug.Log("是纯数字");
        }
        else
        {
            //Debug.Log("不是纯数字");
            inputField.text = "";
            _maskLayer.ShowAlertWithTips("请输入数字");
        }
    }
    /// <summary>
    /// 判断输入是否包含空格
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="inputField"></param>
    protected bool containsSpace(string arg0, InputField inputField)
    {
        if (arg0 == null || arg0 == "")
        {
            return false;
        }
        if (arg0.Contains(" "))
        {
            Debug.Log("包含空格");
            inputField.text = "";
            _maskLayer.ShowAlertWithTips("不能包含空格");
            return true;
        }
        else
        {
            Debug.Log("不包含空格");
            return false;
        }
    }
    /// <summary>
    /// 限制字符串长度
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="inputField"></param>
    protected bool lengthOverLimit(InputField arg0,int min,int max)
    {
        if (arg0.text == null)
        {
            return false;
        }
        else if(arg0.text.Length > max || arg0.text.Length < min)
        {
           // inputField.text = "";
           // _maskLayer.ShowAlertWithTips(string.Format("请输入{0}-{1}位文字",min.ToString(),max.ToString()));
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// 判断两次输入是否一样
    /// </summary>
    /// <param name="inputField1"></param>
    /// <param name="inputField2"></param>
    protected void isSame( InputField inputField1, InputField inputField2)
    {
        if (inputField1.text == null || inputField1.text == "")
        {
            return;
        }
        if (!inputField1.text.Equals(inputField2.text))
        {
            Debug.Log("密码输入不一致!");
            inputField1.text = "";
            _maskLayer.ShowAlertWithTips("密码输入不一致!");
        }
        else
        {
            Debug.Log("一致");

        }
    }
    protected void lengthlimit(string arg0,InputField inputField)
    {
        //if (arg0.Length>max||arg0.Length<min)
        //{
        //    Debug.Log("长度不对");
        //    inputField.text = "";
        //    _maskLayer.ShowAlertWithTips("请输入6-20位长度密码");
        //}

        if (arg0 == null || arg0 == "")
        {
            return;
        }
        //Regex regex = new Regex(@"^[a-zA-Z]\w{5,17}$");

        var regex = new Regex(@"
        (?=.*[0-9])                     #必须包含数字
        (?=.*[a-zA-Z])                  #必须包含小写或大写字母
        .{6,20}                         #至少8个字符，最多30个字符
        ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        bool b = regex.IsMatch(arg0);
        if (b == true)
        {
            Debug.Log("符合要求");
        }
        else
        {
            inputField.text = "";
            _maskLayer.ShowAlertWithTips(string.Format("密码必须包含数字和字母,且长度在6-20之间"));
            
        }
    }
    /// <summary>
    /// 判断输入是否为汉字
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="inputField"></param>
    protected void isChinese(string arg0, InputField inputField)
    {
        if (arg0 == null || arg0 == "")
        {
            return;
        }
        Regex regex = new Regex(@"[\u4e00-\u9fa5]");
        bool b = regex.IsMatch(arg0);
        if (b == true)
        {
            Debug.Log("是汉字");
        }
        else
        {
            Debug.Log("不是汉字");
            inputField.text = "";
            _maskLayer.ShowAlertWithTips("请输入汉字");
        }
    }
    /// <summary>
    /// 判断输入是否为电话号码
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="inputField"></param>
    protected void isPhone(string arg0, InputField inputField)
    {
        if (arg0 == null || arg0 == "")
        {
            return;
        }
        Regex regex = new Regex("^1(3[0-9]|4[01456879]|5[0-35-9]|6[2567]|7[0-8]|8[0-9]|9[0-35-9])\\d{8}$");
        bool b = regex.IsMatch(arg0);
        if (b == true)
        {
            Debug.Log("是电话");
        }
        else
        {
            Debug.Log("不是电话");
            inputField.text = "";
            _maskLayer.ShowAlertWithTips("请输入正确的手机号码");
        }
    }
    /// <summary>
    /// 判断输入是否为邮箱
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="inputField"></param>
    protected void isEmail(string arg0, InputField inputField)
    {
        if (arg0 == null || arg0 == "")
        {
            return;
        }
        Debug.Log(arg0+ inputField.name);
        Regex regex = new Regex("^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+$");
        bool b = regex.IsMatch(arg0);
        if (b == true)
        {
            Debug.Log("是邮箱");
        }
        else
        {
            Debug.Log("不是邮箱");
            inputField.text = "";
            _maskLayer.ShowAlertWithTips("请输入正确的邮箱");
        }
    }
    /// <summary>
    /// 判断输入是否为身份证
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="inputField"></param>
    protected void isID(string arg0, InputField inputField)
    {
        if (arg0 == null || arg0 == "")
        {
            return;
        }
        Debug.Log(arg0 + inputField.name);
        Regex regex = new Regex("^[1-9][0-9]{5}(19|20)[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|31)|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}([0-9]|x|X)$");
        bool b = regex.IsMatch(arg0);
        if (b == true)
        {
            Debug.Log("是身份证");
        }
        else
        {
            Debug.Log("是身份证");
            inputField.text = "";
            _maskLayer.ShowAlertWithTips("请输入正确的身份证号");
        }
    }
    /// <summary>
    /// 判断输入是否为空
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="inputField"></param>
    protected bool isNull( InputField inputField)
    {

        if (inputField.text == "")
        {
            return true;
        }
        return false;
    }
    #endregion
    #region 时间戳转换成时间
    public DateTime ConvertStringToDateTime(string timeStamp)
    {
        //Debug.Log(timeStamp.Length);
        //while (timeStamp.Length < 13)
        //{
        //    timeStamp += "0";
        //}
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }
    #endregion
    #region 时间转换成时间戳
    public long ConvertStringToTimeTamp(DateTime dateTime)
    {
        TimeSpan st = dateTime - new DateTime(1970, 1, 1).ToLocalTime();//获取时间戳  

        long l = Convert.ToInt64(st.TotalSeconds);
        //Debug.Log(l);
        //while (l < 1000000000000&&l!=0)
        //{
        //    l *= 10;
        //}
        //Debug.Log(l);

        return l;
    }
    #endregion
}
