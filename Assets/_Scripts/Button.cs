using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public RectTransform rectTransform;
    public bool isPileOpen = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void PileGlide()
    {
        GameObject parent = transform.parent.gameObject;
        isPileOpen = !isPileOpen;
        if (isPileOpen)
            LeanTween.move(parent, parent.transform.position + parent.transform.right * (parent.GetComponent<RectTransform>().sizeDelta.x - rectTransform.sizeDelta.x), 1) ;
        else
            LeanTween.move(parent, parent.transform.position + -parent.transform.right * (parent.GetComponent<RectTransform>().sizeDelta.x - rectTransform.sizeDelta.x), 1);
    }
}
