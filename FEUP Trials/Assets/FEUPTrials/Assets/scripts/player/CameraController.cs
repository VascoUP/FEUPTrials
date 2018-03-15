﻿using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private GameObject _playerObject;

    public PlayerManager playerManager;

    public Vector3 offset;

    private void Start()
    {
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