using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using BestHTTP;
using System.Text;

[RequireComponent(typeof(PlayerCache))]

public class PlayerCore:Define
{
	public bool isNew = false;
	public string Dirpath;
	public PlayerCache playerCache;

	void Awake ()
	{
		playerCache = GetComponent<PlayerCache> ();


		initProfile();

		
		//定义存档路径
		Dirpath = Application.persistentDataPath + "/Save";
		//如果没有创建存档文件夹则是第一次进入游戏
		if(!IOHelper.IsDirectoryExists(Dirpath))
		{
			isNew = true;
			Debug.Log("创建存档Player Profile not found,init.");
			//创建存档文件夹
			IOHelper.CreateDirectory (Dirpath);
			initProfile();
			saveProfile();
		}
		else
		{
			if(GameManager.instance. deletePlayerPrefs)
			{
				IOHelper.deletAll(Dirpath);
				IOHelper.CreateDirectory (Dirpath);
				initProfile();

			}else
			{
				//Debug.Log("提取存档Player Profile found,load.");
				loadProfile();
			}


		}
		
	}
	void Start()
	{
        
       // RegistNotification(this, kNotificationKeys.getGreenHouselist, OnGetGreenHousedataFinished);//把大棚信息保存到本地
    }
	public void initProfile ()
	{
		playerCache.newGame();
	}
		
	public void loadProfile ()
	{
		
		if(IOHelper.IsFileExists(Dirpath + "/PlayerData.sav"))
		{
			playerCache._Data = (SaveData)IOHelper.GetData(Dirpath + "/PlayerData.sav",typeof(SaveData));
			//Debug.Log("提取存档成功" + playerCache._playerData.dataToJson());
		}else
		{
			playerCache.newGame();
		}
		playerCache.checkVersion ();
		

	}



	public void saveProfile ()
	{
		
		string fileplayer = Dirpath + "/PlayerData.sav";
		IOHelper.SetData(fileplayer,playerCache._Data);
		Debug.Log("存档成功");
		
	}


	#region 上传存档
	///// <summary>
	///// 上传存档
	///// </summary>
	///// 
	//public void upLodingSaveProfile()//上传存档
	//{
 //       Hashtable data = _playerCache._Data.dataToJson();
 //       Debug.Log("上传" + data.toJson());
 //       _dataManager.PostUserData(data);
 //       /*       //半小时后继续上传
 //              Invoke("upLodingSaveProfile", 1800f);
 //              Debug.Log("存档上传");*/
 //   }
    public void OnGetGreenHousedataFinished(Hashtable hash)
    {
        Debug.Log(hash.toJson());
       // Debug.Log(hash["msg"] as string);
        
        //_playerCache._Data.dataFromHastable(hash);
        PostNotification(kNotificationKeys.refreshHouseData, hash);

    }
    #endregion
}



