using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleInteractee : MonoBehaviour, IInteract
{
    [SerializeField]
    private Animator[] _animations;
    [SerializeField]
    private string[] _completeAnimationNames;

    public void Cancel()
    {}

    public void Channeling()
    {}

    public void Completed()
    {
        for(int i = 0; i < _animations.Length; i++)
        {
            _animations[i].Play(_completeAnimationNames[i]);
        }
    }
}
