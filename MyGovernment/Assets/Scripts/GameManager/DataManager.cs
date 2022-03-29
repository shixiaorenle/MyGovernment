using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using BestHTTP;

public class DataManager : Define
{


    public void startConnect()
    {
        //getBasicData();
        //GetWeatherData();
    }
    #region 注册获取手机验证码
    public void getRegistVerificationCode(Hashtable hash)
    {
        //        Debug.Log("获取验证码");
        RequestHttp(_playerCache._playerData.HttpIPAddress+IpAddressUrl.Verify, hash, getRegistVerificationCodeFinished, true);
    }
    public void getRegistVerificationCodeFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getRegistVerification);
        }, delegate
        {

        });
    }
    #endregion
    #region 忘记密码获取手机验证码
    public void getForgetVerificationCode(Hashtable hash)
    {
        //        Debug.Log("获取验证码");
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.Verify, hash, getForgetVerificationCodeFinished, true);
    }
    public void getForgetVerificationCodeFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getForgetVerification);
        }, delegate
        {

        });
    }
    #endregion
    #region 忘记密码修改密码
    public void updateForgetPassword(Hashtable hash)
    {
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.Update, hash, updateForgetPasswordFinished, true);
    }
    public void updateForgetPasswordFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.updateforgetpassword);
        }, delegate
        {

        });
    }
    #endregion
    #region 检查手机验证码
    public void checkVerificationCode(Hashtable hash)
    {
        //        Debug.Log("获取验证码");
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.Check, hash, checkVerificationCodeFinished, true);
    }
    public void checkVerificationCodeFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.checkVerification);
        }, delegate
        {

        });
    }
    #endregion
    #region 新增用户
    public void addUser(Hashtable hash)
    {
        //        Debug.Log("新增用户");
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.Regist, hash, addUserFinished, true);
    }
    public void addUserFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.regist,data);
        }, delegate
        {

        });
    }
    #endregion
    #region 修改密码
    public void updatePassword(Hashtable hash)
    {
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.Update, hash, updatePasswordFinished, true);
    }
    public void updatePasswordFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.updatepassword,data);
        }, delegate
        {

        });
    }
    #endregion
    #region 修改用户信息
    public void updateUser(Hashtable hash)
    {
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.UpdateUser, hash, updateUserFinished, true);
    }
    public void updateUserFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.updateuser,data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取对应用户名保存的大棚列表
    public void GetUserGreenHouseList(Hashtable hash=null)
    {
        if (hash==null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetGreenHouseList, hash, OnRequestGetListFinished, true);
    }
    void OnRequestGetListFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getGreenHouselist, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取单个大棚信息
    public void GetUserGreenHouseData(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetGreenHouseData, hash, OnRequestGetDataFinished, true);
    }
    void OnRequestGetDataFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getGreenHousedata, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取单个大棚所有点
    public void GetUserGreenHouseControlPoint(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetGreenHouseControlPoint, hash, OnRequestGetControlPointFinished,true);
    }
    void OnRequestGetControlPointFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getGreenHouseControlpoint, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取单个大棚开关时间参数
    public void GetUserGreenHouseParam(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }

        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetGreenHouseState, hash, OnRequestGetMonitoringPointFinished, true);

    }
    void OnRequestGetMonitoringPointFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getGreenHouseMonitoringpoint, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取单个大棚气象数据
    public void GetUserWeatherParam(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }

        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetGreenHouseState, hash, OnRequestGetUserWeatherParamFinished, true);

    }
    void OnRequestGetUserWeatherParamFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getGreenHouseWeather, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取单个设备状态信息
    public void GetDeviceState(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }

        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetGreenHouseState, hash, OnRequestGetDeviceStateFinished, false);

    }
    void OnRequestGetDeviceStateFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getSingleDeviceState, data);
        }, delegate
        {

        },false);
    }
    #endregion
    #region 刷新大棚首页HouseItem状态信息
    public void GetHouseItemState(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
       // Debug.Log(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetSingleHouseState);
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetSingleHouseState, hash, OnRequestGetHouseItemStateFinished, false);

    }
    void OnRequestGetHouseItemStateFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getHouseItemState, data);
        }, delegate
        {

        },false);
    }
    #endregion
    #region 查询操作记录
    public void GetRecord(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.controlRecord, hash, OnRequestGetRecordFinished,true);
    }
    void OnRequestGetRecordFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getRecord, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 添加大棚
    public void addGreenHouse(Hashtable hash)
    {
                Debug.Log("新增大棚");
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.AddDateGreenHouse, hash, addGreenHouseFinished, true);
    }
    public void addGreenHouseFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.addGreenHouse);
        }, delegate
        {

        });
    }
    #endregion
    #region 删除大棚
    public void deleteGreenHouse(Hashtable hash)
    {
        Debug.Log("删除大棚");
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.DeleteGreenHouse, hash, deleteGreenHouseFinished, true);
    }
    public void deleteGreenHouseFinished(HTTPRequest request, HTTPResponse response)
    {
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.deleteGreenHouse);
        }, delegate
        {

        });
    }
    #endregion
    #region 更新用户的大棚信息
    public void PostUserData(Hashtable userdata)
    {
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.upDateGreenHouseData, userdata, OnRequestPostDataFinished, true);
    }
    void OnRequestPostDataFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.postuserdata, data);

        }, delegate
        {

        });
    }
    #endregion

    #region 获取视频监控点信息
    public void GetCameraData(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetCameraData, hash, OnRequestGetCameraDataFinished, true);
    }
    void OnRequestGetCameraDataFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getCameradata, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 筛选视频监控点信息
    public void GetMyCameraData(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        Debug.Log(hash.toJson());
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetlistMyCamera, hash, OnRequestGetMyCameraDataFinished, true);
    }
    void OnRequestGetMyCameraDataFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            Debug.Log(data.toJson());
            PostNotification(kNotificationKeys.getCameradata, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取监控点的rtsp流
    public void GetCameraRTSP(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetCameraRTSP, hash, OnRequestGetCameraURLFinished, true);
    }
    void OnRequestGetCameraURLFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getCameraURL, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取监控点的hls流
    public void GetCameraHLS(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetCameraHLS, hash, OnRequestGetCameraHLSLFinished, true);
    }
    void OnRequestGetCameraHLSLFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getCameraURL, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 控制摄像头
    public void ControlCamera(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.controlCamera, hash, OnRequestControlCameraFinished,false);
    }
    void OnRequestControlCameraFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            //PostNotification(kNotificationKeys.getCameradata, data);
            Debug.Log(data.toJson());
        }, delegate
        {

        },false);
    }
    #endregion
    #region 控制设备
    public void ControlDevice(Hashtable hash)
    {
        Debug.Log(hash.toJson());
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.controlDevice, hash, OnRequestControlDeviceFinished,true);
    }
    void OnRequestControlDeviceFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.controlPoint, data);
            //Debug.Log(data.toJson());
        }, delegate
        {

        });
    }
    #endregion
    #region 是否开启延迟
    public void ControlDelay(Hashtable hash)
    {
        Debug.Log(hash.toJson());
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.controlDevice, hash, OnRequestControlDelayFinished, true);
    }
    void OnRequestControlDelayFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.controlDelay, data);
            //Debug.Log(data.toJson());
        }, delegate
        {

        });
    }
    #endregion
    #region 卷帘卷膜统一写值
    public void ControlAllDeviceOne(Hashtable hash)
    {
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.controlAllDevice, hash, OnRequestControlAllDeviceOneFinished, true);
    }
    void OnRequestControlAllDeviceOneFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.controlAllDeviceOne, data);
            //Debug.Log(data.toJson());
        }, delegate
        {

        });
    }
    #endregion
    #region 设备2统一写值
    public void ControlAllDeviceTwo(Hashtable hash)
    {
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.controlAllDevice, hash, OnRequestControlAllDeviceTwoFinished, true);
    }
    void OnRequestControlAllDeviceTwoFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.controlAllDeviceTwo, data);
            //Debug.Log(data.toJson());
        }, delegate
        {

        });
    }
    #endregion
    #region  登录
    public void LogIn(string userName, string passWorld)
    {
        Hashtable postHash = new Hashtable();
       postHash["accountName"] = userName;
        //postHash["password"] = AesEncrypt(passWorld);
        postHash["password"] = passWorld;
        //postHash["len"] = StringLength(passWorld);
       // Debug.Log(postHash.toJson());
        //Debug.Log(_playerCache._playerData.HttpIPAddress + IpAddressUrl.LogIn);
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.LogIn, postHash, OnRequestLogInFinished, true);
        
        //RequestHttp("http://192.168.43.85:8088/login", "{\"accountName\":\"admin\",\"password\":\"123456\"}".hashtableFromJson(), OnRequestLogInFinished, true);

    }

    void OnRequestLogInFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.LogIn, data);

        }, delegate
        {

        });
    }

    #endregion
    #region 天气信息
    public void GetWeatherData(Hashtable hash=null)
    {
        if (hash==null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress+IpAddressUrl.GetWeather, hash, OnRequestGetWeatherDataFinished, false);
    }
    void OnRequestGetWeatherDataFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getWeather, data);
        }, delegate
        {

        },false);
    }
    #endregion
    #region 获取所有报警条目
    public void GetAlarmList(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.boxAlarmList, hash, OnRequesGetAlarmListFinished, true);
    }
    void OnRequesGetAlarmListFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getAlarmList, data);
        }, delegate
        {

        }, true);
    }
    #endregion
    #region 报警中心获取报警信息
    public void GetAlarm(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.boxAlarm, hash, OnRequestGetAlarmFinished, true);
    }
    void OnRequestGetAlarmFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getAlarm, data);
        }, delegate
        {

        }, true);
    }
    #endregion
    #region 轮询获取报警信息（现在不用了，因为有短信）
    public void RealTimeAlarm(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.boxAlarm, hash, OnRequestRealTimeAlarmFinished, false);
    }
    void OnRequestRealTimeAlarmFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.realTimeAlarm, data);
        }, delegate
        {

        }, false);
    }
    #endregion
    #region Alarm界面获取报警历史
    public void GetAlarmHistory(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.boxAlarmRecord, hash, OnRequestGetAlarmHistoryFinished, true);
    }
    void OnRequestGetAlarmHistoryFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getAlarmHistory, data);
        }, delegate
        {

        }, true);
    }
    #endregion
    #region Warning界面获取报警历史
    public void RefreshAlarmHistory(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.boxAlarmRecord, hash, OnRequestRefreshAlarmHistoryFinished, true);
    }
    void OnRequestRefreshAlarmHistoryFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.refreshAlarmHistory, data);
        }, delegate
        {

        }, true);
    }
    #endregion
    #region 确认报警
    public void ConfirmAlarm(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.confirmBoxAlarm, hash, OnRequestConfirmAlarmFinished, true);
    }
    void OnRequestConfirmAlarmFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.confirmAlarm, data);
        }, delegate
        {

        }, true);
    }
    #endregion
    #region 控制设备时获取气象信息
    public void GetWeatherInfo(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.GetGreenHouseState, hash, OnRequestGetMonitoringInfoFinished, true);
    }
    void OnRequestGetMonitoringInfoFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getMonitoringInfo, data);
        }, delegate
        {

        });
    }
    #endregion
    #region 获取该分组下所有报警联系人
    public void GetAlarmDetail(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.boxAlarmDetail, hash, OnRequesGetAlarmDetailFinished, true);
    }
    void OnRequesGetAlarmDetailFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.getAlarmDetail, data);
        }, delegate
        {

        }, true);
    }
    #endregion
    #region 修改报警联系人是否通知
    public void ChangeGetInfo(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.boxAlarmLinkman, hash, OnRequesChangeGetInfoFinished, true);
    }
    void OnRequesChangeGetInfoFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.changeLinkmanInfo, data);
        }, delegate
        {

        }, true);
    }
    #endregion
    #region 删除联系人
    public void DeleteLinkman(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.DeleteLinkman, hash, OnRequesDeleteLinkmanFinished, true);
    }
    void OnRequesDeleteLinkmanFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.deleteLinkman, data);
        }, delegate
        {

        }, true);
    }
    #endregion
    #region 添加联系人
    public void AddLinkman(Hashtable hash = null)
    {
        if (hash == null)
        {
            hash = new Hashtable();
        }
        RequestHttp(_playerCache._playerData.HttpIPAddress + IpAddressUrl.AddLinkman, hash, OnRequesAddLinkmanFinished, true);
    }
    void OnRequesAddLinkmanFinished(HTTPRequest request, HTTPResponse response)
    { // 请求回调
        OnRequestFinished(request, response, delegate (Hashtable data)
        {
            PostNotification(kNotificationKeys.addLinkman, data);
        }, delegate
        {

        }, true);
    }
    #endregion
}