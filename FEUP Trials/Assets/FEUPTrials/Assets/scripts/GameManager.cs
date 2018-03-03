using UnityEngine;

public class GameManager : MonoBehaviour {
    public bool isMultiplayer;
    [SerializeField]
    private GameObject _cameraPrefab;
    [SerializeField]
    private GameObject _playerManagerPrefab;

    // Use this for initialization
    private void Start ()
    {
        SpawnCameras();
        SpawnPlayerManagers();
    }

    private void SpawnCameras()
    {
        GameObject player = GameObject.Find("Player 1");
        GameObject cameraObject = Instantiate(_cameraPrefab, player.transform);
        cameraObject.AddComponent<AudioListener>();
        cameraObject.layer = 8;
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.name = "Camera Player 1";

        if (isMultiplayer)
        {
            camera.orthographicSize = 15;
            camera.rect = new Rect(new Vector2(0f, 0f), new Vector2(0.5f, 1f));

            player = GameObject.Find("Player 2");
            cameraObject = Instantiate(_cameraPrefab, player.transform);
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
        GameObject playerManagerObject = Instantiate(_playerManagerPrefab, player.transform);
        playerManagerObject.name = "Player Manager";

        if (isMultiplayer)
        {
            player = GameObject.Find("Player 2");
            _playerManagerPrefab.layer = 9;
            playerManagerObject = Instantiate(_playerManagerPrefab, player.transform); 
            playerManagerObject.name = "Player Manager";

            _playerManagerPrefab.layer = 8;
        }
    }
}
