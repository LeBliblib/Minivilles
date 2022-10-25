using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGameObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] ParticleSystem particles;

    public void Init(Sprite sprite, Vector3 goTo)
    {
        sr.sprite = sprite;
        LeanTween.move(gameObject, goTo, 0.3f).setEaseInOutExpo();
    }

    public void ShowActivation()
    {
        particles.Play();

        Vector3 baseSize = transform.localScale;

        LeanTween.scale(gameObject, baseSize + Vector3.one * 0.1f, 0.25f).setEaseInOutExpo().setOnComplete(() =>
        {
            LeanTween.scale(gameObject, baseSize, 0.15f).setEaseOutExpo();
        });
    }

    public void ChangeSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }
}
