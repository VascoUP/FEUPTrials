using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[SerializeField]
internal class Game : IGameState
{
    private PrefabManager prefabs;

    private bool isMultiplayer;

    private List<GameObject> cameras = new List<GameObject>();

    public Game(bool isMultiplayer = false)
    { 
        // Create an alias for the prefab manager instance
        prefabs = GameManager.instance.prefabManager;
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
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Going back to the main menu.");
            GameManager.instance.ChangeState(new MainMenu());
        }
        if (Input.GetKey(KeyCode.P))
        {
            GameManager.instance.PushState(new Pause());
        }
    }

    public void OnExit()
    {
        // Cleanup cameras
        foreach (GameObject cam in cameras) {
            Debug.Log("Destroyed camera muahahah");
            Object.Destroy(cam);
        }
    }

    private void SpawnPlayerOne()
    {
        GameObject player = GameObject.Find("Player 1");
        GameObject playerManagerObject = Object.Instantiate(prefabs.playerManager, player.transform);
        playerManagerObject.name = "Player Manager";

        AssociatePlayerManagerCamera(playerManagerObject, prefabs.camera);

        GameObject cameraObject = Object.Instantiate(prefabs.camera, player.transform);
        cameraObject.AddComponent<AudioListener>();
        cameraObject.layer = 8;
        cameras.Add(cameraObject);

        Camera camera = cameraObject.GetComponent<Camera>();
        camera.name = "Camera Player 1";

        if (isMultiplayer)
        {
            camera.orthographicSize = 15;
            camera.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 1f));
        }

    }
    
    private void SpawnPlayerTwo()
    {
        if (!isMultiplayer)
            return;

        GameObject player = GameObject.Find("Player 2");

        prefabs.playerManager.layer = 9;
        GameObject playerManagerObject = Object.Instantiate(prefabs.playerManager, player.transform);
        playerManagerObject.name = "Player Manager";
        prefabs.playerManager.layer = 8;

        AssociatePlayerManagerCamera(playerManagerObject, prefabs.camera);

        GameObject cameraObject = Object.Instantiate(prefabs.camera, player.transform);
        cameraObject.layer = 9;
        cameras.Add(cameraObject);

        Camera camera = cameraObject.GetComponent<Camera>();
        camera.name = "Camera Player 2";
        camera.orthographicSize = 15;
        camera.rect = new Rect(new Vector2(0.5f, 0f), new Vector2(0.5f, 1f));
        camera.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("TransparentFX"))
            | (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Water"))
            | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Player 2"));
    }

    private void AssociatePlayerManagerCamera(GameObject playerManager, GameObject camera)
    {
        CameraController controller = prefabs.camera.GetComponent<CameraController>();
        if (controller != null)
        {
            controller.playerManager = playerManager.GetComponent<PlayerManager>();
        }
    }
}