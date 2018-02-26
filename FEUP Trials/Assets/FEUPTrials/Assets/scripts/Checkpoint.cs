using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    /*[SerializeField]
    private int _id;*/
    public bool active = false;
    [SerializeField]
    private Vector3 _spawnOffset;

    public Vector3 BikeNewPosition()
    {
        return transform.position + _spawnOffset;
    }
}
