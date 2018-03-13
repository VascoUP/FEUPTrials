using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private GameObject _spGOPanel;
    [SerializeField]
    private Text _spState;
    [SerializeField]
    private Text _spTime;
    [SerializeField]
    private Text _spFaults;
    /*[SerializeField]
    private */

    void LateUpdate () {
        if (_timer.isTimeToText)
        {
            _timerText.text = Utils.TimeToString(_timer.timeCounter);
            _timer.isTimeToText = false;
        }
    }

    public void SPGameOver(bool won, float time, int faults)
    {
        _timerText.gameObject.SetActive(false);
        
        _spGOPanel.SetActive(true);

        _spState.text = won ? "Winner" : "Loser";
        _spTime.text = Utils.TimeToString(time);
        _spFaults.text = faults.ToString();
    }
}
