using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Button yanzheng;
    public Text t;
    private bool begin = false;
    private float time = 5;
    void Start()
    {
    }
    public void StartCountDown()
    {
        gameObject.SetActive(true);
        begin = true;
        t.text = "60";
        time = 60;
    }
    
    void Update()
    {
        if (begin)
        {
            time -= Time.deltaTime;
            t.text = ((int)time+1).ToString();
            if (time<=0)
            {
                begin = false;
                yanzheng.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
