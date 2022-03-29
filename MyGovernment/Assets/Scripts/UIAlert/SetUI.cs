using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetUI : UIAlertLayer
{
    public GameObject web;
    public RectTransform content;
    public RectTransform mid;
    public RectTransform logout;
    public RectTransform quit;
    protected override void Start()
    {
        base.Start();
        setMinSize();
    }

    public override void RefreshData(Hashtable hashtable)
    {
        GridLayoutGroup gg = mid.GetComponent<GridLayoutGroup>();
        mid.sizeDelta = new Vector2(mid.sizeDelta.x, (gg.cellSize.y + gg.spacing.y) * mid.childCount + gg.padding.top + gg.padding.bottom);
        VerticalLayoutGroup VG = content.GetComponent<VerticalLayoutGroup>();
        content.sizeDelta = new Vector2(content.sizeDelta.x, mid.sizeDelta.y + logout.sizeDelta.y + quit.sizeDelta.y+VG.padding.top+VG.padding.bottom+VG.spacing*content.childCount);
    }

    public void Return()
    {
        _dataManager.GetUserGreenHouseList();
    }
    public void Stop()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.Stop,KMasklayerCenter.masklayer);
    }
    public void Call()
    {
        string phoneNumber = _playerCache._playerData.cellPhone;
        Application.OpenURL("tel://" + phoneNumber);
    }
    public void MessageBoard()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.MessageBoard, KMasklayerCenter.set);
    }
    public void CloseAuto()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.CloseAuto, KMasklayerCenter.masklayer);
    }
    public void OpenWeb()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.WebUI, KMasklayerCenter.none);
    }
    public void OpenGuide()
    {

        _maskLayer.ShowAlertOnly(kAlertTypes.GuideUI, KMasklayerCenter.masklayer);


    }
    public void ResetLog()
    {
        _maskLayer.Container = _maskLayer.transform;
        
        _playerCache._Data = new SaveData();
        _playerCore.saveProfile();
        _maskLayer.Windows.Clear();
        //onClose();
        SceneManager.LoadScene("Enter");
    }
    public void UpdatePassword()
    {
       // _maskLayer.Container = _maskLayer.transform;
        _maskLayer.ShowAlertOnly(kAlertTypes.UpdatePassword, KMasklayerCenter.masklayer);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
