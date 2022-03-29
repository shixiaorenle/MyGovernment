using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIMSG : UIAlertLayer
{
    public Text text;

    public CanvasGroup cg;
    protected override void Start()
    {
        setMinSize();
        cg.alpha = 0;
        cg.gameObject.SetActive(false);
    }
    public void Refresh()
    {
        
        if (Param != null)
        {
            text.text = Param["tips"].ToString();
        }
        cg.alpha = 0;
        cg.gameObject.SetActive(true);
        cg.DOFade(1, 1).SetEase(Ease.OutCubic).OnComplete(delegate {
            cg.DOFade(0, 2).SetEase(Ease.InCubic).OnComplete(delegate {
                cg.gameObject.SetActive(false);
            });
        });
    }
}
