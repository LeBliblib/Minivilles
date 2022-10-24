using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    //Vector3 startPos;
    Vector3 normalscale;

    public AudioSource flip;
    public AudioClip clip;

    private bool isPlaying = false;

    void Start()
    {
        //startPos = transform.position;
        normalscale = transform.localScale;
    }
    public void OnMouseOver()
    {
        if (isPlaying == false) //pour éviter les échos
        {
            flip.PlayOneShot(clip, 0.5f); 
            isPlaying = true;
        }
        

        //transform.position = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(0.7f, 0.7f, 0);
        GetComponent<SpriteRenderer>().sortingOrder = 5;
    }

    public void OnMouseExit()
    {
        isPlaying = false;
        //transform.position = startPos;
        transform.localScale = normalscale;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
}
