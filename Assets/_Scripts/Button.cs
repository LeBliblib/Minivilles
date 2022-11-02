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
        RectTransform parent = transform.parent.GetComponent<RectTransform>();

        isPileOpen = !isPileOpen;
        if (isPileOpen)
            LeanTween.move(parent, new Vector3(-24, 0, 0), 0.2f); //parent.transform.position + parent.transform.right * (parent.sizeDelta.x - rectTransform.sizeDelta.x)
        else
            LeanTween.move(parent, new Vector3(-467, 0, 0), 0.2f); //parent.transform.position + -parent.transform.right * (parent.sizeDelta.x - rectTransform.sizeDelta.x)
    }
}
