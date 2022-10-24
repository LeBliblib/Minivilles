using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    //Vector3 startPos;
    Vector3 normalscale;

    void Start()
    {
        //startPos = transform.position;
        normalscale = transform.localScale;
    }
    public void OnMouseOver()
    {
        //transform.position = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(0.7f, 0.7f, 0);
        GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    public void OnMouseExit()
    {
        //transform.position = startPos;
        transform.localScale = normalscale;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
}
