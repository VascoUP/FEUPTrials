using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private GameObject _playerObject;

    public Vector3 offset;

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
