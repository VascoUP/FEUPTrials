using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
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
        GameObject tmp = Utils.FilterTaggedObjectByParentAndName("Body", "Player Body", transform.parent.name);
        if (tmp != null)
            Destroy(tmp);
        tmp = Utils.FilterTaggedObjectByParent("Bike", transform.parent.name);
        if (tmp != null)
            Destroy(tmp);

        // Instantiate bike
        activeBike = Instantiate(_bikePrefab);
        activeBike.name = "Bike";
        activeBike.transform.parent = transform.parent;
        activeBike.transform.position = _checkpoint.BikeNewPosition();
        _cameraController.SetPlayerObject(activeBike);
        Utils.SetLayer(activeBike.transform, gameObject.layer);

        // Instantiate player
        tmp = Instantiate(_playerPrefab);
        tmp.name = "Player Body";
        tmp.transform.parent = transform.parent;
        tmp.transform.position = _checkpoint.BikeNewPosition();
        Utils.SetLayer(tmp.transform, gameObject.layer);
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        _checkpoint = checkpoint;
        _checkpoint.ActivateCheckpoint();
    }
}
