using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInteract : MonoBehaviour, IInteract {
    private GameObject _enviromnent;
    [SerializeField]
    private GameObject _spawnObject;
    [SerializeField]
    private Vector3 _position;
    [SerializeField]
    private float _rotation;


	void Start () {
        _enviromnent = GameObject.Find("Enviromnent");
    }

    public void Channeling()
    {

    }

    public void Cancel()
    {

    }

    public void Completed()
    {
        // Spawn Object
        GameObject spawnedObject = Instantiate(_spawnObject, _position, Quaternion.AngleAxis(_rotation, Vector3.forward));
        spawnedObject.transform.parent = _enviromnent.transform;
    }
}
