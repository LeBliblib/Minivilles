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
    [SerializeField] GameObject panel1;
    [SerializeField] GameObject panel2;
    [SerializeField] Image[] curtains;

    [SerializeField] UnityEvent curtainsAction;

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
        panel1.transform.localScale = new Vector2(0, 0);
        panel2.transform.localScale = new Vector2(0, 0);

        curtains[0].transform.position = new Vector2(Screen.width / 2, Screen.height);
        curtains[0].rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 2);
        curtains[1].transform.position = new Vector2(Screen.width / 2, 0);
        curtains[1].rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 2);

        areCurtainsOpened = false;

    }


    //==================================================================================================


    void Update()
    {

        if (step != 4 && step != 6) { timer += Time.deltaTime; }

        if ( timer >= 0.5f && step == 0)
        {
            curtainsAction.Invoke();
            step++;
            timer = 0;
        }

        if(timer>= 0.3f && step == 1)
        {
            LeanTween.scale(title.gameObject, new Vector2(1.0f, 1.0f), 2.0f).setEase(LeanTweenType.easeOutElastic);
            step++;
            timer = 0;
        }

        if (timer >= 0.3f && step == 2)
        {
            LeanTween.scale(sunburst.gameObject, new Vector2(1.0f, 1.0f), 2.0f).setEase(LeanTweenType.easeOutElastic);
            step++;
            timer = 0;
        }

        if(timer>= 0.6f && step == 3)
        {
            LeanTween.scale(panel1, new Vector2(1.0f, 1.0f), 1.0f).setEase(LeanTweenType.easeOutElastic);
            step++;
            timer = 0;
        }

        if(timer >= 0.1f && step == 5)
        {
            LeanTween.scale(panel2, new Vector2(1.0f, 1.0f), 1.0f).setEase(LeanTweenType.easeOutElastic);
            step++;
            timer = 0;
        }

        if (timer >= 0.4 && step ==7)
        {
            timer = 0;
            SceneManager.LoadScene(1);
        }

        if(step == 10 && timer>= 0.4f)
        {
            Application.Quit();
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

    public void PlayButton()
    {
        LeanTween.scale(panel1, new Vector2(0,0),0.3f).setEase(LeanTweenType.easeOutSine);
        step++;
    }

    public void ExitButton()
    {
        CurtainsAction();
        step = 10;
    }

    public void SetEasyDifficultyButton()
    {
        CurtainsAction();
        difficulty = 0;
        step++;
    }

    public void SetHardDifficultyButton()
    {
        CurtainsAction();
        difficulty = 1;
        step++;
    }
}
