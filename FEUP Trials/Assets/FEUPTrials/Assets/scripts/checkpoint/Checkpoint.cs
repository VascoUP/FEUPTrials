using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private PlayerManager _playerManager;

    public bool active = false;
    [SerializeField]
    public bool isFinish = false;
    [SerializeField]
    private Vector3 _spawnOffset;
    
    public void SetPlayerManager(PlayerManager playerManager)
    {
        _playerManager = playerManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bike" && collision.gameObject.layer == gameObject.layer + 1 && !active && _playerManager != null)
        {
            _playerManager.SetCheckpoint(this);
            ActivateCheckpoint();

            if (isFinish)
                _playerManager.FinalCheckpoint();
        }
    }

    public Vector3 BikeNewPosition()
    {
        return transform.position + _spawnOffset;
    }

    public void ActivateCheckpoint()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.Play("Activate");
    }
}
