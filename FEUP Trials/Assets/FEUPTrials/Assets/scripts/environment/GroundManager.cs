using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour {
    [SerializeField]
    private GameObject _groundPrefab;
    private float _groundWidth = 10;
    private float _groundHeight = 10;
    
    private GameObject _playerOne;
    private GameObject _playerTwo;

    [SerializeField]
    private int _numberOfGroundObjectsPerPlayer;

    public Vector2 groundOffset;

    private Dictionary<float, GameObject> _groundObjects;

	// Use this for initialization
	void Start ()
    {
        _groundObjects = new Dictionary<float, GameObject>();

        if(_playerOne != null)
            UpdateGroundObjects(_playerOne);
    }

    public void SetPlayerOneObject(GameObject player)
    {
        _playerOne = player;
    }

    public void SetPlayerTwoObject(GameObject player)
    {
        _playerTwo = player;
    }

    void LateUpdate ()
    {
        if(_playerOne != null)
            UpdateGroundObjects(_playerOne);
        if(_playerTwo != null)
            UpdateGroundObjects(_playerTwo);
    }

    private void UpdateGroundObjects(GameObject player)
    {
        Vector3 playerPosition = player.transform.position;
        float snapBlockX = Mathf.Floor(playerPosition.x / _groundWidth);
        float firstBlockX = Mathf.Floor(snapBlockX - (_numberOfGroundObjectsPerPlayer - 3) / 2f);

        for(int i = 0; i < _numberOfGroundObjectsPerPlayer; i++)
        {
            float xPosition = (firstBlockX + i) * _groundWidth;
            if (IsGroundAlreadySpawned(xPosition))
                continue;
            GameObject spawnedObject = Instantiate(_groundPrefab, transform);
            spawnedObject.transform.position = new Vector3(xPosition + groundOffset.x - _groundWidth / 2f, groundOffset.y - _groundHeight / 2f);
            _groundObjects.Add(xPosition, spawnedObject);
        }
    }

    private bool IsGroundAlreadySpawned(float xPosition)
    {
        GameObject ground;
        _groundObjects.TryGetValue(xPosition, out ground);
        return ground != null;
    }
}
