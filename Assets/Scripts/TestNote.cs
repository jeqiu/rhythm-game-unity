using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNote : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime; // time note should be tapped by player
    void Start()
    {
        timeInstantiated = assignedTime - TestConductor.Instance.noteTime;
        //timeInstantiated = Conductor.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = TestConductor.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (TestConductor.Instance.noteTime * 2));


        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * TestConductor.Instance.noteSpawnY, Vector3.up * TestConductor.Instance.noteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}