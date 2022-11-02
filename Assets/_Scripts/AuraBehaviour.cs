using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AuraBehaviour : MonoBehaviour
{
    internal Image aura;
    internal Vector2 speed;
    internal Vector2 weight;
    internal Vector2 friction;
    internal Vector2 power;

    internal Color color;
    float alpha;
    float dispersion;
    
    bool toDisable;
    float lifeTimer;
    float timeToLive;

    float lclScale;

    void Awake()
    {
        aura = GetComponent<Image>();

        var pwr = (Random.Range(50, 301) / 100.0f);
        var angle = Random.Range(0, 361);
        var vx = (float)Mathf.Cos(angle * Mathf.Deg2Rad);
        var vy = (float)Mathf.Sin(angle * Mathf.Deg2Rad);
        var frct = (vx > 0) ? 0.005f : -0.005f;

        speed = new Vector2(vx, vy);
        weight = new Vector2(0, -0.01f);
        friction = new Vector2(frct, 0);
        power = new Vector2(pwr, pwr);

        lclScale = Random.Range(100, 201) / 100.0f;
        aura.transform.localScale = new Vector2(lclScale, lclScale);

        alpha = 1.0f;
        dispersion = 1.0f / Random.Range(90, 181);

        toDisable = false;
        lifeTimer = 0;
        timeToLive = Random.Range(75, 151) / 100;

        aura.color = color;
    }

    

    void Update()
    {
        lifeTimer += Time.deltaTime;

        alpha -= dispersion/2.0f;
        color.a = alpha;
        aura.color = color;


        if (lifeTimer >= timeToLive || alpha <= 0) { toDisable = true; }

        speed -= (weight + friction)/2;

        Vector2 movement = speed * power;
        aura.transform.position -= (Vector3)movement;

        if (toDisable) { gameObject.SetActive(false); }
    }
}
