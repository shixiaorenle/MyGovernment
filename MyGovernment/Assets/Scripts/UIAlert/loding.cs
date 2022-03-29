using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loding : UIAlertLayer
{
	public GameObject connect;
    float time = 0;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        RegistNotification (this, kNotificationKeys.closeLoading, CloesLoding);
		StartCoroutine (DelayToInvoke.DelayToInvokeDo (() => {
			gameObject.GetComponent<Animator>().Play("loading");
			connect.SetActive(true);
//			gameObject.GetComponent<Animator>().Play("loding");
		}, 1f));

	}
    public override void RefreshData(Hashtable hashtable)
    {
        //Debug.Log(index+"加");
    }

    // Update is called once per frame
    protected override void Update ()
	{
        time += Time.deltaTime;
        if (time>=30)
        {
            _maskLayer.ShowAlertWithTips("连接超时，请重新操作");
            
            //Debug.Log(_playerCache._playerData.nowUrl);
            //Debug.Log(_playerCache._playerData.nowHash);
            //Debug.Log(_playerCache._playerData.nowFinishDel);
            //Debug.Log(_playerCache._playerData.nowBool);
            onClose();
            //RequestHttp(_playerCache._playerData.nowUrl, _playerCache._playerData.nowHash, _playerCache._playerData.nowFinishDel, _playerCache._playerData.nowBool);
            
        }
	}
	public void CloesLoding(Hashtable hash = null)
	{


        onClose();
    }
}
