using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor.UIElements;

public class MenuManager : MonoBehaviour
{
    public static int difficulty = 0;

    [SerializeField] Image title;
    [SerializeField] Image sunburst;
    [SerializeField] Image[] curtains;

    [SerializeField] UnityEvent curtainsAction;
    [SerializeField] UnityEvent titlePop;
    [SerializeField] UnityEvent sunburstPop;

    byte step;

    float timer;

    bool areCurtainsOpened;


    //==================================================================================================

    
    void Start()
    {
        timer = 0;
        step = 0;

        title.transform.localScale = new Vector2(0, 0);
        sunburst.transform.localScale = new Vector2(0, 0);

        curtains[0].transform.position = new Vector2(Screen.width / 2, Screen.height);
        curtains[0].rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 2);
        curtains[1].transform.position = new Vector2(Screen.width / 2, 0);
        curtains[1].rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 2);

        areCurtainsOpened = false;

    }


    //==================================================================================================


    void Update()
    {

        timer += Time.deltaTime;
        if ( timer >= 0.5f && step == 0)
        {
            curtainsAction.Invoke();
            step++;
            timer = 0;
        }

        if(timer>= 0.3f && step == 1)
        {
            titlePop.Invoke();
            step++;
            timer = 0;
        }

        if (timer >= 0.3f && step == 2)
        {
            sunburstPop.Invoke();
            step++;
            timer = 0;
        }
    }


    //==================================================================================================

    
    public void CurtainsAction()
    {
        if (!areCurtainsOpened)
        {
            LeanTween.scaleY(curtains[0].gameObject, 0, 0.3f).setEase(LeanTweenType.easeOutSine);
            LeanTween.scaleY(curtains[1].gameObject, 0, 0.3f).setEase(LeanTweenType.easeOutSine);
            areCurtainsOpened = true;
        }
        else
        {
            LeanTween.scaleY(curtains[0].gameObject, 1, 0.3f).setEase(LeanTweenType.easeOutSine);
            LeanTween.scaleY(curtains[1].gameObject, 1, 0.3f).setEase(LeanTweenType.easeOutSine);
        }
    }

    public void PopTitle()
    {
        LeanTween.scale(title.gameObject, new Vector2(1.3f, 1.3f), 2.0f).setEase(LeanTweenType.easeOutElastic);
    }

    public void PopSunburst()
    {
        LeanTween.scale(sunburst.gameObject, new Vector2(1.0f, 1.0f), 2.0f).setEase(LeanTweenType.easeOutElastic);
    }
}
