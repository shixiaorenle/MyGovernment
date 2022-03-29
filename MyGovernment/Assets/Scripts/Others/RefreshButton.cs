using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RefreshButton : MonoBehaviour
{
    public RectTransform icon;
    void Start()
    {
        
    }

    public void OnClick()
    {
       // Debug.Log(1111);
        icon.DOLocalRotate(new Vector3(0, 0, -180), 0.5f).SetEase(Ease.InQuad).OnComplete(() => {

            icon.DOLocalRotate(new Vector3(0, 0, -360), 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                icon.localRotation = Quaternion.identity;
            });

        });
    }
    void Update()
    {
        
    }
}
