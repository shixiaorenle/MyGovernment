using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestButton : MonoBehaviour
{
    public InputField inputField;
    private bool iii = true;
    void Start()
    {
        
    }

    public void Down()
    {
        if (iii)
        {
            inputField.contentType = InputField.ContentType.Standard;
        }
        else
        {
            inputField.contentType = InputField.ContentType.Password;
        }
        iii = !iii;
    }
    public void Up()
    {
        
    }
    void Update()
    {
        
    }
}
