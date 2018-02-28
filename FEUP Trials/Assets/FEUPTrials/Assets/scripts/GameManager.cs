using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isToRestart = false;

    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _bikePrefab;

    private CameraController _cameraController;

    public GameObject activeBike;

    [SerializeField]
    private Checkpoint _checkpoint;

    private void Start()
    {
        _cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    private void Update () {
        if (isToRestart || Input.GetKeyUp(KeyCode.R))
            Restart();
	}

    private void Restart()
    {
        // Destroy both player and bike game objects
        GameObject tmp = GameObject.Find("Player Body");
        if(tmp != null)
            Destroy(tmp);
        tmp = GameObject.Find("Bike");
        if (tmp != null)
            Destroy(tmp);

        // Instantiate bike
        activeBike = Instantiate(_bikePrefab);
        activeBike.name = "Bike";
        activeBike.transform.position = _checkpoint.BikeNewPosition();
        _cameraController.SetPlayerObject(activeBike);

        // Instantiate player
        tmp = Instantiate(_playerPrefab);
        tmp.name = "Player Body";
        tmp.transform.position = _checkpoint.BikeNewPosition();
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        _checkpoint = checkpoint;
        _checkpoint.ActivateCheckpoint();
    }
}
