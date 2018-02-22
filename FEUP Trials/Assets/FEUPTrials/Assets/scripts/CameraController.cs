using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField]
    private GameObject _playerObject;

    public Vector3 offset;
    	
	void LateUpdate () {
        transform.position = _playerObject.transform.position + offset;
    }
}
