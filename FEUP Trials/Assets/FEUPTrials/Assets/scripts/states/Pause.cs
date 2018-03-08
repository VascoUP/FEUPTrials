using System;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class Pause : IGameState
{
    public void LoadScene()
    {
    }

    public void OnEnter()
    {
    }

    public void Update()
    {
        Debug.Log("Pause Update");
    }

    public void OnExit()
    {

    }
}