using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public GroundOffset[] groundOffsets;
    public Vector2 groundOffset;
    
    private Dictionary<float, GameObject> _previousGroundObjects;
    
    void Start ()
    {
        _previousGroundObjects = new Dictionary<float, GameObject>();

        UpdateGroundObjects(true, true);
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
        UpdateGroundObjects(true, true);
    }
    
    private void UpdateGroundObjects(bool doP1, bool doP2)
    {
        HashSet<float> groundObjects = new HashSet<float>();
        
        if (_playerOne != null)
            CalcSpawnedBlocks(_playerOne, ref groundObjects);
        if (_playerTwo != null)
            CalcSpawnedBlocks(_playerTwo, ref groundObjects);

        UpdateSpawnedBlocks(ref groundObjects);
    }

    private void CalcSpawnedBlocks(GameObject player, ref HashSet<float> groundObjects)
    {
        Vector3 playerPosition = player.transform.position;
        float snapBlockX = Mathf.Floor(playerPosition.x / _groundWidth);
        float firstBlockX = Mathf.Floor(snapBlockX - (_numberOfGroundObjectsPerPlayer - 3) / 2f);
        
        for(int i = 0; i < _numberOfGroundObjectsPerPlayer; i++)
        {
            float xPosition = (firstBlockX + i) * _groundWidth;
            groundObjects.Add(xPosition);
        }
    }
    
    private void UpdateSpawnedBlocks(ref HashSet<float> groundObjects)
    {
        List<float> removableObjects = new List<float>();
        foreach (KeyValuePair<float, GameObject> entry in _previousGroundObjects)
        {
            if(groundObjects.Contains(entry.Key))
            {
                groundObjects.Remove(entry.Key);
            }
            else
            {
                Destroy(entry.Value);
                removableObjects.Add(entry.Key);
            }
        }

        foreach (float value in removableObjects)
        {
            _previousGroundObjects.Remove(value);
        }

        foreach(float value in groundObjects)
        {
            if(!_previousGroundObjects.ContainsKey(value))
            {            
                // Spawn a new object
                GameObject spawnedObject = Instantiate(_groundPrefab, transform);
                Vector2 offset = BlockOffset(value);
                spawnedObject.transform.position = new Vector3(value + offset.x - _groundWidth / 2f, offset.y - _groundHeight / 2f);
                _previousGroundObjects.Add(value, spawnedObject);
            }
        }
    }

    private Vector2 BlockOffset(float xPosition)
    {
        foreach(GroundOffset go in groundOffsets)
        {
            if (go.xPosition - go.offset.x >= xPosition)
                return go.offset;
        }
        return Vector2.zero;
    }
}

[Serializable]
public class GroundOffset
{
    public float xPosition;
    public Vector2 offset;
}