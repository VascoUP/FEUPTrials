using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public float timeCounter = 0;
    public bool isTimeToText = true;
    
    void Update () {
        float tmp = timeCounter + Time.deltaTime;
        if(Mathf.FloorToInt(tmp) != Mathf.FloorToInt(timeCounter))
        {
            isTimeToText = true;
        }
        timeCounter = tmp;
	}
}
