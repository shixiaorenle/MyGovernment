using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用作连续弹出不同alert时
/// </summary>
public class AlertQueueItem
{

    public kAlertTypes _pType;
    public Hashtable _pParam;
    //public bool _isStateBarAlert;

    /// <summary>
    /// 创建一个弹框所需的参数
    /// </summary>
    /// <param name="pType">弹框类型</param>
    /// <param name="pParam"> 传入的哈希表.</param>
    /// <param name="isStateBarAlert">是否屏蔽顶部状态栏按钮</param>
    public AlertQueueItem(kAlertTypes pType, Hashtable pParam = null, bool isStateBarAlert = false)
    {
        _pType = pType;
        _pParam = pParam;
        //	_isStateBarAlert = isStateBarAlert;

    }


}
public class MaskLayer : Define
{
    public Transform Container;
    public Transform MSGContainer;
    public List<GameObject> Windows;
    Dictionary<kAlertTypes, GameObject> _prefabRelated = new Dictionary<kAlertTypes, GameObject>();
    //*************************************************************************************************
    public GameObject LoadingPrefab;//转动的圈圈
    public GameObject UITips;//弹框
    public GameObject Center;//大棚中心
    public GameObject Record;//操作记录
    public GameObject WarningHistory;//报警历史列表
    public GameObject WarningList;//报警列表
    public GameObject DeleteLinkman;//删除单个联系人
    public GameObject AddLinkman;//添加联系人
    public GameObject Control;//单个大棚
    public GameObject DeleteHouse;//删除单个大棚
    public GameObject WarningDetail;//单个报警历史
    public GameObject WarningConfirm;//单个报警确认处理
    public GameObject WarningEdit;//报警编辑
    public GameObject RecordUI;//单个报警
    public GameObject SetUI;//我的设置
    public GameObject OtherUI;//我的设其他功能置
    public GameObject MaxVideo;//视频框
    public GameObject Stop;//紧急制动
    public GameObject Regist;//注册
    public GameObject ForgetPassword;//忘记密码
    public GameObject UpdatePassword;//重置密码
    public GameObject AddGreenHouse;//添加大棚
    public GameObject WebUI;//网页
    public GameObject Guide;//操作指南
    public GameObject Data;//传感器参数
    public GameObject Photo;//通知消息
    public GameObject Date;//筛选日期
    public GameObject MessageBoard;//留言板
    public UIMSG MSG;//通知消息
    public List<AlertQueueItem> AlertQueueList = new List<AlertQueueItem>();
    private void Awake()
    {
        _prefabRelated.Add(kAlertTypes.loading, LoadingPrefab);
        _prefabRelated.Add(kAlertTypes.UITips, UITips);
        _prefabRelated.Add(kAlertTypes.WarningHistory, WarningHistory);
        _prefabRelated.Add(kAlertTypes.Center, Center);
        _prefabRelated.Add(kAlertTypes.Record, Record);
        _prefabRelated.Add(kAlertTypes.Control, Control);
        _prefabRelated.Add(kAlertTypes.AddLinkman, AddLinkman);
        _prefabRelated.Add(kAlertTypes.DeleteLinkman, DeleteLinkman);
        _prefabRelated.Add(kAlertTypes.DeleteHouse, DeleteHouse);
        _prefabRelated.Add(kAlertTypes.RecordUI, RecordUI);
        _prefabRelated.Add(kAlertTypes.WarningDetail, WarningDetail);
        _prefabRelated.Add(kAlertTypes.WarningList, WarningList);
        _prefabRelated.Add(kAlertTypes.WarningConfirm, WarningConfirm);
        _prefabRelated.Add(kAlertTypes.WarningEdit, WarningEdit);
        _prefabRelated.Add(kAlertTypes.MaxVideo, MaxVideo);
        _prefabRelated.Add(kAlertTypes.Set, SetUI);
        _prefabRelated.Add(kAlertTypes.Other, OtherUI);
        _prefabRelated.Add(kAlertTypes.Stop, Stop);
        _prefabRelated.Add(kAlertTypes.Regist, Regist);
        _prefabRelated.Add(kAlertTypes.AddGreenHouse, AddGreenHouse);
        _prefabRelated.Add(kAlertTypes.ForgetPassword, ForgetPassword);
        _prefabRelated.Add(kAlertTypes.UpdatePassword, UpdatePassword);
        _prefabRelated.Add(kAlertTypes.WebUI, WebUI);
        _prefabRelated.Add(kAlertTypes.GuideUI, Guide);
        _prefabRelated.Add(kAlertTypes.Data, Data);
        _prefabRelated.Add(kAlertTypes.Photo, Photo);
        _prefabRelated.Add(kAlertTypes.Date, Date);
        _prefabRelated.Add(kAlertTypes.MessageBoard, MessageBoard);
    }
    void Start()
    {
        AutoDismiss();

    }
    public void AutoDismiss()
    {
        foreach (Transform child in Container)
        {
            if (child.gameObject.activeSelf)
            {
                return;
            }
        }
        gameObject.SetActive(false);
    }
    public GameObject ShowAlertWithTips(string detail,bool autoDestroy = true)
    {
        Container = transform;
        Hashtable dehash = new Hashtable();
        dehash["tips"] = detail;
        dehash["autoDestroy"] = autoDestroy;

        gameObject.SetActive(true);
        GameObject alert = Instantiate(_prefabRelated[kAlertTypes.UITips]);

        alert.transform.SetParent(Container);
        alert.transform.localPosition = Vector3.zero;
        alert.transform.localScale = Vector3.one;
        UIAlertLayer alertLayerClass = alert.GetComponent<UIAlertLayer>();
        alertLayerClass.Type = kAlertTypes.UITips;
        alertLayerClass.Param = dehash;
        Debug.Log("Instantiate a alertLayer:" + alert.ToString());
        Windows.Add(alert);
        return alert;
    }
    public void ShowAlertWithMSG(string detail)
    {
        Hashtable dehash = new Hashtable();
        dehash["tips"] = detail;
        gameObject.SetActive(true);
        MSG.Param = dehash;
        MSG.Refresh();
    }
    public void RemoveAlert(GameObject pAlertObject)
    {
        UIAlertLayer alertLayerClass = pAlertObject.GetComponent<UIAlertLayer>();
        bool isqueue = alertLayerClass.isQueue;
        //判断是否为Loding弹框

        //		if(alertLayerClass.Type == kAlertTypes.loding)
        //		{
        ////			Debug.Log ("去除loding");
        //			LodingOnlyMask = false;
        //		}


        Windows.Remove(pAlertObject);
        Destroy(pAlertObject);
        if (Windows.Count == 0)
        {

            gameObject.SetActive(false);
        }

        if (isqueue)
        {
            AlertQueueList.RemoveAt(0);
            queueAlert();
        }

    }
    public void RemoveAlertWithType(List<kAlertTypes> removeList)
    {
        List<GameObject> needRemove = new List<GameObject>();
        foreach (var VARIABLE in Windows)                                   //在windos窗口遍历              
        {
            if (removeList.Contains(VARIABLE.GetComponent<UIAlertLayer>().Type))
            {
                needRemove.Add(VARIABLE);

            }
        }

        foreach (var VARIABLE in needRemove)
        {
            RemoveAlert(VARIABLE);
        }
    }
    public void queueAlert()
    {
        if (AlertQueueList.Count == 0)
        {
            return;
        }
        //需要传递的数据啦
        AlertQueueItem item = AlertQueueList[0];
        //弹框
        //	_gameManager.iscanSlide = false;
        gameObject.SetActive(true);
        GameObject alert = Instantiate(_prefabRelated[item._pType]);
        alert.transform.SetParent(Container);
        alert.transform.localPosition = Vector3.zero;
        alert.transform.localScale = Vector3.one;
        UIAlertLayer alertLayerClass = alert.GetComponent<UIAlertLayer>();
        alertLayerClass.Type = item._pType;
        alertLayerClass.Param = item._pParam;
        alertLayerClass.isQueue = true;


        Debug.Log("Instantiate a alertLayer:" + alert.ToString());
        Windows.Add(alert);


    }
    public void ShowLoadingAlert(kAlertTypes pType, KMasklayerCenter kMasklayerCenter = KMasklayerCenter.masklayer, Hashtable pParam = null)
    {
        Transform tran = transform;
        switch (kMasklayerCenter)
        {
            case KMasklayerCenter.masklayer:
                tran = transform;
                break;
            case KMasklayerCenter.control:
                tran = _gameManager._PCUIManager.control;
                break;
            case KMasklayerCenter.record:
                tran = _gameManager._PCUIManager.record;
                break;
            case KMasklayerCenter.warning:
                tran = _gameManager._PCUIManager.warning;
                break;
            case KMasklayerCenter.set:
                tran = _gameManager._PCUIManager.set;
                break;
            case KMasklayerCenter.Data:
                tran = _gameManager._PCUIManager.data;
                break;
            //case KMasklayerCenter.workingsurface:
            //    tran = _gameManager._modManager.WorkingSurface.transform;
              //  break;
            default:
                break;
        }
        gameObject.SetActive(true);
        GameObject alert = Instantiate(_prefabRelated[pType]);
        alert.transform.SetParent(tran);
        alert.transform.localPosition = Vector3.zero;
        alert.transform.localScale = Vector3.one;
        UIAlertLayer alertLayerClass = alert.GetComponent<UIAlertLayer>();
        alertLayerClass.Type = pType;
        alertLayerClass.Param = pParam;
        alertLayerClass.RefreshData(alertLayerClass.Param);

       // Debug.Log("打开loading界面");
        Windows.Add(alert);

        


    }
    public GameObject ShowAlertRepetiton(kAlertTypes pType, KMasklayerCenter kMasklayerCenter = KMasklayerCenter.masklayer, Hashtable pParam = null)
    {

        /*foreach (var VARIABLE in Windows)                                   //在windos窗口遍历              
		{
			if (VARIABLE.GetComponent<UIAlertLayer>().Type == pType)       ///在 UIAlertLayer 这个类里的Type等于 ptype，即是输入的第一个参数
			{
				VARIABLE.GetComponent<UIAlertLayer>().onClose();             //关闭这个窗口
			}
		}*/
        switch (kMasklayerCenter)
        {
            case KMasklayerCenter.masklayer:
                Container = transform;
                break;
            case KMasklayerCenter.control:
                Container = _gameManager._PCUIManager.control;
                break;
            case KMasklayerCenter.record:
                Container = _gameManager._PCUIManager.record;
                break;
            case KMasklayerCenter.warning:
                Container = _gameManager._PCUIManager.warning;
                break;
            case KMasklayerCenter.set:
                Container = _gameManager._PCUIManager.set;
                break;
            //case KMasklayerCenter.workingsurface:
            //    Container = _gameManager._modManager.WorkingSurface.transform;
               // break;
            default:
                break;
        }
        gameObject.SetActive(true);                                    ///本物体开启                        
		GameObject alert = Instantiate(_prefabRelated[pType], Container);             ///实例化ptype为名的预制体为alert
        //alert.transform.SetParent(Container);                                 ///将alert放入Container
        alert.transform.localPosition = Vector3.zero;                        ///初始化位置
		alert.transform.localScale = Vector3.one;
        UIAlertLayer alertLayerClass = alert.GetComponent<UIAlertLayer>();         ///定义一个uialertlayer的类
		alertLayerClass.Type = pType;                                              ///定义的这个类下的type等于输入的ptype
        alertLayerClass.Param = pParam;                                              ///哈希表pParam也放入这个类下
        alertLayerClass.RefreshData(alertLayerClass.Param);                          ///刷新该uialertlayer类的信息


        //Debug.Log("Instantiate a alertLayer:" + alert.ToString());                 ///
        Windows.Add(alert);                                                          ///桌面添加这个alet预制体
		return alert;                                                                ///返回alert
	}
    public GameObject ShowAlertOnly(kAlertTypes pType, KMasklayerCenter kMasklayerCenter = KMasklayerCenter.masklayer, Hashtable pParam = null, bool isStateBarAlert = false)
    {

        /*foreach (var VARIABLE in Windows)                                   //在windos窗口遍历              
		{
			if (VARIABLE.GetComponent<UIAlertLayer>().Type == pType)       ///在 UIAlertLayer 这个类里的Type等于 ptype，即是输入的第一个参数
			{
				VARIABLE.GetComponent<UIAlertLayer>().onClose();             //关闭这个窗口
			}
		}*/
        switch (kMasklayerCenter)
        {
            case KMasklayerCenter.masklayer:
                Container = transform;
                break;
            case KMasklayerCenter.control:
                Container = _gameManager._PCUIManager.control;
                break;
            case KMasklayerCenter.record:
                Container = _gameManager._PCUIManager.record;
                break;
            case KMasklayerCenter.warning:
                Container = _gameManager._PCUIManager.warning;
                break;
            case KMasklayerCenter.set:
                Container = _gameManager._PCUIManager.set;
                break;
            //case KMasklayerCenter.workingsurface:
            //    Container = _gameManager._modManager.WorkingSurface.transform;
                //break;
            case KMasklayerCenter.Data:
                Container = _gameManager._PCUIManager.data;
                break;
            case KMasklayerCenter.none:
                Container = null;
                break;
            default:
                break;
        }
        gameObject.SetActive(true);                                    ///本物体开启      
        GameObject alert;
        UIAlertLayer alertLayerClass;
        for (int i = 0; i < Windows.Count; i++)
        {
            if (Windows[i].GetComponent<UIAlertLayer>().Type == pType)       ///在 UIAlertLayer 这个类里的Type等于 ptype，即是输入的第一个参数
			{
               // Windows[i].GetComponent<UIAlertLayer>().onClose();             //关闭这个窗口
                alert = Windows[i];             
                //alert.transform.SetParent(Container);                                 ///将alert放入Container
                alert.transform.localPosition = Vector3.zero;                        ///初始化位置
                alert.transform.localScale = Vector3.one;
                alertLayerClass = alert.GetComponent<UIAlertLayer>();         ///定义一个uialertlayer的类
                alertLayerClass.Type = pType;                                              ///定义的这个类下的type等于输入的ptype
                alertLayerClass.Param = pParam;                                              ///哈希表pParam也放入这个类下
                alertLayerClass.RefreshData(alertLayerClass.Param);                          ///刷新该uialertlayer类的信息
                Windows.RemoveAt(i);
                Windows.Add(alert);
                //Debug.Log("Instantiate a alertLayer:" + alert.ToString());                 ///                                                      ///桌面添加这个alet预制体
                return alert;

            }
        }

        //	_gameManager.iscanSlide = false;
             
		alert = Instantiate(_prefabRelated[pType], Container);             ///实例化ptype为名的预制体为alert
		//alert.transform.SetParent(Container);                                 ///将alert放入Container
		alert.transform.localPosition = Vector3.zero;                        ///初始化位置
		alert.transform.localScale = Vector3.one;
        alertLayerClass = alert.GetComponent<UIAlertLayer>();         ///定义一个uialertlayer的类
		alertLayerClass.Type = pType;                                              ///定义的这个类下的type等于输入的ptype
        alertLayerClass.Param = pParam;                                              ///哈希表pParam也放入这个类下
        alertLayerClass.RefreshData(alertLayerClass.Param);                          ///刷新该uialertlayer类的信息
        //Debug.Log("Instantiate a alertLayer:" + alert.ToString());                 ///
        Windows.Add(alert);                                                          ///桌面添加这个alet预制体
		return alert;                                                                ///返回alert
	}
    public void RemoveAll()
    {
        foreach (var e in Windows)
        {
            Debug.Log(e.name);
            Destroy(e);
        }

        Windows.Clear();

        gameObject.SetActive(false);

    }

}
