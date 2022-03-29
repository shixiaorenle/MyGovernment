using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class BackToListButton : Define, IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * 1.2f,0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        transform.DOScale(Vector3.one , 0.1f);
    }
}
