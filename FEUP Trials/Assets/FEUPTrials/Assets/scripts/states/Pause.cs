using System;
using UnityEngine;

internal class Pause : IGameState
{
    public void OnEnter()
    {
    }

    public void Update()
    {
        Debug.Log("Pause Update");
    }
}