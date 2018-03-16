using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private enum GameState { GAME, PAUSE, GAME_OVER };
    private GameState state = GameState.GAME;

    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private Text _timerText;

    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private Button _resumeButton;
    [SerializeField]
    private Button _mainMenuButton;

    [SerializeField]
    private GameObject _spGOPanel;
    [SerializeField]
    private Text _spState;
    [SerializeField]
    private Text _spTime;
    [SerializeField]
    private Text _spFaults;
    [SerializeField]
    private Button _spMainMenuButton;

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
    [SerializeField]
    private Button _mpMainMenuButton;

    // Dirty hack
    private bool won;

    private void Start()
    {
        _resumeButton.onClick.AddListener(() =>
        {
            PauseMenu(false);
        });
        _mainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            QuitGame();
        });
        _spMainMenuButton.onClick.AddListener(() =>
        {
            ExitGame();
        });
        _mpMainMenuButton.onClick.AddListener(() =>
        {
            ExitGame();
        });
    }

    private void Update()
    {
        if (state == GameState.GAME_OVER)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
            }
        }
        else if (state == GameState.GAME)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu(true);
            }
        }
        else if (state == GameState.PAUSE)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                QuitGame();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu(false);
            }
        }
    }

    private void LateUpdate () {
        if (_timer.isTimeToText)
        {
            _timerText.text = Utils.TimeToString(_timer.timeCounter);
            _timer.isTimeToText = false;
        }
    }

    private void QuitGame()
    {
        GameManager.instance.ChangeState(new MainMenu());
    }
    
    private void ExitGame()
    {
        if (won) GameManager.instance.ChangeState(new Outro());
        else GameManager.instance.ChangeState(new MainMenu());
    }

    private void PauseMenu(bool isPause)
    {
        int timeScale;
        GameState nextState;

        if (isPause)
        {
            timeScale = 0;
            nextState = GameState.PAUSE;
        }
        else
        {
            timeScale = 1;
            nextState = GameState.GAME;
        }

        PauseMenu(timeScale, isPause, nextState);
    }

    private void PauseMenu(int timeScale, bool isPause, GameState state)
    {
        Time.timeScale = timeScale;

        GameObject ui = GameObject.Find("UI Manager");
        UIManager uiManager = ui.GetComponent<UIManager>();
        uiManager.Pause(isPause);

        this.state = state;

    }


    private void SetTimerPanel(bool active)
    {
        _timerText.gameObject.SetActive(active);
    }

    public void Pause(bool active)
    {
        _pausePanel.SetActive(active);
        SetTimerPanel(!active);
    }

    public void SPGameOver(bool won, PlayerStats stats)
    {
        // Hackity hack
        this.won = won;

        state = GameState.GAME_OVER;

        SetTimerPanel(false);

        _spGOPanel.SetActive(true);

        _spState.text = won ? "Winner" : "Loser";
        _spTime.text = Utils.TimeToString(stats.time);
        _spFaults.text = stats.faults.ToString();
    }

    public void MPGameOver(bool p1Won, PlayerStats p1Stats, float p1Total,
                            bool p2Won, PlayerStats p2Stats, float p2Total)
    {
        state = GameState.GAME_OVER;

        SetTimerPanel(false);

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
