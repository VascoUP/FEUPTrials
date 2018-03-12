using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private GameObject _playerObject;

    public PlayerManager playerManager;

    public Vector3 offset;

    private void Start()
    {
        Debug.Log("Player manager is " + (playerManager == null ? "null" : "not null"));
        playerManager.NewPlayer += new SetPlayer(SetPlayerObject);
        SetPlayerObject(playerManager.activeBike);
    }

    private void LateUpdate ()
    {
        if (_playerObject != null)
        {
            transform.position = _playerObject.transform.position + offset;
        }
    }

    public void SetPlayerObject(GameObject player)
    {
        _playerObject = player;
    }
}
