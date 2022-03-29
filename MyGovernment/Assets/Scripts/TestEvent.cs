using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TestEvent : MonoBehaviour
{
    public EventTrigger eventTrigger;
    void Start()
    {
        Test();
    }

    void Test()
    {
        UnityAction<BaseEventData> click = new UnityAction<BaseEventData>(OnEnter);
        EventTrigger.Entry myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerEnter;
        myclick.callback.AddListener(click);
        eventTrigger.triggers.Add(myclick);

        click = new UnityAction<BaseEventData>(OnExit);
        myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerExit;
        myclick.callback.AddListener(click);
        eventTrigger.triggers.Add(myclick);

        click = new UnityAction<BaseEventData>(OnClickUp);
        myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerClick;
        myclick.callback.AddListener(click);
        eventTrigger.triggers.Add(myclick);

        click = new UnityAction<BaseEventData>(OnClickDown);
        myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.PointerDown;
        myclick.callback.AddListener(click);
        eventTrigger.triggers.Add(myclick);
    }

    private void OnClickDown(BaseEventData arg0)
    {
        Debug.Log("点击按下");
    }

    private void OnClickUp(BaseEventData arg0)
    {
        Debug.Log("点击抬起");
    }

    private void OnExit(BaseEventData arg0)
    {
        Debug.Log("退出");
    }

    private void OnEnter(BaseEventData arg0)
    {
        Debug.Log("进入");
    }

    void Update()
    {
        
    }
}
