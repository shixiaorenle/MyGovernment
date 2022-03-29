

public enum kNotificationKeys
{
    refreshUI,//刷新UI
    resetCamera,
    webSocketCameraData,//刷新sockt实时数据
    closeLoading,

    /// <summary>
    /// 获取注册验证码
    /// </summary>
    getRegistVerification,
    /// <summary>
    /// 获取忘记密码验证码
    /// </summary>
    getForgetVerification,
    /// <summary>
    /// 检查验证码
    /// </summary>
    checkVerification,
    /// <summary>
    /// 注册用户返回
    /// </summary>
    regist,
    /// <summary>
    /// 修改密码返回
    /// </summary>
    updatepassword,
    /// <summary>
    /// 修改用户信息返回
    /// </summary>
    updateuser,
    /// <summary>
    /// 忘记密码修改密码返回
    /// </summary>
    updateforgetpassword,
    LogIn,
    backToMain,//返回主场景
    getGreenHouselist,//获取对应用户名的大棚列表
    /// <summary>
    /// 获取对应用户名的大棚信息
    /// </summary>
    getGreenHousedata,
    /// <summary>
    /// 获取对应用户名的大棚控制点
    /// </summary>
    getGreenHouseControlpoint,
    /// <summary>
    /// 获取对应用户名的大棚监控点以及设备状态
    /// </summary>
    getGreenHouseMonitoringpoint,
    /// <summary>
    /// 获取大棚气象数据
    /// </summary>
    getGreenHouseWeather,
    /// <summary>
    /// 控制设备时获取监控点信息
    /// </summary>
    getMonitoringInfo,
    /// <summary>
    /// 获取操作记录
    /// </summary>
    getRecord,
    /// <summary>
    /// 添加大棚完毕
    /// </summary>
    addGreenHouse,
    /// <summary>
    /// 删除大棚完毕
    /// </summary>
    deleteGreenHouse,
    /// <summary>
    /// 上传用户的信息
    /// </summary>
    postuserdata,
    /// <summary>
    /// 返回主场景
    /// </summary>
    auto_manaul,
    /// <summary>
    /// 获取所有大棚数据
    /// </summary>
    getHouseData,
    /// <summary>
    /// 刷新所有大棚数据
    /// </summary>
    refreshHouseData,
    /// <summary>
    /// 获取所有报警数据
    /// </summary>
    getWarningData,
    /// <summary>
    /// 获取摄像头信息
    /// </summary>
    getCameradata,
    /// <summary>
    /// 获取摄像头URL
    /// </summary>
    getCameraURL,
    /// <summary>
    /// 获取天气信息
    /// </summary>
    getWeather,
    /// <summary>
    /// 获取报警列表
    /// </summary>
    getAlarmList,
    /// <summary>
    /// 获取该分组下所有报警联系人
    /// </summary>
    getAlarmDetail,
    /// <summary>
    /// 修改报警联系人是否通知
    /// </summary>
    changeLinkmanInfo,
    /// <summary>
    /// 删除报警联系人
    /// </summary>
    deleteLinkman,
    /// <summary>
    /// 添加报警联系人
    /// </summary>
    addLinkman,
    /// <summary>
    /// 删除报警联系人完成
    /// </summary>
    deleteLinkmanOver,
    /// <summary>
    /// 获取当前有哪些报警触发
    /// </summary>
    getAlarm,
    /// <summary>
    /// /// <summary>
    /// 循环获取报警
    /// </summary>
    realTimeAlarm,
    /// <summary>
    /// 获取报警历史
    /// </summary>
    getAlarmHistory,
    /// <summary>
    /// 刷新报警历史
    /// </summary>
    refreshAlarmHistory,
    /// <summary>
    /// 确认报警
    /// </summary>
    confirmAlarm,
    /// <summary>
    /// 刷新报警状态
    /// </summary>
    refreshAlarm,
    /// <summary>
    /// 控制设备返回
    /// </summary>
    controlPoint,
    /// <summary>
    /// 设备1统一写值返回
    /// </summary>
    controlAllDeviceOne,
    /// <summary>
    /// 设备1统一写值返回
    /// </summary>
    controlAllDeviceTwo,
    /// <summary>
    /// 是否延迟控制返回
    /// </summary>
    controlDelay,
    /// <summary>
    /// 刷新大棚中心首页信息返回
    /// </summary>
    getHouseItemState,
    /// <summary>
    /// 开启设备一定时间后停止
    /// </summary>
    delayStopDevice,
    /// <summary>
    /// 取消延时停止
    /// </summary>
    cancelDelayStopDevice,
    /// <summary>
    /// 过了设定时间后停止设备
    /// </summary>
    delayStopDeviceOver,
    /// <summary>
    /// 获取单个设备状态
    /// </summary>
    getSingleDeviceState,
    /// <summary>
    /// 返回按钮
    /// </summary>
    androidReturn,
    /// <summary>
    /// 扫描结果
    /// </summary>
    scan,
    /// <summary>
    /// 筛选日期
    /// </summary>
    selectDate,
    none
   

}
public enum kAlertTypes
{
    UITips,
    loading,
    Center,//大棚中心
    //Control,//控制中心
    Record,//操作记录
    RecordUI,//单个操作
    Alarm,//报警中心
    WarningHistory,//报警列历史
    WarningDetail,//单个报警历史
    WarningList,//报警列表
    WarningConfirm,//单个报警历史
    WarningEdit,//报警编辑
    DeleteLinkman,//删除单个报警联系人
    Control,//控制中心
    DeleteHouse,//删除单个大棚
    MaxVideo,//全屏视频框
    Set,//我的设置
    Other,//其他功能
    Stop,//紧急制动
    CloseAuto,//关闭自动控制
    Parameter,//设置参数
    Regist,//注册
    ForgetPassword,//忘记密码
    UpdatePassword,//重置密码
    AddGreenHouse,//添加大棚
    AddLinkman,//添加联系人
    Workingsurface,//3d工作面
    WebUI,//网页
    GuideUI,//操作指南
    Data,//数据中心
    Photo,//扫描二维码
    Date,//筛选日期
    MessageBoard,//留言板
    none
}
public enum kMod
{
    Main2D,
    WorkingSurface
}
public enum ToggleType
{
    warning, control, set,record,none,working,data
}
public enum KDevicetype
{
    
    juanlianji1,
    juanlianji2,
    tongfengkou1,
    tongfengkou2,
    tongfengkou3,
    tongfengkou4,
    buguangdeng,
    CO2,
    shuifei,
    dayao,
    none
}
public enum KMasklayerCenter
{
    masklayer,workingsurface,control,record,warning,set,none, Data
}
public enum KSensor
{
    temperature1, temperature2,hum,soiltem,soilhum,light,CO2,N,P,K,ec,main,PH
}
