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

    [SerializeField]
    private GameObject _mpGOPanel;
    [SerializeField]
    private Text _mpP1Time;
    [SerializeField]
    private Text _mpP1Faults;
    [SerializeField]
    private Text _mpP1Total;
    [SerializeField]
    private Text _mpP1State;
    [SerializeField]
    private Text _mpP2Time;
    [SerializeField]
    private Text _mpP2Faults;
    [SerializeField]
    private Text _mpP2Total;
    [SerializeField]
    private Text _mpP2State;

    void LateUpdate () {
        if (_timer.isTimeToText)
        {
            _timerText.text = Utils.TimeToString(_timer.timeCounter);
            _timer.isTimeToText = false;
        }
    }

    private void GameOver()
    {
        _timerText.gameObject.SetActive(false);
    }

    public void SPGameOver(bool won, PlayerStats stats)
    {
        GameOver();

        _spGOPanel.SetActive(true);

        _spState.text = won ? "Winner" : "Loser";
        _spTime.text = Utils.TimeToString(stats.time);
        _spFaults.text = stats.faults.ToString();
    }

    public void MPGameOver(bool p1Won, PlayerStats p1Stats, float p1Total,
                            bool p2Won, PlayerStats p2Stats, float p2Total)
    {
        GameOver();

        _mpGOPanel.SetActive(true);
        
        _mpP1Time.text = Utils.TimeToString(p1Stats.time);
        _mpP1Faults.text = p1Stats.faults.ToString();
        _mpP1Total.text = p1Total.ToString();

        _mpP2Time.text = Utils.TimeToString(p2Stats.time);
        _mpP2Faults.text = p2Stats.faults.ToString();
        _mpP2Total.text = p2Total.ToString();

        if (p1Won && !p2Won)
        {
            _mpP1State.text = "Winner";
            _mpP2State.text = "Loser";
        }
        else if (!p1Won && p2Won)
        {
            _mpP1State.text = "Loser";
            _mpP2State.text = "Winner";
        }
        else
        {
            _mpP1State.text = "Draw";
            _mpP2State.text = "Draw";
        }
    }
}
