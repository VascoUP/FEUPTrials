using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[SerializeField]
internal class Game : IGameState
{
    private enum GameState { GAME, PAUSE, GAME_OVER, ONE_PLAYER_FINISHED };
    private GameState state = GameState.GAME;
    
    private bool isMultiplayer;

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
        if(state == GameState.GAME || state == GameState.GAME_OVER)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                GameManager.instance.ChangeState(new MainMenu());
            }
        }
    }

    public void OnExit()
    {
        // Cleanup cameras
        foreach (GameObject cam in cameras) {
            Object.Destroy(cam);
        }
    }

    private void OnPlayerOneFinish(float time, int faults)
    {
        if(isMultiplayer)
        {

        }
        else
        {
            state = GameState.GAME_OVER;
            GameObject ui = GameObject.Find("UI Manager");
            UIManager uiManager = ui.GetComponent<UIManager>();
            bool won = time <= GameManager.instance.spTimeLimit && faults <= GameManager.instance.spFaultLimit;
            uiManager.SPGameOver(won, time, faults);
        }
    }

    private void OnPlayerTwoFinish(float time, int faults)
    {

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
        CameraController controller = GameManager.instance.prefabManager.camera.GetComponent<CameraController>();
        if (controller != null)
        {
            controller.playerManager = playerManager.GetComponent<PlayerManager>();
        }
    }
    
}