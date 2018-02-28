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

    public void ActivateCheckpoint()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.Play("CheckpointAnimation");
        else
            Debug.Log("null animator");
    }
}
