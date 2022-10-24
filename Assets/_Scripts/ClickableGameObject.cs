using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableGameObject : MonoBehaviour
{
    Vector3 baseSize;
    Vector3 baseRot;

    [Header("Transform Changes")]
    [SerializeField] Vector3 sizeChange;
    [SerializeField] Vector3 rotChange;

    [Header("Values")]
    [SerializeField] float animSpeed;
    [SerializeField] float clickAnimSpeed;

    [Header("On Click")]
    [SerializeField] UnityEvent toDo;

    private void Start()
    {
        baseSize = transform.localScale;
        baseRot = transform.rotation.eulerAngles;
    }

    public void OnMouseEnter()
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, baseSize + sizeChange, animSpeed).setEaseInOutExpo();
        LeanTween.rotate(gameObject, baseRot + rotChange, animSpeed).setEaseInOutExpo();
    }

    public void OnMouseExit()
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, baseSize, animSpeed).setEaseInOutExpo();
        LeanTween.rotate(gameObject, baseRot, animSpeed).setEaseInOutExpo();
    }

    public void OnMouseDown()
    {
        LeanTween.cancel(gameObject);

        toDo.Invoke();

        LeanTween.scale(gameObject, baseSize, clickAnimSpeed).setEaseInExpo().setOnComplete(() =>
        {
            LeanTween.scale(gameObject, baseSize + sizeChange, clickAnimSpeed).setEaseOutExpo();
            LeanTween.rotate(gameObject, baseRot + rotChange, animSpeed).setEaseOutExpo();
        });
    }
}
