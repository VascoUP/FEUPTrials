using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private Text _timerText;
    /*[SerializeField]
    private */

	void LateUpdate () {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
        if (_timer.isTimeToText)
        {
            _timerText.text = TimeToString();
            _timer.isTimeToText = false;
        }
    }

    private string TimeToString()
    {
        float timeCounter = _timer.timeCounter;
        int minutes = Mathf.FloorToInt(timeCounter / 60f);
        int seconds = Mathf.FloorToInt(timeCounter - minutes * 60f);
        return (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
    }
}
