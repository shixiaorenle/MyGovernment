using System;
using System.Collections;
using System.Collections.Generic;
using UI.Dates;
using UnityEngine;

public class TestDate : MonoBehaviour
{
    
    void Start()
    {
        DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
        Debug.Log(firstDay.ToString());
        Debug.Log(firstDay.ToDateString());
        Debug.Log(lastDay.ToString());
        Debug.Log(lastDay.ToDateString());

        DateTime nowMonth = DateTime.Now;
        Debug.Log(nowMonth.ToString());
        DateTime lastYearMonth = nowMonth.AddMonths(-12);
        Debug.Log(lastYearMonth.ToString());

        for (int i = 0; i < 13; i++)
        {
            lastYearMonth = nowMonth.AddMonths(-i);
            Debug.Log(lastYearMonth.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
