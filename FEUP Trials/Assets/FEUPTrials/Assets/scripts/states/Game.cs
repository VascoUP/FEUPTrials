using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[SerializeField]
internal class Game : IGameState
{
    private enum GameState { GAME, PAUSE, GAME_OVER };
    private GameState state = GameState.GAME;
    
    private bool isMultiplayer;
    private PlayerStats p1PlayerStats;
    private PlayerStats p2PlayerStats;

    private List<GameObject> cameras = new List<GameObject>();

    public Game(bool isMultiplayer = false)
    { 
        // Create an alias for the prefab manager instance
        this.isMultiplayer = isMultiplayer;
    }


    public void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnEnter()
    {
        SpawnPlayerOne();
        SpawnPlayerTwo();
    }

    public void Update()
    {
        if(state == GameState.GAME_OVER)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || InputManager.IsNext())
            {
                GameManager.instance.ChangeState(new Outro());
            }
        }
        else if(state == GameState.GAME)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu(true);
            }
            // TODO: Apagar isto
            if (Input.GetKeyDown(KeyCode.F12))
            {
                state = GameState.GAME_OVER;
            }
        }
        else if(state == GameState.PAUSE)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                ExitGame();
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu(false);
            }
        }
    }

    public void OnExit()
    {

    }


    private void ExitGame()
    {
        GameManager.instance.ChangeState(new MainMenu());
    }

    private void PauseMenu(bool isPause)
    {
        int timeScale;
        GameState nextState;

        if(isPause)
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


    private int CalculatePoints(float maxValue, float value)
    {
        int points = 10;
        int frac = 10;

        while(points >= 2)
        {
            if (maxValue * frac / 10f >= value)
                return points;

            points--;
            frac++;
        }

        return points;
    }

    private void OnMPPlayerFinish()
    {
        if(p1PlayerStats != null && p2PlayerStats != null)
        {
            int p1Points = 0;
            int p2Points = 0;

            // time point go from 0 to 10
            if (p1PlayerStats.time <= p2PlayerStats.time)
            {
                p1Points += 10;
                p2Points += CalculatePoints(p1PlayerStats.time, p2PlayerStats.time);

            }
            else
            {
                p2Points += 10;
                p1Points += CalculatePoints(p2PlayerStats.time, p1PlayerStats.time);
            }

            // fault points go from 0 to 10
            if (p1PlayerStats.faults <= p2PlayerStats.faults)
            {
                p1Points += 10;
                p2Points += CalculatePoints(p1PlayerStats.faults, p2PlayerStats.faults);
            }
            else
            {
                p2Points += 10;
                p1Points += CalculatePoints(p2PlayerStats.faults, p1PlayerStats.faults);
            }

            state = GameState.GAME_OVER;

            GameObject ui = GameObject.Find("UI Manager");
            UIManager uiManager = ui.GetComponent<UIManager>();
            uiManager.MPGameOver(p1Points > p2Points, p1PlayerStats, p1Points, 
                                p1Points < p2Points, p2PlayerStats, p2Points);
        }
    }

    private void OnPlayerOneFinish(PlayerStats stats)
    {
        p1PlayerStats = stats;

        if (isMultiplayer)
        {
            OnMPPlayerFinish();
        }
        else
        {
            state = GameState.GAME_OVER;

            GameObject ui = GameObject.Find("UI Manager");
            UIManager uiManager = ui.GetComponent<UIManager>();
            bool won = p1PlayerStats.time <= GameManager.instance.spTimeLimit && p1PlayerStats.faults <= GameManager.instance.spFaultLimit;
            uiManager.SPGameOver(won, p1PlayerStats);
        }
    }

    private void OnPlayerTwoFinish(PlayerStats stats)
    {
        p2PlayerStats = stats;

        OnMPPlayerFinish();
    }


    private void SpawnPlayerOne()
    {
        GameObject player = GameObject.Find("Player 1");
        GameObject playerManagerObject = Object.Instantiate(GameManager.instance.prefabManager.playerManager, player.transform);
        playerManagerObject.name = "Player Manager";

        PlayerManager playerManager = playerManagerObject.GetComponent<PlayerManager>();
        playerManager.GameOver += new PlayerFinish(OnPlayerOneFinish);

        AssociatePlayerManagerCamera(playerManagerObject, GameManager.instance.prefabManager.camera);

        GameObject cameraObject = Object.Instantiate(GameManager.instance.prefabManager.camera, player.transform);
        cameraObject.AddComponent<AudioListener>();
        cameraObject.layer = 8;
        cameras.Add(cameraObject);

        Camera camera = cameraObject.GetComponent<Camera>();
        camera.name = "Camera Player 1";

        if (isMultiplayer)
        {
            camera.orthographicSize = 15;
            camera.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 1f));
            
            CameraController controller = cameraObject.GetComponent<CameraController>();
            controller.offset.y = 10;
        }
    }
    
    private void SpawnPlayerTwo()
    {
        if (!isMultiplayer)
            return;

        GameObject player = GameObject.Find("Player 2");

        GameManager.instance.prefabManager.playerManager.layer = 9;
        GameObject playerManagerObject = Object.Instantiate(GameManager.instance.prefabManager.playerManager, player.transform);
        playerManagerObject.name = "Player Manager";
        GameManager.instance.prefabManager.playerManager.layer = 8;

        PlayerManager playerManager = playerManagerObject.GetComponent<PlayerManager>();
        playerManager.GameOver += new PlayerFinish(OnPlayerTwoFinish);

        AssociatePlayerManagerCamera(playerManagerObject, GameManager.instance.prefabManager.camera);

        GameObject cameraObject = Object.Instantiate(GameManager.instance.prefabManager.camera, player.transform);
        cameraObject.layer = 9;
        cameras.Add(cameraObject);

        Camera camera = cameraObject.GetComponent<Camera>();
        camera.name = "Camera Player 2";
        camera.orthographicSize = 15;
        camera.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
        camera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("TransparentFX"))
            | (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Water"))
            | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Player 2"))
            | (1 << LayerMask.NameToLayer("Background Image"));

        CameraController controller = cameraObject.GetComponent<CameraController>();
        controller.offset.y = 10;
    }

    private void AssociatePlayerManagerCamera(GameObject playerManager, GameObject camera)
    {
        CameraController controller = camera.GetComponent<CameraController>();
        if (controller != null)
        {
            controller.playerManager = playerManager.GetComponent<PlayerManager>();
        }
    }
    
}

public class PlayerStats
{
    public float time;
    public int faults;

    public PlayerStats(float time, int faults)
    {
        this.time = time;
        this.faults = faults;
    }
}