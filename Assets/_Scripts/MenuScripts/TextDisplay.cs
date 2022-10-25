using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] string stringToShow;

    [Header("Random time (sec) for writing")]
    [SerializeField] float min;
    [SerializeField] float max;

    float timer = 0.2f;
    byte counter = 0;
    internal bool isTextCoroutineSwitcherOn;
    bool isAllTextLoaded;

    void Start()
    {
        textMesh.text = "";
        isTextCoroutineSwitcherOn = false;
        isAllTextLoaded = false;
    }

    
    void Update()
    {

        if (isTextCoroutineSwitcherOn)
        {
            StartCoroutine(LoadText());
            isTextCoroutineSwitcherOn = false;
        }

    }

    IEnumerator LoadText()
    {
        if (stringToShow.Length < 1)
        {
            isAllTextLoaded = true;
        }
        else
        {
            while (!isAllTextLoaded)
            {
                yield return new WaitForSeconds(Random.Range(min,max));

                if (counter < stringToShow.Length)
                {
                    textMesh.text += stringToShow[counter];
                    counter++;
                }
                else
                {
                    timer = 0;
                    isAllTextLoaded = true;
                    counter = 0;
                }
            }
        }
    }
}
