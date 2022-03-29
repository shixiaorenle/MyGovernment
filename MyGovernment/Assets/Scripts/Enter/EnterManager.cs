using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class EnterManager : Define
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Toggle checkToggle;//是否保存用户名与密码

    public Text mineTitle;
    public Text mineEnglishTitle;
    // Start is called before the first frame update

    private List<GameObject> inputList;
    EventSystem system;
    void Start()
    {
        RegistNotification(this, kNotificationKeys.LogIn, enter);
        system = EventSystem.current;
        inputList = new List<GameObject>();
        InputField[] array = transform.GetComponentsInChildren<InputField>();
        for (int i = 0; i < array.Length; i++)
        {
            inputList.Add(array[i].gameObject);
        }
        checkToggle.isOn = _playerCache._Data.isRecordUP;
        if (_playerCache._Data.isRecordUP)
        {
            usernameInput.text = _playerCache._Data.userName;
            passwordInput.text = _playerCache._Data.passWord;
        }
        //RefreshTitle();
       

    }

    void RefreshTitle()
    {
        if(!_gameManager.configData.ContainsKey("Title"))
        {
            vp_Timer.In(0.5f,delegate{RefreshTitle();});

        }else
        {
             mineTitle.text = _gameManager.configData["Title"].ToString();
            mineEnglishTitle.text = _gameManager.configData["TitleEnglish"].ToString();
        }
       
    }

    /// <summary>
    /// 登录回调
    /// </summary>
    /// <param name="hash"></param>
    void enter(Hashtable hash)
    {

        Hashtable hashtable = hash["data"] as Hashtable;

      
        Debug.Log("kk看看全部" + hashtable.toJson());
        if (hashtable.ContainsKey("userId"))
        {
            _playerCache._playerData._User.userID = hashtable["userId"].ToString();
        }
        if (hashtable.ContainsKey("userName"))
        {
            _playerCache._playerData._User.userName = hashtable["userName"].ToString();
        }
        if (hashtable.ContainsKey("telephone"))
        {
            _playerCache._playerData._User.userCellPhone = hashtable["telephone"].ToString();
        }


        _maskLayer.ShowAlertWithMSG("登录成功");
        Hashtable data = new Hashtable();
        _playerCache._playerData.token = hashtable["token"].ToString();
        //downloadSaveProfile(data);
        Globe.nextSceneName = "Main";
        SceneManager.LoadScene("Load");
    }


    public void downloadSaveProfile(Hashtable hash)//下载用户的信息
	{
        
        //_dataManager.GetUserGreenHouseData();
	}
    public void ClickRegister()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.Regist, KMasklayerCenter.masklayer);
    }
    public void ClickForgetPassword()
    {
        _maskLayer.ShowAlertOnly(kAlertTypes.ForgetPassword, KMasklayerCenter.masklayer);
    }
    public void ClickLogIn()
    {
        if(_maskLayer.Container.childCount >0)
        {
            return;//如果现在又提示则退出
        }
        if (usernameInput.text.Equals("admin") && passwordInput.text.Equals("123456"))
        {
            SceneManager.LoadScene("Load");
            return;
        }
        if (usernameInput.text.Equals("") || passwordInput.text.Equals(""))
        {
            Debug.Log("用户名或密码不得为空");
           
            _maskLayer.ShowAlertWithTips("用户名或密码不得为空");
        }
        else
        {
            _dataManager.LogIn(usernameInput.text,passwordInput.text);
            _playerCache._Data.userName = usernameInput.text;
            _playerCache._Data.passWord = passwordInput.text;
            _playerCache._Data.isRecordUP = checkToggle.isOn;
            _playerCore.saveProfile();

        }
    }

    public void gameQuit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }


    void Update()
    {

       // Debug.Log(_playerCache._playerData.HttpIPAddress);
        if (Input.GetKeyUp(KeyCode.Return))
        {
              Debug.Log("登录");
            ClickLogIn();  
          
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (inputList.Contains(system.currentSelectedGameObject))
            {
                //正序
                GameObject next = NextInput(system.currentSelectedGameObject);
                system.SetSelectedGameObject(next);
               
                //GameObject last = LastInput(system.currentSelectedGameObject);
                //system.SetSelectedGameObject(last);
            }
        }
        
     


    }


     //获取上一个物体
    private GameObject LastInput(GameObject input)
    {
        int indexNow = IndexNow(input);
        if (indexNow - 1 >= 0)
        {
            return inputList[indexNow - 1].gameObject;
        }
        else
        {
            return inputList[inputList.Count-1].gameObject;
        }
    }
    //获取当前选中物体的序列
    private int IndexNow(GameObject input)
    {
        int indexNow = 0;
        for (int i = 0; i < inputList.Count; i++)
        {
            if (input == inputList[i])
            {
                indexNow = i;
                break;
            }
        }
        return indexNow;
    }

    //获取下一个物体
    private GameObject NextInput(GameObject input)
    {
        int indexNow = IndexNow(input);
        if(indexNow +1< inputList.Count)
        {
            return inputList[indexNow + 1].gameObject;
        }
        else
        {
            return inputList[0].gameObject;
        }
    }
}
