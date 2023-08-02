using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitAnimation : MonoBehaviour
{
    public GameObject hitTextObject;
    private TMPro.TextMeshPro hitText;

    private Color startColor;
    private Color currentColor;
    public static float showAlpha = 0.8f;
    public Color missColor = new Color(1, 0.2f, 0.2f);
    public Color hitColor = new Color(0.4f, 0.7f, 0.9f);
    public Color perfectHitColor = new Color(0, 1, 0); 

    void Start()
        {
            hitText = hitTextObject.GetComponent<TMPro.TextMeshPro>();
            currentColor = hitText.color;
            currentColor.a = showAlpha;
            startColor = hitText.color;
            startColor.a = 0;
        }

    public void StartAnim(string hitString)
    {
        hitText.color = currentColor;
        hitText.text = hitString;
        StartCoroutine(PlayAnim());
    }

    IEnumerator PlayAnim()
    {
        Color currentColor = hitText.color;

        float currentTime = 0;
        float animSpeedInSec = 0.5f;

        while (currentTime < animSpeedInSec)
        {
            currentTime += Time.deltaTime;
            hitText.color = Color.Lerp(currentColor, startColor, currentTime / animSpeedInSec);
            yield return null;
        }
        EndAnim();
        yield break;
    }

    public void EndAnim()
    {
        //Destroy(gameObject);
    }
}
