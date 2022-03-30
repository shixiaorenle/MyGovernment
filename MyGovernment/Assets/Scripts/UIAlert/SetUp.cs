using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetUp : UIAlertLayer
{
    //每个标签具体显示的内容
    public InputField userName;
    public Text firstStr;

    [Header("加载组件")]
    [Tooltip("控制台开启按钮")]
    public Toggle consoleToggle;
    new void Start()
    {
        userName.text = _playerCache._playerData._User.userName;
        if (userName.text.Length>0)
        firstStr .text= userName.text.Remove(1);
        userName.onEndEdit.AddListener((string str) => {
            if (!lengthOverLimit(userName, 1, 8))
            {
                _maskLayer.ShowAlertWithTips("输入长度有误");
                userName.text = _playerCache._playerData._User.userName;
            }

            else if (containsSpace(str, userName))
            {
                userName.text = _playerCache._playerData._User.userName;

            }
        });
        if (_gameManager.debugMode)
        {
            consoleToggle.isOn = true;
        }
        else
        {
            consoleToggle.isOn = false;
        }
    }
    /// <summary>
    /// 退出
    /// </summary>
    public void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
 
    }
    
    public void confirmSave()
    {
        if (_playerCache._playerData._User.userName!= userName.text)
        {
            _playerCache._playerData._User.userName = userName.text;
            Hashtable hashtable = new Hashtable();
            hashtable["id"] = _playerCache._playerData._User.userID;
            hashtable["userName"] = userName.text;
            _dataManager.updateUser(hashtable);
            _playerCore.saveProfile();
        }

        if (consoleToggle.isOn)
        {
            _gameManager.debugMode = true;

        }
        else
        {
            _gameManager.debugMode = false;

        }
        onClose();
    }
   
    // Start is called before the first frame update
   

    /// <summary>
    /// 开启外部软件cmd
    /// </summary>
    public void openPowerCMD()
    {

        string path = Application.streamingAssetsPath + "//PowerCmd//PowerCmd.exe";
        Application.OpenURL(path);
    }

    public void Resetlog()
    {
        onClose();
        SceneManager.LoadScene("Enter");
    }
}
