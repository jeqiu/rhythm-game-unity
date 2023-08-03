using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DelaySettings : MonoBehaviour
{
    [SerializeField] Slider delaySlider;
    [SerializeField] TextMeshProUGUI delayText;
    //public TMPro.TextMeshProUGUI delayText;

    private int inputDelay;

    // Start is called before the first frame update
    void Start()
    {
        inputDelay = SharedData.inputDelay;

        SetInputDelay(inputDelay);
    }

    public void SetInputDelay(int _delayinMilliseconds)
    {
        if (_delayinMilliseconds < -100)
        {
            _delayinMilliseconds = -100;
        }
        if (_delayinMilliseconds > 100)
        {
            _delayinMilliseconds = 100;
        }

        RefreshDelaySlider(_delayinMilliseconds);
        SharedData.inputDelay = _delayinMilliseconds;

    }

    public void SetDelayFromSlider()
    {
        SetInputDelay((int)delaySlider.value);
    }

    private void RefreshDelaySlider(int _delayinMilliseconds)
    {
        delaySlider.value = _delayinMilliseconds;
        delayText.text = (SharedData.inputDelay).ToString();
        InternalGameLog.LogMessage("Delay is now: " + _delayinMilliseconds);

    }
}
