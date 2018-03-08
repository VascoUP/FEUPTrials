using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationInteractee : MonoBehaviour, IInteract
{
    private Animator _animator;
    [SerializeField]
    private string _completeAnimationName;

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
        _animator.Play(_completeAnimationName);
    }
}
