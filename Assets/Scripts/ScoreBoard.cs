using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard Instance;
    private double gameScore;
    public TMPro.TextMeshPro scoreText;
    public TMPro.TextMeshPro returnText;
    public float animSpeedInSec = 1f;
    bool keepAnimating = false;
    private string MenuName = "Menu";

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        gameScore = (double)SharedData.score / SharedData.maxScore;
        scoreText.text = string.Format("{0:P}", gameScore);
        SharedData.score = 0;
        startReturnTextAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("return"))
        {
            ReturnToMenu();
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(MenuName);
    }

    private IEnumerator AnimateReturnText()
    {
        Color currentColor = returnText.color;
        Color invisibleColor = returnText.color;
        invisibleColor.a = 0; //Set Alpha to 0

        float oldAnimSpeedInSec = animSpeedInSec;
        float counter = 0;
        while (keepAnimating)
        {
            //Hide Slowly
            while (counter < oldAnimSpeedInSec)
            {
                if (!keepAnimating)
                {
                    yield break;
                }

                //Reset counter when Speed is changed
                if (oldAnimSpeedInSec != animSpeedInSec)
                {
                    counter = 0;
                    oldAnimSpeedInSec = animSpeedInSec;
                }

                counter += Time.deltaTime;
                returnText.color = Color.Lerp(currentColor, invisibleColor, counter / oldAnimSpeedInSec);
                yield return null;
            }

            yield return null;


            //Show Slowly
            while (counter > 0)
            {
                if (!keepAnimating)
                {
                    yield break;
                }

                //Reset counter when Speed is changed
                if (oldAnimSpeedInSec != animSpeedInSec)
                {
                    counter = 0;
                    oldAnimSpeedInSec = animSpeedInSec;
                }

                counter -= Time.deltaTime;
                returnText.color = Color.Lerp(currentColor, invisibleColor, counter / oldAnimSpeedInSec);
                yield return null;
            }
        }
    }

    // call to start animation
    void startReturnTextAnimation()
    {
        if (keepAnimating)
        {
            return;
        }
        keepAnimating = true;
        StartCoroutine(AnimateReturnText());
    }

    void changeReturnTextAnimationSpeed(float animSpeed)
    {
        animSpeedInSec = animSpeed;
    }

    void stopReturnTextAnimation()
    {
        keepAnimating = false;
    }
}
