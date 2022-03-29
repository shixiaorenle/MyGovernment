using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IpAddressUrl : Define
{
    /// <summary>
    /// 登录接口
    /// </summary>
    public static string LogIn = "login";


    /// <summary>
    /// 注册接口
    /// </summary>
    public static string Regist = "system/userInfo/register";
    /// <summary>
    /// 获取手机验证码接口
    /// </summary>
    public static string Verify = "sms/send";
    /// <summary>
    /// 检查手机验证码接口
    /// </summary>
    public static string Check = "sms/check";
    /// <summary>
    /// 修改密码接口
    /// </summary>
    public static string Update = "account/update";
    /// <summary>
    /// 修改用户名
    /// </summary>
    public static string UpdateUser = "/system/userInfo/updateById";


    /// <summary>
    /// 添加大棚
    /// </summary>
    public static string AddDateGreenHouse = "bigArch/add";
    /// <summary>
    /// 获取大棚列表
    /// </summary>
    public static string GetGreenHouseList = "bigArch/list";
    /// <summary>
    /// 获取单个大棚详情
    /// </summary>
    public static string GetGreenHouseData = "bigArch/listById";
    /// <summary>
    /// 删除大棚
    /// </summary>
    public static string DeleteGreenHouse = "bigArch/delete";
    /// <summary>
    /// 更新大棚数据
    /// </summary>
    public static string upDateGreenHouseData = "bigArch/update";




    /// <summary>
    /// 获取单个大棚控制点
    /// </summary>
    public static string GetGreenHouseControlPoint = "boxItem/list";
    /// <summary>
    /// 控制大棚设备
    /// </summary>
    public static string controlDevice = "boxItem/op";
    /// <summary>
    /// 大棚设备统一写值
    /// </summary>
    public static string controlAllDevice = "boxItem/listOp";
    /// <summary>
    /// 控制记录查询
    /// </summary>
    public static string controlRecord = "itemOpHistory/list";
    /// <summary>
    /// 获取单个大棚监控点
    /// </summary>
    public static string GetGreenHouseState = "boxItem/read";
    /// <summary>
    /// 获取单个大棚状态
    /// </summary>
    public static string GetSingleHouseState = "box/detail";




    /// <summary>
    /// 所有报警条目查询(有哪些报警，报警规则)
    /// </summary>
    public static string boxAlarmList = "boxAlarm/list";
    /// <summary>
    /// 当前报警状态查询
    /// </summary>
    public static string boxAlarm = "boxAlarm/defs";
    /// <summary>
    /// 确认报警
    /// </summary>
    public static string confirmBoxAlarm = "boxAlarm/confirm";
    /// <summary>
    /// 报警历史查询
    /// </summary>
    public static string boxAlarmRecord = "boxAlarm/datas";
    /// <summary>
    /// 获取该分组下所有报警联系人
    /// </summary>
    public static string boxAlarmDetail = "boxAlarm/group/detail";
    /// <summary>
    /// 修改报警联系人是否通知
    /// </summary>
    public static string boxAlarmLinkman = "boxAlarm/group/contacts/enabled";
    /// <summary>
    /// 删除报警联系人
    /// </summary>
    public static string DeleteLinkman = "boxAlarm/group/contacts/delete";
    /// <summary>
    /// 添加报警联系人
    /// </summary>
    public static string AddLinkman = "boxAlarm/group/contacts/add";
    /// <summary>
    /// 获取摄像头监控点信息
    /// </summary>
    public static string GetCameraData = "camera/listCamera";
    /// <summary>
    /// 筛选摄像头监控点信息
    /// </summary>
    public static string GetlistMyCamera = "camera/listMyCamera";
    /// <summary>
    /// 获取摄像头监控点下rtsp地址
    /// </summary>
    public static string GetCameraRTSP = "camera/getRtspUrl";
    /// <summary>
    /// 获取摄像头监控点下HLS地址
    /// </summary>
    public static string GetCameraHLS = "camera/getHlsUrl";
    /// <summary>
    /// 控制摄像头
    /// </summary>
    public static string controlCamera = "camera/controlling";

    /// <summary>
    /// 获取天气信息
    /// </summary>
    public static string GetWeather = "weather/getCurrentWeather_v2";

    /// <summary>
    /// 获取操作指南
    /// </summary>
    public static string GetGuideFile = "file/agr_file/大棚接口12.07(1).pdf";
    public static string Weather = "http://api.k780.com/?app=weather.city&areaType=cn&appkey=59701&sign=7cbd3622746c6319cd2fed48bfbee8c1&format=json";
}
