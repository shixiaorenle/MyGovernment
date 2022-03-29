using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
[RequireComponent(typeof(AspectRatioFitter))]
public class PhotoUI : UIAlertLayer
{
    [HideInInspector]
    public Color32[] data;
    private bool isScan;
    public GameObject all;
    public RawImage cameraTexture;
    private WebCamTexture webCameraTexture;
    private BarcodeReader barcodeReader;
    private float timer = 0;
    private bool isOk = false;
    private bool open = false;
    int width;
    private int difuseIndex;
    public AspectRatioFitter fit;
    protected override void Start()
    {
        base.Start();
        setMinSize();
        all.SetActive(false);
    }
    IEnumerator ScanQRcode()
    {
       // data
        data = webCameraTexture.GetPixels32();
        DecodeQR(webCameraTexture.width, webCameraTexture.height);
        yield return new WaitForEndOfFrame();
    }

    public override void RefreshData(Hashtable hashtable)
    {
        difuseIndex = 0;
        open = true;
        //StartCoroutine(Photo());
    }
    private void DecodeQR(int width, int height)
    {
        var br = barcodeReader.Decode(data, width, height);
        if (br != null)
        {
            string str = br.Text;
            string[] array = str.Split(' ');
            Hashtable hashtable = new Hashtable();

            string sn = array[0];
            Regex regex = new Regex("^(-?[0-9]*[.]*[0-9]{0,3})$");
            bool b = regex.IsMatch(sn);
            if (b == true)
            {
                // Debug.Log("是纯数字");
                hashtable["SN"] = sn;
                PostNotification(kNotificationKeys.scan, hashtable);
                isScan = false;
                onClose();
            }
            else
            {
                _maskLayer.ShowAlertWithTips("扫描错误,请重新扫描");
                onClose();
            }
            MMVibrationManager.Haptic(HapticTypes.Selection);
        }

    }
    public override void onClose()
    {
        StopAllCoroutines();
        isScan = false;
        isOk = false;
        open = false;
        difuseIndex = 0;
        if (webCameraTexture)
        {
            webCameraTexture.Stop();
        }
        
        base.onClose();
    }
    void ScreenChange()//屏幕横竖屏切换
    {
        if (width == Screen.width)
            return;
        width = Screen.width;

        if (width > Screen.height)
        {
            cameraTexture.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            cameraTexture.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
    }

    private IEnumerator Photo()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if (!_playerCache._playerData.getCameraAuthorization)
            {
                _maskLayer.ShowAlertWithTips("请打开摄像头使用权限后重试");
                onClose();
            }
            else
            {
                barcodeReader = new BarcodeReader();

                all.SetActive(true);

                WebCamDevice[] devices = WebCamTexture.devices;
                string devicename = devices[0].name;
                webCameraTexture = new WebCamTexture(devicename, Screen.width, Screen.height, 15);

                cameraTexture.texture = webCameraTexture;
                Debug.Log(7);
                webCameraTexture.Play();

                isScan = true;
            }
        }
            

    }
    protected override void Update()
    {
        if (webCameraTexture == null)
        {
            if (open)
            {

                barcodeReader = new BarcodeReader();
                
                WebCamDevice[] devices = WebCamTexture.devices;

                if (_playerCache._playerData.getCameraAuthorization)
                {
                    if (devices[0].name != null && devices[0].name != "")
                    {
                        all.SetActive(true);
                        string devicename = devices[0].name;
                        webCameraTexture = new WebCamTexture(devicename, Screen.width, Screen.height, 15);
                        cameraTexture.texture = webCameraTexture;
                        webCameraTexture.Play();
                        open = false;
                        isScan = true;
                    }
                }
                else
                {
                    //_maskLayer.ShowAlertWithTips("请打开摄像头使用权限后重试");
                    difuseIndex++;
                    if (difuseIndex >= 5)
                    {

                        onClose();
                    }

                }


            }
        }

        if (isScan)
        {
            AdjustmentRawimage();
            timer += Time.deltaTime;

            if (timer > 1f) //1秒扫描一次
            {
                
                StartCoroutine(ScanQRcode());
                timer = 0;
            }
        }
        //ScreenChange(); 
        

    }
    void AdjustmentRawimage()
    {

        if (cameraTexture == null) return;

        float ratio = (float)webCameraTexture.width / (float)webCameraTexture.height;
        fit.aspectRatio = ratio; // Set the aspect ratio  

       // fit.aspectRatio = 1;
        float scaleY = webCameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not  
        cameraTexture.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera  


        int orient = -webCameraTexture.videoRotationAngle;
        cameraTexture.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

    }
}
