using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class Globe
{
    public static string nextSceneName =  "Main";
    public static string SceneNametext ="";
}

public class AsyncLoadScene : MonoBehaviour
{
    public Slider loadingSlider;
    public Text loadingscencename;
    public CanvasGroup logo;
    private float timedetails = 0;

    private float loadingSpeed = 1;
    private float targetValue = 0f;
    private AsyncOperation operation;



    void Start()
    {


        //清理Unity垃圾  
        Resources.UnloadUnusedAssets();
        //清理GC垃圾  
        System.GC.Collect();


        logo.alpha = 0f;
        loadingSlider.value = 0.0f;

        if (SceneManager.GetActiveScene().name == "Load")
        {

            //启动协程
            StartCoroutine(AsyncLoading());
        }
    }

    IEnumerator AsyncLoading()
    {
        operation = SceneManager.LoadSceneAsync(Globe.nextSceneName);
        //阻止当加载完成自动切换
        operation.allowSceneActivation = false;

        yield return operation;
    }

    // Update is called once per frame
    void Update()
    {
        if (operation == null) return;
        targetValue = operation.progress;

        if (operation.progress >= 0.9f)
        {
            //operation.progress的值最大为0.9
            targetValue = 1.0f;
        }

        if (targetValue != loadingSlider.value)
        {
            //插值运算
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, targetValue, Time.deltaTime * loadingSpeed);
            logo.alpha  = Mathf.Lerp(loadingSlider.value, targetValue, Time.deltaTime * loadingSpeed);
            if (Mathf.Abs(loadingSlider.value - targetValue) < 0.01f)
            {
                loadingSlider.value = targetValue;
                logo.alpha = targetValue;
            }
        }

        //loadingText.text = ((int)(loadingSlider.value * 100)).ToString() + "%";
        loadingscencename.text = "正在加载"+Globe.SceneNametext+ ((int)(loadingSlider.value * 100)).ToString() + "%";

        if ((int)(loadingSlider.value * 100) == 100)
        {
            //允许异步加载完毕后自动切换场景
            operation.allowSceneActivation = true;
        }



    }
}
