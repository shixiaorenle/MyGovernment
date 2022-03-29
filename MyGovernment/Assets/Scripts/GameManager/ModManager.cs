using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModManager : Define
{
    /// <summary>
    /// 现在的模块
    /// </summary>
    public kMod currentMod = kMod.Main2D;
    [Space(   )] 
   
    /// <summary>
    /// 模块字典
    /// </summary>
    /// <typeparam name="kMod"></typeparam>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    private Dictionary<kMod,GameObject> ModDict = new Dictionary<kMod, GameObject>();

    public CinemachineBrain myBrain;
    #region 各个模块


    [Header("各个子场景组件")]
    [Tooltip("主场景")]
    public GameObject ModMain2d;
    //public GameObject WorkingSurface;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InitData();
        ChangeMod(currentMod);




    }
    /// <summary>
    /// 初始化内容
    /// </summary>
    void InitData()
    {

       _gameManager._modManager = this;
       ModDict.Clear();
       ModDict.Add(kMod.Main2D,ModMain2d);
        //ModDict.Add(kMod.WorkingSurface, WorkingSurface);

    }                                                                                                                                                                                 

    void Update()
    {
       
    }
    public void ChangeCameraMode()
    {
        if (currentMod.Equals(kMod.Main2D))
        {
            myBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseIn, 1);
            
        }
        else
        {
            myBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut,0);
        }
    }
    /// <summary>
    /// 改变模块
    /// </summary>
    /// <param name="nowmod"></param>
    private void ChangeMod(kMod nowmod)
    {

        currentMod = nowmod;
        myBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0);
       // ChangeCameraMode();
        foreach (var item in ModDict)
        {
            if (item.Key.Equals(currentMod))
            {
                item.Value.SetActive(true);
            }
            else
            {
                item.Value.SetActive(false);
            }
        }

    }
    /// <summary>
    /// 点击跳转按钮方法
    /// </summary>
    /// <param name="kmodName"></param>
    public void ChangeModstr(string kmodName)
    {
        ChangeMod( ParseEnum<kMod>(kmodName));
    }



}
