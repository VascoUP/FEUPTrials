using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampInteract : MonoBehaviour, IInteract
{
    private Animator _animator;

    // Use this for initialization
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Channeling()
    {

    }

    public void Cancel()
    {

    }

    public void Completed()
    {
        _animator.Play("RampIntAnim");
    }
}
