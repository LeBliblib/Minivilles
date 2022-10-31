using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGameObject : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] ParticleSystem particles;

    [HideInInspector] public CardScriptableObject cardSo;

    public void Init(CardScriptableObject _cardSo, Transform parent)
    {
        cardSo = _cardSo;

        img.sprite = cardSo.texture;
        LeanTween.moveLocal(gameObject, parent.transform.localPosition, 0.3f).setEaseInOutExpo().setOnComplete(() =>
        {
            transform.parent = parent;
        });
    }

    public void ShowActivation()
    {
        //particles.Play();

        Vector3 baseSize = transform.localScale;

        LeanTween.scale(gameObject, baseSize + Vector3.one * 0.1f, 0.25f).setEaseInOutExpo().setOnComplete(() =>
        {
            LeanTween.scale(gameObject, baseSize, 0.15f).setEaseOutExpo();
        });
    }

    public void ChangeSprite(Sprite sprite)
    {
        img.sprite = sprite;
    }
}
