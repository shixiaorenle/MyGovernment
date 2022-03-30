using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCUIManager : Define
{

    public Animator ani;

    public HouseMain houseMain;

    public Transform control;
    public Transform record;
    public Transform warning;
    public Transform data;
    public Transform set;
    private void Awake()
    {
        _gameManager._PCUIManager = this;
    }
    //UI界面的动画。
    public void Showlist(bool showList)
    {
        ani.SetBool("ShowList", showList);
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
