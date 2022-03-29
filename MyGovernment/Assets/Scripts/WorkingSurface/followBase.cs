using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class followBase : Define
{
  
    [Header("是否开启跟随摄像机旋转")] public bool isFollow = true;
    [Header("是否连线")] public bool isLine = false;

    //连线相关，记录开启与结束的位置
    public Transform startPos;
    public Transform endPos;
    [Header("赋值后将采用该材质，否则使用材质LineRender1")]
    public Material LineMaterial = null;
    private LineRenderer lineRenderer;
    
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
        LineInit();
     
    }



    public void LineInit()
    {
       
        if (isLine)
        {
            //  lineRenderer = gameObject.AddComponent<LineRenderer>();
            //设置材质
            // lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            // lineRenderer.material = Resources.Load<Material>("Materials/LineRender1");
            //设置颜色
            //  lineRenderer.SetColors(Color.red, Color.yellow);
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            Material meshRender = Resources.Load("Materials/LineRender1") as Material;
        

            if (LineMaterial == null)
            {
               
                if (meshRender == null)
                {
            
                    return;
                }
                else
                {
                    lineRenderer.GetComponent<Renderer>().sharedMaterial = meshRender;
                }
            }
            else
            {
                lineRenderer.GetComponent<Renderer>().sharedMaterial = LineMaterial;
            }
           


           
        }
    }
    public void lookAtCamera()
    {
        gameObject.transform.LookAt(Camera.main.transform.position);	
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - gameObject.transform.position ), 10 * Time.deltaTime);
                
       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isFollow)
        {
            lookAtCamera();
        }

    }

    private void FixedUpdate()
    {
        if (isLine)
        {
            drawLine();
        }
    }


    public void drawLine()
    {
      
        //设置宽度
       // lineRenderer.SetWidth(0.02f, 0.02f);
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, startPos.position);
        lineRenderer.SetPosition(1, endPos.position);
    }

public void MovetoUI()
{
Vector3 uiposition = gameObject.transform.position;
//_gameManager.controlCamera.MoveToPosition(uiposition);
}

//public void showWorkingsurface()
//{
//_maskLayer.ShowAlertOnly(kAlertTypes.workingSurface);
//}
}
