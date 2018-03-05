using UnityEngine;

[SerializeField]
internal class Game : IGameState
{
    private PrefabManager prefabs;

    private bool isMultiplayer;

    public Game(bool isMultiplayer = false)
    { 
        // Create an alias for the prefab manager instance
        prefabs = GameManager.instance.prefabManager;
        this.isMultiplayer = isMultiplayer;
    }

    public void OnEnter()
    {
        SpawnCameras();
        SpawnPlayerManagers();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            GameManager.instance.ChangeState(new MainMenu());
        }
        if (Input.GetKey(KeyCode.P))
        {
            GameManager.instance.PushState(new Pause());
        }
    }

    private void SpawnCameras()
    {
        GameObject player = GameObject.Find("Player 1");
        GameObject cameraObject = Object.Instantiate(prefabs.camera, player.transform);
        cameraObject.AddComponent<AudioListener>();
        cameraObject.layer = 8;
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.name = "Camera Player 1";

        if (isMultiplayer)
        {
            camera.orthographicSize = 15;
            camera.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 1f));

            player = GameObject.Find("Player 2");
            cameraObject = Object.Instantiate(prefabs.camera, player.transform);
            cameraObject.layer = 9;
            camera = cameraObject.GetComponent<Camera>();
            camera.name = "Camera Player 2";
            camera.orthographicSize = 15;
            camera.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
            camera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("TransparentFX"))
                | (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Water"))
                | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Player 2"));
        }
    }

    private void SpawnPlayerManagers()
    {
        GameObject player = GameObject.Find("Player 1");
        GameObject playerManagerObject = Object.Instantiate(prefabs.playerManager, player.transform);
        playerManagerObject.name = "Player Manager";

        if (isMultiplayer)
        {
            player = GameObject.Find("Player 2");
            prefabs.playerManager.layer = 9;
            playerManagerObject = Object.Instantiate(prefabs.playerManager, player.transform);
            playerManagerObject.name = "Player Manager";

            prefabs.playerManager.layer = 8;
        }
    }
}